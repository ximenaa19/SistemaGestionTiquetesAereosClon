// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Regions\Infrastructure\Repository\RegionRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Regions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Regions.Domain.Repositories;
using GestionAerolineas.src.Modules.Regions.Domain.ValueObject;
using GestionAerolineas.src.Modules.Regions.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Regions.Infrastructure.Repository;

public class RegionRepository : IRegionRepository
{
    private readonly AppDbContext _context;

    public RegionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Region>> GetAllAsync()
    {
        var entities = await _context.Regions.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Region?> GetByIdAsync(RegionId id)
    {
        var entity = await _context.Regions
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Region?> GetByNameAsync(RegionName name)
    {
        var entity = await _context.Regions
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(Region region)
    {
        await _context.Regions.AddAsync(MapToEntity(region));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Region region)
    {
        var existing = await _context.Regions
            .FirstOrDefaultAsync(e => e.Id == region.Id.Value);

        if (existing is null)
            return;

        existing.Name = region.Name.Value;
        existing.Type = region.Type.Value;
        existing.CountryId = region.CountryId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Region region)
    {
        var entity = await _context.Regions.FindAsync(region.Id.Value);

        if (entity is null)
            return;

        _context.Regions.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(RegionId id)
    {
        return _context.Regions.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByNormalizedNameInCountryAsync(string normalizedName, int countryId, int? excludingId = null)
    {
        var query = _context.Regions
            .AsNoTracking()
            .Where(r => r.CountryId == countryId && r.Name != null);

        if (excludingId.HasValue)
            query = query.Where(r => r.Id != excludingId.Value);

        return query.AnyAsync(r => r.Name!.Trim().ToUpper() == normalizedName);
    }

    private static Region MapToDomain(RegionEntity entity)
    {
        try
        {
            return Region.Create(
                RegionId.Create(entity.Id),
                RegionName.Create(entity.Name ?? string.Empty),
                RegionType.Create(entity.Type ?? string.Empty),
                RegionCountryId.Create(entity.CountryId)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro regions(id={entity.Id}) tiene datos inválidos. " +
                $"nombre='{entity.Name}', tipo='{entity.Type}', pais_id={entity.CountryId}.",
                ex);
        }
    }

    private static RegionEntity MapToEntity(Region region)
    {
        return new RegionEntity
        {
            Id = region.Id.Value,
            Name = region.Name.Value,
            Type = region.Type.Value,
            CountryId = region.CountryId.Value
        };
    }
}
