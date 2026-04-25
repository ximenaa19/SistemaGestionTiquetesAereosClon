// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Infrastructure\Repository\FlightRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.Repositories;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;

public class FlightRepository : IFlightRepository
{
    private readonly AppDbContext _context;

    public FlightRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Flight>> GetAllAsync()
    {
        var entities = await _context.Flights
            .AsNoTracking()
            .OrderByDescending(e => e.DepartureDateTime)
            .ThenBy(e => e.Code)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Flight?> GetByIdAsync(FlightId id)
    {
        var entity = await _context.Flights
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Flight?> GetByCodeAsync(FlightCode code)
    {
        var normalized = FlightCode.Normalize(code.Value);
        var entity = await _context.Flights
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Code != null && e.Code.Trim().ToUpper() == normalized);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Flight>> GetByAirlineIdAsync(FlightAirlineId airlineId)
    {
        var entities = await _context.Flights
            .AsNoTracking()
            .Where(e => e.AirlineId == airlineId.Value)
            .OrderByDescending(e => e.DepartureDateTime)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Flight>> GetByRouteIdAsync(FlightRouteId routeId)
    {
        var entities = await _context.Flights
            .AsNoTracking()
            .Where(e => e.RouteId == routeId.Value)
            .OrderByDescending(e => e.DepartureDateTime)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Flight>> GetByStateIdAsync(FlightStateId stateId)
    {
        var entities = await _context.Flights
            .AsNoTracking()
            .Where(e => e.StateId == stateId.Value)
            .OrderByDescending(e => e.DepartureDateTime)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Flight>> GetByDepartureDateRangeAsync(DateTime fromInclusive, DateTime toInclusive)
    {
        var entities = await _context.Flights
            .AsNoTracking()
            .Where(e => e.DepartureDateTime >= fromInclusive && e.DepartureDateTime <= toInclusive)
            .OrderBy(e => e.DepartureDateTime)
            .ThenBy(e => e.Code)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(Flight flight)
    {
        await _context.Flights.AddAsync(MapToEntity(flight));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Flight flight)
    {
        var existing = await _context.Flights
            .FirstOrDefaultAsync(e => e.Id == flight.Id.Value);

        if (existing is null)
            return;

        existing.Code = flight.Code.Value;
        existing.AirlineId = flight.AirlineId.Value;
        existing.RouteId = flight.RouteId.Value;
        existing.AircraftId = flight.AircraftId.Value;
        existing.DepartureDateTime = flight.DepartureDateTime.Value;
        existing.EstimatedArrivalDateTime = flight.EstimatedArrivalDateTime.Value;
        existing.TotalCapacity = flight.TotalCapacity.Value;
        existing.AvailableSeats = flight.AvailableSeats.Value;
        existing.StateId = flight.StateId.Value;
        existing.RescheduledAt = flight.RescheduledAt.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Flight flight)
    {
        var entity = await _context.Flights.FindAsync(flight.Id.Value);
        if (entity is null)
            return;

        _context.Flights.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(FlightId id)
    {
        return _context.Flights.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByNormalizedCodeAsync(string normalizedCode, int? excludingId = null)
    {
        var query = _context.Flights
            .AsNoTracking()
            .Where(f => f.Code != null);

        if (excludingId.HasValue)
            query = query.Where(f => f.Id != excludingId.Value);

        return query.AnyAsync(f => f.Code!.Trim().ToUpper() == normalizedCode);
    }

    public Task<bool> ExistsAircraftOverlapAsync(int aircraftId, DateTime departure, DateTime estimatedArrival, int? excludingId = null)
    {
        var query = _context.Flights
            .AsNoTracking()
            .Where(f => f.AircraftId == aircraftId && f.DepartureDateTime < estimatedArrival && f.EstimatedArrivalDateTime > departure);

        if (excludingId.HasValue)
            query = query.Where(f => f.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static Flight MapToDomain(FlightEntity entity)
    {
        try
        {
            return Flight.Create(
                FlightId.Create(entity.Id),
                FlightCode.Create(entity.Code ?? string.Empty),
                FlightAirlineId.Create(entity.AirlineId),
                FlightRouteId.Create(entity.RouteId),
                FlightAircraftId.Create(entity.AircraftId),
                FlightDepartureDateTime.Create(entity.DepartureDateTime),
                FlightEstimatedArrivalDateTime.Create(entity.EstimatedArrivalDateTime),
                FlightTotalCapacity.Create(entity.TotalCapacity),
                FlightAvailableSeats.Create(entity.AvailableSeats),
                FlightStateId.Create(entity.StateId),
                FlightRescheduledAt.Create(entity.RescheduledAt)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro flights(id={entity.Id}) tiene datos invalidos. " +
                $"codigo_vuelo='{entity.Code}', aerolinea_id={entity.AirlineId}, ruta_id={entity.RouteId}, aeronave_id={entity.AircraftId}, " +
                $"fecha_salida={entity.DepartureDateTime:o}, fecha_llegada_estimada={entity.EstimatedArrivalDateTime:o}, " +
                $"capacidad_total={entity.TotalCapacity}, asientos_disponibles={entity.AvailableSeats}, estado_vuelo_id={entity.StateId}.",
                ex);
        }
    }

    private static FlightEntity MapToEntity(Flight flight)
    {
        return new FlightEntity
        {
            Id = flight.Id.Value,
            Code = flight.Code.Value,
            AirlineId = flight.AirlineId.Value,
            RouteId = flight.RouteId.Value,
            AircraftId = flight.AircraftId.Value,
            DepartureDateTime = flight.DepartureDateTime.Value,
            EstimatedArrivalDateTime = flight.EstimatedArrivalDateTime.Value,
            TotalCapacity = flight.TotalCapacity.Value,
            AvailableSeats = flight.AvailableSeats.Value,
            StateId = flight.StateId.Value,
            RescheduledAt = flight.RescheduledAt.Value
        };
    }
}

