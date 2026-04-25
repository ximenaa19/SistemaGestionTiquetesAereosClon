// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\Infrastructure\Repository\ReservationStatusTransitionRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.Infrastructure.Repository;

public class ReservationStatusTransitionRepository : IReservationStatusTransitionRepository
{
    private readonly AppDbContext _context;

    public ReservationStatusTransitionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReservationStatusTransition>> GetAllAsync()
    {
        var entities = await _context.Set<ReservationStatusTransitionEntity>()
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<ReservationStatusTransition?> GetByIdAsync(ReservationStatusTransitionId id)
    {
        var entity = await _context.Set<ReservationStatusTransitionEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<ReservationStatusTransition?> GetByPairAsync(
        ReservationStatusOriginId originStatusId,
        ReservationStatusDestinationId destinationStatusId)
    {
        var entity = await _context.Set<ReservationStatusTransitionEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.OriginStatusId == originStatusId.Value &&
                e.DestinationStatusId == destinationStatusId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(ReservationStatusTransition transition)
    {
        await _context.Set<ReservationStatusTransitionEntity>().AddAsync(MapToEntity(transition));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ReservationStatusTransition transition)
    {
        var existing = await _context.Set<ReservationStatusTransitionEntity>()
            .FirstOrDefaultAsync(e => e.Id == transition.Id.Value);

        if (existing is null)
            return;

        existing.OriginStatusId = transition.OriginStatusId.Value;
        existing.DestinationStatusId = transition.DestinationStatusId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ReservationStatusTransition transition)
    {
        var entity = await _context.Set<ReservationStatusTransitionEntity>().FindAsync(transition.Id.Value);

        if (entity is null)
            return;

        _context.Set<ReservationStatusTransitionEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(ReservationStatusTransitionId id)
    {
        return _context.Set<ReservationStatusTransitionEntity>().AnyAsync(e => e.Id == id.Value);
    }

    private static ReservationStatusTransition MapToDomain(ReservationStatusTransitionEntity entity)
    {
        return ReservationStatusTransition.Create(
            ReservationStatusTransitionId.Create(entity.Id),
            ReservationStatusOriginId.Create(entity.OriginStatusId),
            ReservationStatusDestinationId.Create(entity.DestinationStatusId)
        );
    }

    private static ReservationStatusTransitionEntity MapToEntity(ReservationStatusTransition transition)
    {
        return new ReservationStatusTransitionEntity
        {
            Id = transition.Id.Value,
            OriginStatusId = transition.OriginStatusId.Value,
            DestinationStatusId = transition.DestinationStatusId.Value
        };
    }
}
