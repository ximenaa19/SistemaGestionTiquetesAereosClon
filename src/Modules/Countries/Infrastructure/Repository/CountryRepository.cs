// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Countries\Infrastructure\Repository\CountryRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Countries.Domain.Aggregate;
using GestionAerolineas.src.Modules.Countries.Domain.Repositories;
using GestionAerolineas.src.Modules.Countries.Domain.ValueObject;
using GestionAerolineas.src.Modules.Countries.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Countries.Infrastructure.Repository;

public class CountryRepository : ICountryRepository
{
    private readonly AppDbContext _context;

    public CountryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        var entities = await _context.Countries.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Country?> GetByIdAsync(CountryId id)
    {
        var entity = await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Country?> GetByNameAsync(CountryName name)
    {
        var entity = await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Country?> GetByIsoCodeAsync(CountryCodigoIso isoCode)
    {
        var entity = await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IsoCode == isoCode.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(Country country)
    {
        await _context.Countries.AddAsync(MapToEntity(country));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Country country)
    {
        var existing = await _context.Countries
            .FirstOrDefaultAsync(e => e.Id == country.Id.Value);

        if (existing is null)
            return;

        existing.Name = country.Name.Value;
        existing.IsoCode = country.IsoCode.Value;
        existing.ContinentId = country.ContinentId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Country country)
    {
        var entity = await _context.Countries.FindAsync(country.Id.Value);

        if (entity is null)
            return;

        _context.Countries.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(CountryId id)
    {
        return _context.Countries.AnyAsync(e => e.Id == id.Value);
    }

    private static Country MapToDomain(CountryEntity entity)
    {
        return Country.Create(
            CountryId.Create(entity.Id),
            CountryName.Create(entity.Name ?? string.Empty),
            CountryCodigoIso.Create(entity.IsoCode ?? string.Empty),
            CountryContinentId.Create(entity.ContinentId)
        );
    }

    private static CountryEntity MapToEntity(Country country)
    {
        return new CountryEntity
        {
            Id = country.Id.Value,
            Name = country.Name.Value,
            IsoCode = country.IsoCode.Value,
            ContinentId = country.ContinentId.Value
        };
    }
}

