// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SeatLocationTypes\Infrastructure\Repository\SeatLocationTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.ValueObject;
using GestionAerolineas.src.Modules.SeatLocationTypes.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.SeatLocationTypes.Infrastructure.Repository;

public class SeatLocationTypeRepository : ISeatLocationTypeRepository
{
    private readonly AppDbContext _context;

    public SeatLocationTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SeatLocationType>> GetAllAsync()
    {
        var entities = await _context.SeatLocationTypes.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<SeatLocationType?> GetByIdAsync(SeatLocationTypeId id)
    {
        var entity = await _context.SeatLocationTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<SeatLocationType?> GetByNameAsync(SeatLocationTypeName name)
    {
        var entity = await _context.SeatLocationTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public Task<int> CountAsync()
    {
        return _context.SeatLocationTypes.CountAsync();
    }

    public async Task AddAsync(SeatLocationType seatLocationType)
    {
        await _context.SeatLocationTypes.AddAsync(MapToEntity(seatLocationType));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SeatLocationType seatLocationType)
    {
        var existing = await _context.SeatLocationTypes
            .FirstOrDefaultAsync(e => e.Id == seatLocationType.Id.Value);

        if (existing is null)
            return;

        existing.Name = seatLocationType.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(SeatLocationType seatLocationType)
    {
        var entity = await _context.SeatLocationTypes.FindAsync(seatLocationType.Id.Value);

        if (entity is null)
            return;

        _context.SeatLocationTypes.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(SeatLocationTypeId id)
    {
        return _context.SeatLocationTypes.AnyAsync(e => e.Id == id.Value);
    }

    private static SeatLocationType MapToDomain(SeatLocationTypeEntity entity)
    {
        return SeatLocationType.Create(
            SeatLocationTypeId.Create(entity.Id),
            SeatLocationTypeName.Create(entity.Name ?? string.Empty)
        );
    }

    private static SeatLocationTypeEntity MapToEntity(SeatLocationType seatLocationType)
    {
        return new SeatLocationTypeEntity
        {
            Id = seatLocationType.Id.Value,
            Name = seatLocationType.Name.Value
        };
    }
}

