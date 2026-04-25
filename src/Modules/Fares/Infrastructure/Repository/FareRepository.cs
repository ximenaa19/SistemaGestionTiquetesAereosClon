// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Infrastructure\Repository\FareRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Fares.Domain.Aggregate;
using GestionAerolineas.src.Modules.Fares.Domain.Repositories;
using GestionAerolineas.src.Modules.Fares.Domain.ValueObject;
using GestionAerolineas.src.Modules.Fares.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Fares.Infrastructure.Repository;

public class FareRepository : IFareRepository
{
    private readonly AppDbContext _context;

    public FareRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Fare>> GetAllAsync()
    {
        var entities = await _context.Fares
            .AsNoTracking()
            .OrderBy(e => e.RouteId)
            .ThenBy(e => e.CabinTypeId)
            .ThenBy(e => e.PassengerTypeId)
            .ThenBy(e => e.SeasonId)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Fare?> GetByIdAsync(FareId id)
    {
        var entity = await _context.Fares
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Fare>> GetByRouteIdAsync(FareRouteId routeId)
    {
        var entities = await _context.Fares
            .AsNoTracking()
            .Where(e => e.RouteId == routeId.Value)
            .OrderBy(e => e.CabinTypeId)
            .ThenBy(e => e.PassengerTypeId)
            .ThenBy(e => e.SeasonId)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Fare?> GetByKeysAsync(FareRouteId routeId, FareCabinTypeId cabinTypeId, FarePassengerTypeId passengerTypeId, FareSeasonId seasonId)
    {
        var entity = await _context.Fares
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.RouteId == routeId.Value &&
                e.CabinTypeId == cabinTypeId.Value &&
                e.PassengerTypeId == passengerTypeId.Value &&
                e.SeasonId == seasonId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(Fare fare)
    {
        await _context.Fares.AddAsync(MapToEntity(fare));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Fare fare)
    {
        var existing = await _context.Fares
            .FirstOrDefaultAsync(e => e.Id == fare.Id.Value);

        if (existing is null)
            return;

        existing.RouteId = fare.RouteId.Value;
        existing.CabinTypeId = fare.CabinTypeId.Value;
        existing.PassengerTypeId = fare.PassengerTypeId.Value;
        existing.SeasonId = fare.SeasonId.Value;
        existing.BasePrice = fare.BasePrice.Value;
        existing.ValidFrom = fare.ValidFrom.Value;
        existing.ValidUntil = fare.ValidUntil.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Fare fare)
    {
        var entity = await _context.Fares.FindAsync(fare.Id.Value);
        if (entity is null)
            return;

        _context.Fares.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(FareId id)
    {
        return _context.Fares.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByKeysAsync(int routeId, int cabinTypeId, int passengerTypeId, int seasonId, int? excludingId = null)
    {
        var query = _context.Fares
            .AsNoTracking()
            .Where(e =>
                e.RouteId == routeId &&
                e.CabinTypeId == cabinTypeId &&
                e.PassengerTypeId == passengerTypeId &&
                e.SeasonId == seasonId);

        if (excludingId.HasValue)
            query = query.Where(e => e.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static Fare MapToDomain(FareEntity entity)
    {
        try
        {
            return Fare.Create(
                FareId.Create(entity.Id),
                FareRouteId.Create(entity.RouteId),
                FareCabinTypeId.Create(entity.CabinTypeId),
                FarePassengerTypeId.Create(entity.PassengerTypeId),
                FareSeasonId.Create(entity.SeasonId),
                FareBasePrice.Create(entity.BasePrice),
                FareValidFromDate.Create(entity.ValidFrom),
                FareValidUntilDate.Create(entity.ValidUntil)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro fares(id={entity.Id}) tiene datos invalidos. " +
                $"ruta_id={entity.RouteId}, tipo_cabina_id={entity.CabinTypeId}, " +
                $"tipo_pasajero_id={entity.PassengerTypeId}, temporada_id={entity.SeasonId}, " +
                $"precio_base={entity.BasePrice}, vigencia_desde={entity.ValidFrom:yyyy-MM-dd}, vigencia_hasta={entity.ValidUntil:yyyy-MM-dd}.",
                ex);
        }
    }

    private static FareEntity MapToEntity(Fare fare)
    {
        return new FareEntity
        {
            Id = fare.Id.Value,
            RouteId = fare.RouteId.Value,
            CabinTypeId = fare.CabinTypeId.Value,
            PassengerTypeId = fare.PassengerTypeId.Value,
            SeasonId = fare.SeasonId.Value,
            BasePrice = fare.BasePrice.Value,
            ValidFrom = fare.ValidFrom.Value,
            ValidUntil = fare.ValidUntil.Value
        };
    }
}

