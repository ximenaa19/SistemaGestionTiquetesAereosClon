// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinTypes\Infrastructure\Repository\CabinTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using GestionAerolineas.src.Modules.CabinTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.CabinTypes.Domain.Repository;
using GestionAerolineas.src.Modules.CabinTypes.Domain.ValueObject;
using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Repository;

public class CabinTypeRepository : ICabinTypeRepository
{
    private readonly AppDbContext _context;

    public CabinTypeRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<CabinType>> GetAllAsync()
    {
        var entities = await _context.CabinTypes
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<CabinType?> GetByIdAsync(CabinTypesId id)
    {
        var entity = await _context.CabinTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<CabinType?> GetByNameAsync(CabinTypesName name)
    {
        var entity = await _context.CabinTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(CabinType cabinType)
    {
        await _context.CabinTypes.AddAsync(MapToEntity(cabinType));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CabinType cabinType)
    {
        var existing = await _context.CabinTypes
            .FirstOrDefaultAsync(e => e.Id == cabinType.Id.Value);

        if (existing is null)
            return;

        // 🔥 Actualizar la misma instancia (no crear una nueva)
        existing.Name = cabinType.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CabinType cabinType)
    {
        var entity = await _context.CabinTypes.FindAsync(cabinType.Id.Value);

        if (entity is null)
        {
            return;
        }

        _context.CabinTypes.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(CabinTypesId id)
    {
        return await _context.CabinTypes.AnyAsync(e => e.Id == id.Value);
    }

    private static CabinType MapToDomain(CabinTypeEntity entity)
    {
        return CabinType.Create(
            CabinTypesId.Create(entity.Id),
            CabinTypesName.Create(entity.Name ?? string.Empty)
        );
    }

    private static CabinTypeEntity MapToEntity(CabinType cabinType)
    {
        return new CabinTypeEntity
        {
            Id = cabinType.Id.Value,
            Name = cabinType.Name.Value
        };
    }

}
