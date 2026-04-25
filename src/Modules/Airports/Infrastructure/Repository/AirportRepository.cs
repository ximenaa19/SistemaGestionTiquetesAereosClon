// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\Infrastructure\Repository\AirportRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Domain.Aggregate;
using GestionAerolineas.src.Modules.Airports.Domain.Repositories;
using GestionAerolineas.src.Modules.Airports.Domain.ValueObject;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;

public class AirportRepository : IAirportRepository
{
    private readonly AppDbContext _context;

    public AirportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Airport>> GetAllAsync()
    {
        var entities = await _context.Airports.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Airport?> GetByIdAsync(AirportId id)
    {
        var entity = await _context.Airports
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Airport?> GetByNameAsync(AirportName name)
    {
        var entity = await _context.Airports
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(Airport airport)
    {
        await _context.Airports.AddAsync(MapToEntity(airport));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Airport airport)
    {
        var existing = await _context.Airports
            .FirstOrDefaultAsync(e => e.Id == airport.Id.Value);

        if (existing is null)
            return;

        existing.Name = airport.Name.Value;
        existing.IataCode = airport.IataCode.Value;
        existing.IcaoCode = airport.IcaoCode?.Value;
        existing.CityId = airport.CityId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Airport airport)
    {
        var entity = await _context.Airports.FindAsync(airport.Id.Value);

        if (entity is null)
            return;

        _context.Airports.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(AirportId id)
    {
        return _context.Airports.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByNormalizedNameInCityAsync(string normalizedName, int cityId, int? excludingId = null)
    {
        var query = _context.Airports
            .AsNoTracking()
            .Where(a => a.CityId == cityId && a.Name != null);

        if (excludingId.HasValue)
            query = query.Where(a => a.Id != excludingId.Value);

        return query.AnyAsync(a => a.Name!.Trim().ToUpper() == normalizedName);
    }

    public Task<bool> ExistsByNormalizedIataCodeAsync(string normalizedIataCode, int? excludingId = null)
    {
        var query = _context.Airports
            .AsNoTracking()
            .Where(a => a.IataCode != null);

        if (excludingId.HasValue)
            query = query.Where(a => a.Id != excludingId.Value);

        return query.AnyAsync(a => a.IataCode!.Trim().ToUpper() == normalizedIataCode);
    }

    public Task<bool> ExistsByNormalizedIcaoCodeAsync(string normalizedIcaoCode, int? excludingId = null)
    {
        var query = _context.Airports
            .AsNoTracking()
            .Where(a => a.IcaoCode != null);

        if (excludingId.HasValue)
            query = query.Where(a => a.Id != excludingId.Value);

        return query.AnyAsync(a => a.IcaoCode!.Trim().ToUpper() == normalizedIcaoCode);
    }

    private static Airport MapToDomain(AirportEntity entity)
    {
        try
        {
            return Airport.Create(
                AirportId.Create(entity.Id),
                AirportName.Create(entity.Name ?? string.Empty),
                AirportIataCode.Create(entity.IataCode ?? string.Empty),
                AirportIcaoCode.CreateOptional(entity.IcaoCode),
                AirportCityId.Create(entity.CityId)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro airports(id={entity.Id}) tiene datos invalidos. " +
                $"nombre='{entity.Name}', codigo_iata='{entity.IataCode}', codigo_icao='{entity.IcaoCode}', ciudad_id={entity.CityId}.",
                ex);
        }
    }

    private static AirportEntity MapToEntity(Airport airport)
    {
        return new AirportEntity
        {
            Id = airport.Id.Value,
            Name = airport.Name.Value,
            IataCode = airport.IataCode.Value,
            IcaoCode = airport.IcaoCode?.Value,
            CityId = airport.CityId.Value
        };
    }
}
