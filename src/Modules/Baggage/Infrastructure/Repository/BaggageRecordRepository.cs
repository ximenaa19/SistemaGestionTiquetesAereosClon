using GestionAerolineas.src.Modules.Baggage.Domain.Aggregate;
using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;
using GestionAerolineas.src.Modules.Baggage.Infrastructure.Entity;
using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Customers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.People.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Tickets.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Baggage.Infrastructure.Repository;

public class BaggageRecordRepository : IBaggageRecordRepository
{
    private readonly AppDbContext _context;

    public BaggageRecordRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(BaggageRecord record)
    {
        await _context.Set<BaggageRecordEntity>().AddAsync(MapToEntity(record));
        await _context.SaveChangesAsync();
    }

    public async Task<BaggageRegistrationContext?> GetRegistrationContextByTicketIdAsync(int ticketId)
    {
        var query =
            from ticket in _context.Set<TicketEntity>().AsNoTracking()
            join reservationPassenger in _context.Set<ReservationPassengerEntity>().AsNoTracking()
                on ticket.ReservationPassengerId equals reservationPassenger.Id
            join reservationFlight in _context.Set<ReservationFlightEntity>().AsNoTracking()
                on reservationPassenger.ReservationFlightId equals reservationFlight.Id
            join reservation in _context.Set<ReservationEntity>().AsNoTracking()
                on reservationFlight.ReservationId equals reservation.Id
            join flight in _context.Set<FlightEntity>().AsNoTracking()
                on reservationFlight.FlightId equals flight.Id
            join passenger in _context.Set<PassengerEntity>().AsNoTracking()
                on reservationPassenger.PassengerId equals passenger.Id
            join person in _context.Set<PersonEntity>().AsNoTracking()
                on passenger.PersonId equals person.Id
            where ticket.Id == ticketId
            select new BaggageRegistrationContext(
                reservation.Id,
                reservation.Code,
                ticket.Id,
                ticket.Code,
                reservationPassenger.Id,
                flight.Id,
                flight.Code,
                passenger.Id,
                BuildPersonName(person.FirstNames, person.LastNames),
                reservation.TotalAmount);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<BaggageRegistrationContext?> GetRegistrationContextByReservationIdAsync(int reservationId)
    {
        var query =
            from reservation in _context.Set<ReservationEntity>().AsNoTracking()
            where reservation.Id == reservationId
            join reservationFlightOptional in _context.Set<ReservationFlightEntity>().AsNoTracking()
                on reservation.Id equals reservationFlightOptional.ReservationId into reservationFlights
            from reservationFlight in reservationFlights.DefaultIfEmpty()
            join flightOptional in _context.Set<FlightEntity>().AsNoTracking()
                on reservationFlight.FlightId equals flightOptional.Id into flights
            from flight in flights.DefaultIfEmpty()
            join reservationPassengerOptional in _context.Set<ReservationPassengerEntity>().AsNoTracking()
                on reservationFlight.Id equals reservationPassengerOptional.ReservationFlightId into reservationPassengers
            from reservationPassenger in reservationPassengers.DefaultIfEmpty()
            join ticketOptional in _context.Set<TicketEntity>().AsNoTracking()
                on reservationPassenger.Id equals ticketOptional.ReservationPassengerId into tickets
            from ticket in tickets.DefaultIfEmpty()
            join passengerOptional in _context.Set<PassengerEntity>().AsNoTracking()
                on reservationPassenger.PassengerId equals passengerOptional.Id into passengers
            from passenger in passengers.DefaultIfEmpty()
            join personOptional in _context.Set<PersonEntity>().AsNoTracking()
                on passenger.PersonId equals personOptional.Id into people
            from person in people.DefaultIfEmpty()
            orderby reservationFlight.Id, reservationPassenger.Id
            select new BaggageRegistrationContext(
                reservation.Id,
                reservation.Code,
                ticket == null ? null : ticket.Id,
                ticket == null ? null : ticket.Code,
                reservationPassenger == null ? null : reservationPassenger.Id,
                flight == null ? null : flight.Id,
                flight == null ? null : flight.Code,
                passenger == null ? null : passenger.Id,
                person == null ? null : BuildPersonName(person.FirstNames, person.LastNames),
                reservation.TotalAmount);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<CabinTypeOption>> GetCabinTypesAsync()
    {
        return await _context.Set<CabinTypeEntity>()
            .AsNoTracking()
            .OrderBy(e => e.Id)
            .Select(e => new CabinTypeOption(e.Id, e.Name ?? string.Empty))
            .ToListAsync();
    }

    public Task<bool> CabinTypeExistsAsync(int cabinTypeId)
    {
        return _context.Set<CabinTypeEntity>().AnyAsync(e => e.Id == cabinTypeId);
    }

    public async Task AddSurchargeToReservationAsync(int reservationId, decimal surcharge)
    {
        if (surcharge <= 0)
            return;

        var reservation = await _context.Set<ReservationEntity>()
            .FirstOrDefaultAsync(e => e.Id == reservationId);

        if (reservation is null)
            throw new InvalidOperationException("La reserva no existe.");

        reservation.TotalAmount = decimal.Round(reservation.TotalAmount + surcharge, 2);
        reservation.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<BaggageRecordView>> GetByCustomerIdAsync(int customerId)
    {
        return await BuildViewQuery(customerId: customerId).ToListAsync();
    }

    public async Task<IReadOnlyList<BaggageRecordView>> GetByFlightIdAsync(int flightId)
    {
        return await BuildViewQuery(flightId: flightId).ToListAsync();
    }

    public async Task<IReadOnlyList<BaggageRecordView>> GetWithSurchargesAsync()
    {
        return await BuildViewQuery(onlySurcharges: true).ToListAsync();
    }

    private IQueryable<BaggageRecordView> BuildViewQuery(
        int? customerId = null,
        int? flightId = null,
        bool onlySurcharges = false)
    {
        var query =
            from baggage in _context.Set<BaggageRecordEntity>().AsNoTracking()
            join reservation in _context.Set<ReservationEntity>().AsNoTracking()
                on baggage.ReservationId equals reservation.Id
            join ticketOptional in _context.Set<TicketEntity>().AsNoTracking()
                on baggage.TicketId equals ticketOptional.Id into tickets
            from ticket in tickets.DefaultIfEmpty()
            join flightOptional in _context.Set<FlightEntity>().AsNoTracking()
                on baggage.FlightId equals flightOptional.Id into flights
            from flight in flights.DefaultIfEmpty()
            join passengerOptional in _context.Set<PassengerEntity>().AsNoTracking()
                on baggage.PassengerId equals passengerOptional.Id into passengers
            from passenger in passengers.DefaultIfEmpty()
            join personOptional in _context.Set<PersonEntity>().AsNoTracking()
                on passenger.PersonId equals personOptional.Id into people
            from person in people.DefaultIfEmpty()
            join cabin in _context.Set<CabinTypeEntity>().AsNoTracking()
                on baggage.CabinTypeId equals cabin.Id
            select new
            {
                Baggage = baggage,
                Reservation = reservation,
                Ticket = ticket,
                Flight = flight,
                Passenger = passenger,
                Person = person,
                Cabin = cabin
            };

        if (customerId.HasValue)
            query = query.Where(x => x.Reservation.CustomerId == customerId.Value);

        if (flightId.HasValue)
            query = query.Where(x => x.Baggage.FlightId == flightId.Value);

        if (onlySurcharges)
            query = query.Where(x => x.Baggage.TotalSurcharge > 0);

        query = query
            .OrderByDescending(x => x.Baggage.RegisteredAt)
            .ThenByDescending(x => x.Baggage.Id);

        return query.Select(x => new BaggageRecordView(
                x.Baggage.Id,
                x.Baggage.ReservationId,
                x.Reservation.Code,
                x.Baggage.TicketId,
                x.Ticket == null ? null : x.Ticket.Code,
                x.Baggage.FlightId,
                x.Flight == null ? null : x.Flight.Code,
                x.Baggage.PassengerId,
                x.Person == null ? null : BuildPersonName(x.Person.FirstNames, x.Person.LastNames),
                x.Baggage.CabinTypeId,
                x.Cabin.Name,
                x.Baggage.BaggageType ?? string.Empty,
                x.Baggage.Quantity,
                x.Baggage.WeightKg,
                x.Baggage.Description,
                x.Baggage.AllowedQuantity,
                x.Baggage.AllowedWeightPerBagKg,
                x.Baggage.AllowedTotalWeightKg,
                x.Baggage.ExcessQuantity,
                x.Baggage.ExcessWeightKg,
                x.Baggage.QuantitySurcharge,
                x.Baggage.WeightSurcharge,
                x.Baggage.TotalSurcharge,
                x.Baggage.RegisteredAt));
    }

    private static BaggageRecordEntity MapToEntity(BaggageRecord record)
    {
        return new BaggageRecordEntity
        {
            Id = record.Id.Value,
            ReservationId = record.ReservationId.Value,
            TicketId = record.TicketId,
            ReservationPassengerId = record.ReservationPassengerId,
            FlightId = record.FlightId,
            PassengerId = record.PassengerId,
            CabinTypeId = record.CabinTypeId.Value,
            BaggageType = record.BaggageType.Value,
            Quantity = record.Quantity.Value,
            WeightKg = record.WeightKg.Value,
            Description = record.Description,
            AllowedQuantity = record.AllowedQuantity,
            AllowedWeightPerBagKg = record.AllowedWeightPerBagKg,
            AllowedTotalWeightKg = record.AllowedTotalWeightKg,
            ExcessQuantity = record.ExcessQuantity,
            ExcessWeightKg = record.ExcessWeightKg,
            QuantitySurcharge = record.QuantitySurcharge.Value,
            WeightSurcharge = record.WeightSurcharge.Value,
            TotalSurcharge = record.TotalSurcharge.Value,
            RegisteredAt = record.RegisteredAt.Value
        };
    }

    private static string BuildPersonName(string? firstNames, string? lastNames)
    {
        var fullName = $"{firstNames} {lastNames}".Trim();
        return string.IsNullOrWhiteSpace(fullName) ? "Sin nombre" : fullName;
    }
}
