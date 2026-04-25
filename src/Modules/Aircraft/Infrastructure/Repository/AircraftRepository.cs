// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\Infrastructure\Repository\AircraftRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Domain.Aggregate;
using GestionAerolineas.src.Modules.Aircraft.Domain.Repositories;
using GestionAerolineas.src.Modules.Aircraft.Domain.ValueObject;
using GestionAerolineas.src.Modules.Aircraft.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Aircraft.Infrastructure.Repository;

public class AircraftRepository : IAircraftRepository
{
    private readonly AppDbContext _context;

    public AircraftRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AircraftAggregate>> GetAllAsync()
    {
        var entities = await _context.Aircraft.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<AircraftAggregate?> GetByIdAsync(AircraftId id)
    {
        var entity = await _context.Aircraft
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<AircraftAggregate?> GetByRegistrationAsync(AircraftRegistration registration)
    {
        var entity = await _context.Aircraft
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Registration == registration.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(AircraftAggregate aircraft)
    {
        await _context.Aircraft.AddAsync(MapToEntity(aircraft));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AircraftAggregate aircraft)
    {
        var existing = await _context.Aircraft
            .FirstOrDefaultAsync(e => e.Id == aircraft.Id.Value);

        if (existing is null)
            return;

        existing.ModelId = aircraft.ModelId.Value;
        existing.AirlineId = aircraft.AirlineId.Value;
        existing.Registration = aircraft.Registration.Value;
        existing.ManufactureDate = aircraft.ManufactureDate.Value;
        existing.IsActive = aircraft.IsActive.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(AircraftAggregate aircraft)
    {
        var entity = await _context.Aircraft.FindAsync(aircraft.Id.Value);

        if (entity is null)
            return;

        _context.Aircraft.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(AircraftId id)
    {
        return _context.Aircraft.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByNormalizedRegistrationAsync(string normalizedRegistration, int? excludingId = null)
    {
        var query = _context.Aircraft
            .AsNoTracking()
            .Where(a => a.Registration != null);

        if (excludingId.HasValue)
            query = query.Where(a => a.Id != excludingId.Value);

        return query.AnyAsync(a => a.Registration!.Trim().ToUpper() == normalizedRegistration);
    }

    private static AircraftAggregate MapToDomain(AircraftEntity entity)
    {
        try
        {
            return AircraftAggregate.Create(
                AircraftId.Create(entity.Id),
                AircraftModelId.Create(entity.ModelId),
                AircraftAirlineId.Create(entity.AirlineId),
                AircraftRegistration.Create(entity.Registration ?? string.Empty),
                AircraftManufactureDate.Create(entity.ManufactureDate),
                AircraftIsActive.Create(entity.IsActive)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro aircraft(id={entity.Id}) tiene datos invalidos. " +
                $"modelo_id={entity.ModelId}, aerolinea_id={entity.AirlineId}, " +
                $"matricula='{entity.Registration}', fecha_fabricacion='{entity.ManufactureDate}', activa={entity.IsActive}.",
                ex);
        }
    }

    private static AircraftEntity MapToEntity(AircraftAggregate aircraft)
    {
        return new AircraftEntity
        {
            Id = aircraft.Id.Value,
            ModelId = aircraft.ModelId.Value,
            AirlineId = aircraft.AirlineId.Value,
            Registration = aircraft.Registration.Value,
            ManufactureDate = aircraft.ManufactureDate.Value,
            IsActive = aircraft.IsActive.Value
        };
    }
}

