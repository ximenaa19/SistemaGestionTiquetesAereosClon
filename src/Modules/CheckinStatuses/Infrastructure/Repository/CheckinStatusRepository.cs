// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CheckinStatuses\Infrastructure\Repository\CheckinStatusRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.ValueObject;
using GestionAerolineas.src.Modules.CheckinStatuses.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.CheckinStatuses.Infrastructure.Repository;

public class CheckinStatusRepository : ICheckinStatusRepository
{
    private readonly AppDbContext _context;

    public CheckinStatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CheckinStatus>> GetAllAsync()
    {
        var entities = await _context.CheckinStatuses
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<CheckinStatus?> GetByIdAsync(CheckinStatusId id)
    {
        var entity = await _context.CheckinStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<CheckinStatus?> GetByNameAsync(CheckinStatusName name)
    {
        var entity = await _context.CheckinStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(CheckinStatus checkinStatus)
    {
        await _context.CheckinStatuses.AddAsync(MapToEntity(checkinStatus));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CheckinStatus checkinStatus)
    {
        var existing = await _context.CheckinStatuses
            .FirstOrDefaultAsync(e => e.Id == checkinStatus.Id.Value);

        if (existing is null)
            return;

        existing.Name = checkinStatus.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CheckinStatus checkinStatus)
    {
        var entity = await _context.CheckinStatuses.FindAsync(checkinStatus.Id.Value);

        if (entity is null)
            return;

        _context.CheckinStatuses.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(CheckinStatusId id)
    {
        return _context.CheckinStatuses.AnyAsync(e => e.Id == id.Value);
    }

    private static CheckinStatus MapToDomain(CheckinStatusEntity entity)
    {
        return CheckinStatus.Create(
            CheckinStatusId.Create(entity.Id),
            CheckinStatusName.Create(entity.Name ?? string.Empty)
        );
    }

    private static CheckinStatusEntity MapToEntity(CheckinStatus checkinStatus)
    {
        return new CheckinStatusEntity
        {
            Id = checkinStatus.Id.Value,
            Name = checkinStatus.Name.Value
        };
    }
}
