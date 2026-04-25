// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Continents\Infrastructure\Repository\ContinentRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Continents.Domain.Aggregate;
using GestionAerolineas.src.Modules.Continents.Domain.Repositories;
using GestionAerolineas.src.Modules.Continents.Domain.ValueObject;
using GestionAerolineas.src.Modules.Continents.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Continents.Infrastructure.Repository;

public class ContinentRepository : IContinentRepository
{
    private readonly AppDbContext _context;

    public ContinentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Continent>> GetAllAsync()
    {
        var entities = await _context.Continents.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<Continent?> GetByIdAsync(ContinentId id)
    {
        var entity = await _context.Continents
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Continent?> GetByNameAsync(ContinentName name)
    {
        var entity = await _context.Continents
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(Continent continent)
    {
        await _context.Continents.AddAsync(MapToEntity(continent));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Continent continent)
    {
        var existing = await _context.Continents
            .FirstOrDefaultAsync(e => e.Id == continent.Id.Value);

        if (existing is null)
            return;

        existing.Name = continent.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Continent continent)
    {
        var entity = await _context.Continents.FindAsync(continent.Id.Value);

        if (entity is null) return;

        _context.Continents.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(ContinentId id)
    {
        return await _context.Continents.AnyAsync(e => e.Id == id.Value);
    }

    private static Continent MapToDomain(ContinentEntity entity)
    {
        return Continent.Create(
            ContinentId.Create(entity.Id),
            ContinentName.Create(entity.Name ?? "")
        );
    }

    private static ContinentEntity MapToEntity(Continent entity)
    {
        return new ContinentEntity
        {
            Id = entity.Id.Value,
            Name = entity.Name.Value
        };
    }
}
