// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\Infrastructure\Repository\AirlineRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Domain.Aggregate;
using GestionAerolineas.src.Modules.Airlines.Domain.Repositories;
using GestionAerolineas.src.Modules.Airlines.Domain.ValueObject;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;

public class AirlineRepository : IAirlineRepository
{
    private readonly AppDbContext _context;

    public AirlineRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Airline>> GetAllAsync()
    {
        var entities = await _context.Airlines.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Airline?> GetByIdAsync(AirlineId id)
    {
        var entity = await _context.Airlines
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Airline?> GetByNameAsync(AirlineName name)
    {
        var entity = await _context.Airlines
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(Airline airline)
    {
        await _context.Airlines.AddAsync(MapToEntity(airline));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Airline airline)
    {
        var existing = await _context.Airlines
            .FirstOrDefaultAsync(e => e.Id == airline.Id.Value);

        if (existing is null)
            return;

        existing.Name = airline.Name.Value;
        existing.IataCode = airline.IataCode.Value;
        existing.OriginCountryId = airline.OriginCountryId.Value;
        existing.IsActive = airline.IsActive.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Airline airline)
    {
        var entity = await _context.Airlines.FindAsync(airline.Id.Value);

        if (entity is null)
            return;

        _context.Airlines.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(AirlineId id)
    {
        return _context.Airlines.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByNormalizedNameInOriginCountryAsync(string normalizedName, int originCountryId, int? excludingId = null)
    {
        var query = _context.Airlines
            .AsNoTracking()
            .Where(a => a.OriginCountryId == originCountryId && a.Name != null);

        if (excludingId.HasValue)
            query = query.Where(a => a.Id != excludingId.Value);

        return query.AnyAsync(a => a.Name!.Trim().ToUpper() == normalizedName);
    }

    public Task<bool> ExistsByNormalizedIataCodeAsync(string normalizedIataCode, int? excludingId = null)
    {
        var query = _context.Airlines
            .AsNoTracking()
            .Where(a => a.IataCode != null);

        if (excludingId.HasValue)
            query = query.Where(a => a.Id != excludingId.Value);

        return query.AnyAsync(a => a.IataCode!.Trim().ToUpper() == normalizedIataCode);
    }

    private static Airline MapToDomain(AirlineEntity entity)
    {
        try
        {
            return Airline.Create(
                AirlineId.Create(entity.Id),
                AirlineName.Create(entity.Name ?? string.Empty),
                AirlineIataCode.Create(entity.IataCode ?? string.Empty),
                AirlineOriginCountryId.Create(entity.OriginCountryId),
                AirlineIsActive.Create(entity.IsActive)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro airlines(id={entity.Id}) tiene datos invalidos. " +
                $"nombre='{entity.Name}', codigo_iata='{entity.IataCode}', pais_origen_id={entity.OriginCountryId}, activa={entity.IsActive}.",
                ex);
        }
    }

    private static AirlineEntity MapToEntity(Airline airline)
    {
        return new AirlineEntity
        {
            Id = airline.Id.Value,
            Name = airline.Name.Value,
            IataCode = airline.IataCode.Value,
            OriginCountryId = airline.OriginCountryId.Value,
            IsActive = airline.IsActive.Value
        };
    }
}

