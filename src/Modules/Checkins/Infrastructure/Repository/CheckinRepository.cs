// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Infrastructure\Repository\CheckinRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Checkins.Domain.Aggregate;
using GestionAerolineas.src.Modules.Checkins.Domain.Repositories;
using GestionAerolineas.src.Modules.Checkins.Domain.ValueObject;
using GestionAerolineas.src.Modules.Checkins.Infrastructure.Entity;
using GestionAerolineas.src.Modules.FlightSeats.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Tickets.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Checkins.Infrastructure.Repository;

public class CheckinRepository : ICheckinRepository
{
    private readonly AppDbContext _context;

    public CheckinRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Checkin>> GetAllAsync()
    {
        var entities = await _context.Set<CheckinEntity>()
            .AsNoTracking()
            .OrderByDescending(e => e.CheckedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Checkin?> GetByIdAsync(CheckinId id)
    {
        var entity = await _context.Set<CheckinEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Checkin?> GetByTicketIdAsync(CheckinTicketId ticketId)
    {
        var entity = await _context.Set<CheckinEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.TicketId == ticketId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Checkin>> GetByPassengerIdAsync(int passengerId)
    {
        var query =
            from c in _context.Set<CheckinEntity>().AsNoTracking()
            join t in _context.Set<TicketEntity>().AsNoTracking() on c.TicketId equals t.Id
            join rp in _context.Set<ReservationPassengerEntity>().AsNoTracking() on t.ReservationPassengerId equals rp.Id
            where rp.PassengerId == passengerId
            orderby c.CheckedAt descending, c.Id descending
            select c;

        var entities = await query.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Checkin>> GetByFlightIdAsync(int flightId)
    {
        var query =
            from c in _context.Set<CheckinEntity>().AsNoTracking()
            join fs in _context.Set<FlightSeatEntity>().AsNoTracking() on c.FlightSeatId equals fs.Id
            where fs.FlightId == flightId
            orderby c.CheckedAt descending, c.Id descending
            select c;

        var entities = await query.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Checkin>> GetByStatusIdAsync(CheckinStatusId statusId)
    {
        var entities = await _context.Set<CheckinEntity>()
            .AsNoTracking()
            .Where(e => e.StatusId == statusId.Value)
            .OrderByDescending(e => e.CheckedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Checkin>> GetByCheckedAtRangeAsync(DateTime fromInclusive, DateTime toInclusive)
    {
        var entities = await _context.Set<CheckinEntity>()
            .AsNoTracking()
            .Where(e => e.CheckedAt >= fromInclusive && e.CheckedAt <= toInclusive)
            .OrderByDescending(e => e.CheckedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(Checkin checkin)
    {
        var seat = await _context.FlightSeats.FirstOrDefaultAsync(s => s.Id == checkin.FlightSeatId.Value);
        if (seat is null)
            throw new Exception("El asiento_vuelo_id no existe");
        if (seat.IsOccupied)
            throw new Exception("El asiento ya esta ocupado");

        seat.IsOccupied = true;
        await _context.Set<CheckinEntity>().AddAsync(MapToEntity(checkin));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Checkin checkin)
    {
        var existing = await _context.Set<CheckinEntity>()
            .FirstOrDefaultAsync(e => e.Id == checkin.Id.Value);

        if (existing is null)
            return;

        var oldSeatId = existing.FlightSeatId;
        var newSeatId = checkin.FlightSeatId.Value;

        if (oldSeatId != newSeatId)
        {
            var oldSeat = await _context.FlightSeats.FirstOrDefaultAsync(s => s.Id == oldSeatId);
            if (oldSeat is not null)
                oldSeat.IsOccupied = false;

            var newSeat = await _context.FlightSeats.FirstOrDefaultAsync(s => s.Id == newSeatId);
            if (newSeat is null)
                throw new Exception("El asiento_vuelo_id no existe");
            if (newSeat.IsOccupied)
                throw new Exception("El asiento ya esta ocupado");

            newSeat.IsOccupied = true;
        }

        existing.TicketId = checkin.TicketId.Value;
        existing.StaffId = checkin.StaffId.Value;
        existing.FlightSeatId = checkin.FlightSeatId.Value;
        existing.CheckedAt = checkin.CheckedAt.Value;
        existing.StatusId = checkin.StatusId.Value;
        existing.BoardingPassNumber = checkin.BoardingPassNumber.Value;
        existing.HasHoldBaggage = checkin.HasHoldBaggage.Value;
        existing.BaggageWeightKg = checkin.BaggageWeightKg.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Checkin checkin)
    {
        var entity = await _context.Set<CheckinEntity>().FindAsync(checkin.Id.Value);
        if (entity is null)
            return;

        var seat = await _context.FlightSeats.FirstOrDefaultAsync(s => s.Id == entity.FlightSeatId);
        if (seat is not null)
            seat.IsOccupied = false;

        _context.Set<CheckinEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(CheckinId id)
    {
        return _context.Set<CheckinEntity>().AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByTicketIdAsync(int ticketId, int? excludingId = null)
    {
        var query = _context.Set<CheckinEntity>()
            .AsNoTracking()
            .Where(c => c.TicketId == ticketId);

        if (excludingId.HasValue)
            query = query.Where(c => c.Id != excludingId.Value);

        return query.AnyAsync();
    }

    public Task<bool> ExistsByFlightSeatIdAsync(int flightSeatId, int? excludingId = null)
    {
        var query = _context.Set<CheckinEntity>()
            .AsNoTracking()
            .Where(c => c.FlightSeatId == flightSeatId);

        if (excludingId.HasValue)
            query = query.Where(c => c.Id != excludingId.Value);

        return query.AnyAsync();
    }

    public Task<bool> ExistsByNormalizedBoardingPassAsync(string normalizedBoardingPass, int? excludingId = null)
    {
        var query = _context.Set<CheckinEntity>()
            .AsNoTracking()
            .Where(c => c.BoardingPassNumber != null);

        if (excludingId.HasValue)
            query = query.Where(c => c.Id != excludingId.Value);

        return query.AnyAsync(c => c.BoardingPassNumber!.Trim().ToUpper() == normalizedBoardingPass);
    }

    public async Task<int?> GetTicketFlightIdAsync(int ticketId)
    {
        var query =
            from t in _context.Set<TicketEntity>().AsNoTracking()
            join rp in _context.Set<ReservationPassengerEntity>().AsNoTracking() on t.ReservationPassengerId equals rp.Id
            join rf in _context.Set<ReservationFlightEntity>().AsNoTracking() on rp.ReservationFlightId equals rf.Id
            where t.Id == ticketId
            select (int?)rf.FlightId;

        return await query.FirstOrDefaultAsync();
    }

    private static Checkin MapToDomain(CheckinEntity entity)
    {
        try
        {
            return Checkin.Create(
                CheckinId.Create(entity.Id),
                CheckinTicketId.Create(entity.TicketId),
                CheckinStaffId.Create(entity.StaffId),
                CheckinFlightSeatId.Create(entity.FlightSeatId),
                CheckinCheckedAt.Create(entity.CheckedAt),
                CheckinStatusId.Create(entity.StatusId),
                CheckinBoardingPassNumber.Create(entity.BoardingPassNumber ?? string.Empty),
                CheckinHasHoldBaggage.Create(entity.HasHoldBaggage),
                CheckinBaggageWeightKg.Create(entity.BaggageWeightKg));
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro checkins(id={entity.Id}) tiene datos invalidos. " +
                $"tiquete_id={entity.TicketId}, personal_id={entity.StaffId}, asiento_vuelo_id={entity.FlightSeatId}, " +
                $"fecha_checkin='{entity.CheckedAt}', estado_checkin_id={entity.StatusId}, numero_tarjeta_embarque='{entity.BoardingPassNumber}'.",
                ex);
        }
    }

    private static CheckinEntity MapToEntity(Checkin checkin)
    {
        return new CheckinEntity
        {
            Id = checkin.Id.Value,
            TicketId = checkin.TicketId.Value,
            StaffId = checkin.StaffId.Value,
            FlightSeatId = checkin.FlightSeatId.Value,
            CheckedAt = checkin.CheckedAt.Value,
            StatusId = checkin.StatusId.Value,
            BoardingPassNumber = checkin.BoardingPassNumber.Value,
            HasHoldBaggage = checkin.HasHoldBaggage.Value,
            BaggageWeightKg = checkin.BaggageWeightKg.Value
        };
    }
}

