// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Infrastructure\Repository\FlightSeatRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightSeats.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightSeats.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightSeats.Domain.ValueObject;
using GestionAerolineas.src.Modules.FlightSeats.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.FlightSeats.Infrastructure.Repository;

public class FlightSeatRepository : IFlightSeatRepository
{
    private readonly AppDbContext _context;

    public FlightSeatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FlightSeat>> GetAllAsync()
    {
        var entities = await _context.FlightSeats
            .AsNoTracking()
            .OrderBy(e => e.FlightId)
            .ThenBy(e => e.SeatCode)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<FlightSeat?> GetByIdAsync(FlightSeatId id)
    {
        var entity = await _context.FlightSeats
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<FlightSeat>> GetByFlightIdAsync(FlightSeatFlightId flightId)
    {
        var entities = await _context.FlightSeats
            .AsNoTracking()
            .Where(e => e.FlightId == flightId.Value)
            .OrderBy(e => e.SeatCode)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<FlightSeat?> GetByFlightAndCodeAsync(FlightSeatFlightId flightId, FlightSeatCode code)
    {
        var normalized = FlightSeatCode.Normalize(code.Value);
        var entity = await _context.FlightSeats
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.FlightId == flightId.Value &&
                e.SeatCode != null &&
                e.SeatCode.Trim().ToUpper() == normalized);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<FlightSeat>> GetByFlightIdAndOccupiedAsync(FlightSeatFlightId flightId, FlightSeatIsOccupied isOccupied)
    {
        var entities = await _context.FlightSeats
            .AsNoTracking()
            .Where(e => e.FlightId == flightId.Value && e.IsOccupied == isOccupied.Value)
            .OrderBy(e => e.SeatCode)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(FlightSeat seat)
    {
        await _context.FlightSeats.AddAsync(MapToEntity(seat));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(FlightSeat seat)
    {
        var existing = await _context.FlightSeats
            .FirstOrDefaultAsync(e => e.Id == seat.Id.Value);

        if (existing is null)
            return;

        existing.FlightId = seat.FlightId.Value;
        existing.SeatCode = seat.Code.Value;
        existing.CabinTypeId = seat.CabinTypeId.Value;
        existing.LocationTypeId = seat.LocationTypeId.Value;
        existing.IsOccupied = seat.IsOccupied.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(FlightSeat seat)
    {
        var entity = await _context.FlightSeats.FindAsync(seat.Id.Value);
        if (entity is null)
            return;

        _context.FlightSeats.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(FlightSeatId id)
    {
        return _context.FlightSeats.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByFlightAndNormalizedCodeAsync(int flightId, string normalizedSeatCode, int? excludingId = null)
    {
        var query = _context.FlightSeats
            .AsNoTracking()
            .Where(s => s.FlightId == flightId && s.SeatCode != null);

        if (excludingId.HasValue)
            query = query.Where(s => s.Id != excludingId.Value);

        return query.AnyAsync(s => s.SeatCode!.Trim().ToUpper() == normalizedSeatCode);
    }

    public Task<int> CountByFlightIdAsync(int flightId)
    {
        return _context.FlightSeats.CountAsync(s => s.FlightId == flightId);
    }

    private static FlightSeat MapToDomain(FlightSeatEntity entity)
    {
        try
        {
            return FlightSeat.Create(
                FlightSeatId.Create(entity.Id),
                FlightSeatFlightId.Create(entity.FlightId),
                FlightSeatCode.Create(entity.SeatCode ?? string.Empty),
                FlightSeatCabinTypeId.Create(entity.CabinTypeId),
                FlightSeatLocationTypeId.Create(entity.LocationTypeId),
                FlightSeatIsOccupied.Create(entity.IsOccupied)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro flightseats(id={entity.Id}) tiene datos invalidos. " +
                $"vuelo_id={entity.FlightId}, codigo_asiento='{entity.SeatCode}', tipo_cabina_id={entity.CabinTypeId}, " +
                $"tipo_ubicacion_id={entity.LocationTypeId}, esta_ocupado={entity.IsOccupied}.",
                ex);
        }
    }

    private static FlightSeatEntity MapToEntity(FlightSeat seat)
    {
        return new FlightSeatEntity
        {
            Id = seat.Id.Value,
            FlightId = seat.FlightId.Value,
            SeatCode = seat.Code.Value,
            CabinTypeId = seat.CabinTypeId.Value,
            LocationTypeId = seat.LocationTypeId.Value,
            IsOccupied = seat.IsOccupied.Value
        };
    }
}

