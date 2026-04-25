// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStatusTransitions\Infrastructure\Repository\FlightStatusTransitionRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.ValueObject;
using GestionAerolineas.src.Modules.FlightStatusTransitions.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.FlightStatusTransitions.Infrastructure.Repository;

public class FlightStatusTransitionRepository : IFlightStatusTransitionRepository
{
    private readonly AppDbContext _context;

    public FlightStatusTransitionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FlightStatusTransition>> GetAllAsync()
    {
        var entities = await _context.Set<FlightStatusTransitionEntity>()
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<FlightStatusTransition?> GetByIdAsync(FlightStatusTransitionId id)
    {
        var entity = await _context.Set<FlightStatusTransitionEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<FlightStatusTransition?> GetByPairAsync(
        FlightStateOriginId originStateId,
        FlightStateDestinationId destinationStateId)
    {
        var entity = await _context.Set<FlightStatusTransitionEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.OriginStateId == originStateId.Value &&
                e.DestinationStateId == destinationStateId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(FlightStatusTransition transition)
    {
        await _context.Set<FlightStatusTransitionEntity>().AddAsync(MapToEntity(transition));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(FlightStatusTransition transition)
    {
        var existing = await _context.Set<FlightStatusTransitionEntity>()
            .FirstOrDefaultAsync(e => e.Id == transition.Id.Value);

        if (existing is null)
            return;

        existing.OriginStateId = transition.OriginStateId.Value;
        existing.DestinationStateId = transition.DestinationStateId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(FlightStatusTransition transition)
    {
        var entity = await _context.Set<FlightStatusTransitionEntity>().FindAsync(transition.Id.Value);

        if (entity is null)
            return;

        _context.Set<FlightStatusTransitionEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(FlightStatusTransitionId id)
    {
        return _context.Set<FlightStatusTransitionEntity>().AnyAsync(e => e.Id == id.Value);
    }

    private static FlightStatusTransition MapToDomain(FlightStatusTransitionEntity entity)
    {
        return FlightStatusTransition.Create(
            FlightStatusTransitionId.Create(entity.Id),
            FlightStateOriginId.Create(entity.OriginStateId),
            FlightStateDestinationId.Create(entity.DestinationStateId)
        );
    }

    private static FlightStatusTransitionEntity MapToEntity(FlightStatusTransition transition)
    {
        return new FlightStatusTransitionEntity
        {
            Id = transition.Id.Value,
            OriginStateId = transition.OriginStateId.Value,
            DestinationStateId = transition.DestinationStateId.Value
        };
    }
}

