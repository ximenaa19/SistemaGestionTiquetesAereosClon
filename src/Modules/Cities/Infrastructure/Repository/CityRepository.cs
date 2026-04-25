// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Cities\Infrastructure\Repository\CityRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Cities.Domain.Aggregate;
using GestionAerolineas.src.Modules.Cities.Domain.Repositories;
using GestionAerolineas.src.Modules.Cities.Domain.ValueObject;
using GestionAerolineas.src.Modules.Cities.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Cities.Infrastructure.Repository;

public class CityRepository : ICityRepository
{
    private readonly AppDbContext _context;

    public CityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<City>> GetAllAsync()
    {
        var entities = await _context.Cities.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<City?> GetByIdAsync(CityId id)
    {
        var entity = await _context.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<City?> GetByNameAsync(CityName name)
    {
        var entity = await _context.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(City city)
    {
        await _context.Cities.AddAsync(MapToEntity(city));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(City city)
    {
        var existing = await _context.Cities
            .FirstOrDefaultAsync(e => e.Id == city.Id.Value);

        if (existing is null)
            return;

        existing.Name = city.Name.Value;
        existing.RegionId = city.RegionId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(City city)
    {
        var entity = await _context.Cities.FindAsync(city.Id.Value);

        if (entity is null)
            return;

        _context.Cities.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(CityId id)
    {
        return _context.Cities.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByNormalizedNameInRegionAsync(string normalizedName, int regionId, int? excludingId = null)
    {
        var query = _context.Cities
            .AsNoTracking()
            .Where(c => c.RegionId == regionId && c.Name != null);

        if (excludingId.HasValue)
            query = query.Where(c => c.Id != excludingId.Value);

        return query.AnyAsync(c => c.Name!.Trim().ToUpper() == normalizedName);
    }

    private static City MapToDomain(CityEntity entity)
    {
        try
        {
            return City.Create(
                CityId.Create(entity.Id),
                CityName.Create(entity.Name ?? string.Empty),
                CityRegionId.Create(entity.RegionId)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro cities(id={entity.Id}) tiene datos invalidos. " +
                $"nombre='{entity.Name}', region_id={entity.RegionId}.",
                ex);
        }
    }

    private static CityEntity MapToEntity(City city)
    {
        return new CityEntity
        {
            Id = city.Id.Value,
            Name = city.Name.Value,
            RegionId = city.RegionId.Value
        };
    }
}
