// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\shared\Seed\MasterDataSeed.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Cities.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Continents.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Countries.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Regions.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.shared.Seed;

public static class MasterDataSeed
{
    public static async Task SeedAsync(AppDbContext context)
    {
        var america = await EnsureContinentAsync(context, "América");
        await EnsureContinentAsync(context, "Europa");
        await EnsureContinentAsync(context, "Asia");
        await EnsureContinentAsync(context, "África");
        await EnsureContinentAsync(context, "Oceanía");
        await EnsureContinentAsync(context, "Antártida");

        var colombia = await EnsureCountryAsync(context, "Colombia", "COL", america.Id);
        var usa = await EnsureCountryAsync(context, "Estados Unidos", "USA", america.Id);

        var cundinamarca = await EnsureRegionAsync(context, "Cundinamarca", "Departamento", colombia.Id);
        var antioquia = await EnsureRegionAsync(context, "Antioquia", "Departamento", colombia.Id);
        var florida = await EnsureRegionAsync(context, "Florida", "Estado", usa.Id);

        var bogota = await EnsureCityAsync(context, "Bogotá", cundinamarca.Id);
        var medellin = await EnsureCityAsync(context, "Medellín", antioquia.Id);
        var miami = await EnsureCityAsync(context, "Miami", florida.Id);

        await EnsureAirportAsync(context, "Aeropuerto Internacional El Dorado", "BOG", "SKBO", bogota.Id);
        await EnsureAirportAsync(context, "Aeropuerto Internacional José María Córdova", "MDE", "SKRG", medellin.Id);
        await EnsureAirportAsync(context, "Miami International Airport", "MIA", "KMIA", miami.Id);
    }

    private static async Task<ContinentEntity> EnsureContinentAsync(AppDbContext context, string name)
    {
        var norm = SeedHelpers.Normalize(name);
        var existing = await context.Continents
            .FirstOrDefaultAsync(x => x.Name != null && x.Name.Trim().ToUpper() == norm);

        if (existing is not null)
            return existing;

        var maxId = await context.Continents.AsNoTracking().Select(x => (int?)x.Id).MaxAsync() ?? 0;
        var entity = new ContinentEntity { Id = maxId + 1, Name = name };
        context.Continents.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    private static async Task<CountryEntity> EnsureCountryAsync(AppDbContext context, string name, string iso, int continentId)
    {
        var isoNorm = SeedHelpers.Normalize(iso);
        var existing = await context.Countries
            .FirstOrDefaultAsync(x => x.IsoCode != null && x.IsoCode.Trim().ToUpper() == isoNorm);

        if (existing is not null)
            return existing;

        var entity = new CountryEntity { Name = name, IsoCode = iso, ContinentId = continentId };
        context.Countries.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    private static async Task<RegionEntity> EnsureRegionAsync(AppDbContext context, string name, string type, int countryId)
    {
        var norm = SeedHelpers.Normalize(name);
        var existing = await context.Regions
            .FirstOrDefaultAsync(x =>
                x.CountryId == countryId &&
                x.Name != null &&
                x.Name.Trim().ToUpper() == norm);

        if (existing is not null)
            return existing;

        var entity = new RegionEntity { Name = name, Type = type, CountryId = countryId };
        context.Regions.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    private static async Task<CityEntity> EnsureCityAsync(AppDbContext context, string name, int regionId)
    {
        var norm = SeedHelpers.Normalize(name);
        var existing = await context.Cities
            .FirstOrDefaultAsync(x =>
                x.RegionId == regionId &&
                x.Name != null &&
                x.Name.Trim().ToUpper() == norm);

        if (existing is not null)
            return existing;

        var entity = new CityEntity { Name = name, RegionId = regionId };
        context.Cities.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    private static async Task<AirportEntity> EnsureAirportAsync(AppDbContext context, string name, string iata, string? icao, int cityId)
    {
        var iataNorm = SeedHelpers.Normalize(iata);
        var existing = await context.Airports
            .FirstOrDefaultAsync(x => x.IataCode != null && x.IataCode.Trim().ToUpper() == iataNorm);

        if (existing is not null)
            return existing;

        var entity = new AirportEntity { Name = name, IataCode = iata, IcaoCode = icao, CityId = cityId };
        context.Airports.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}
