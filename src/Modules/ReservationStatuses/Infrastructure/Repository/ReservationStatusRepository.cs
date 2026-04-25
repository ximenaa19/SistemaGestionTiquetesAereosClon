// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatuses\Infrastructure\Repository\ReservationStatusRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;

public class ReservationStatusRepository : IReservationStatusRepository
{
    private readonly AppDbContext _context;

    public ReservationStatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReservationStatus>> GetAllAsync()
    {
        var entities = await _context.ReservationStatuses.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<ReservationStatus?> GetByIdAsync(ReservationStatusId id)
    {
        var entity = await _context.ReservationStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<ReservationStatus?> GetByNameAsync(ReservationStatusName name)
    {
        var entity = await _context.ReservationStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(ReservationStatus reservationStatus)
    {
        await _context.ReservationStatuses.AddAsync(MapToEntity(reservationStatus));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ReservationStatus reservationStatus)
    {
        var existing = await _context.ReservationStatuses
            .FirstOrDefaultAsync(e => e.Id == reservationStatus.Id.Value);

        if (existing is null)
            return;

        existing.Name = reservationStatus.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ReservationStatus reservationStatus)
    {
        var entity = await _context.ReservationStatuses.FindAsync(reservationStatus.Id.Value);

        if (entity is null)
            return;

        _context.ReservationStatuses.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(ReservationStatusId id)
    {
        return _context.ReservationStatuses.AnyAsync(e => e.Id == id.Value);
    }

    private static ReservationStatus MapToDomain(ReservationStatusEntity entity)
    {
        return ReservationStatus.Create(
            ReservationStatusId.Create(entity.Id),
            ReservationStatusName.Create(entity.Name ?? string.Empty)
        );
    }

    private static ReservationStatusEntity MapToEntity(ReservationStatus reservationStatus)
    {
        return new ReservationStatusEntity
        {
            Id = reservationStatus.Id.Value,
            Name = reservationStatus.Name.Value
        };
    }
}

