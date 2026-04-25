// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AvailabilityStatuses\Infrastructure\Repository\AvailabilityStatusRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.ValueObject;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.AvailabilityStatuses.Infrastructure.Repository;

public class AvailabilityStatusRepository : IAvailabilityStatusRepository
{
    private readonly AppDbContext _context;

    public AvailabilityStatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AvailabilityStatus>> GetAllAsync()
    {
        var entities = await _context.AvailabilityStatuses
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<AvailabilityStatus?> GetByIdAsync(AvailabilityStatusId id)
    {
        var entity = await _context.AvailabilityStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<AvailabilityStatus?> GetByNameAsync(AvailabilityStatusName name)
    {
        var entity = await _context.AvailabilityStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(AvailabilityStatus availabilityStatus)
    {
        await _context.AvailabilityStatuses.AddAsync(MapToEntity(availabilityStatus));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AvailabilityStatus availabilityStatus)
    {
        var existing = await _context.AvailabilityStatuses
            .FirstOrDefaultAsync(e => e.Id == availabilityStatus.Id.Value);

        if (existing is null)
            return;

        existing.Name = availabilityStatus.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(AvailabilityStatus availabilityStatus)
    {
        var entity = await _context.AvailabilityStatuses.FindAsync(availabilityStatus.Id.Value);

        if (entity is null)
            return;

        _context.AvailabilityStatuses.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(AvailabilityStatusId id)
    {
        return _context.AvailabilityStatuses.AnyAsync(e => e.Id == id.Value);
    }

    private static AvailabilityStatus MapToDomain(AvailabilityStatusEntity entity)
    {
        return AvailabilityStatus.Create(
            AvailabilityStatusId.Create(entity.Id),
            AvailabilityStatusName.Create(entity.Name ?? string.Empty)
        );
    }

    private static AvailabilityStatusEntity MapToEntity(AvailabilityStatus availabilityStatus)
    {
        return new AvailabilityStatusEntity
        {
            Id = availabilityStatus.Id.Value,
            Name = availabilityStatus.Name.Value
        };
    }
}
