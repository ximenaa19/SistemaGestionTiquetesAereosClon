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

// implementacion EF Core del repositorio de equipaje
public class BaggageRecordRepository : IBaggageRecordRepository
{
    // contexto compartido de la aplicacion para acceder a las tablas
    private readonly AppDbContext _context;

    public BaggageRecordRepository(AppDbContext context)
    {
        // se recibe desde BaggageModule para usar la misma conexion del sistema
        _context = context;
    }

    public async Task AddAsync(BaggageRecord record)
    {
        // convierte el agregado de dominio a entidad EF y lo agrega al DbContext
        await _context.Set<BaggageRecordEntity>().AddAsync(MapToEntity(record));
        // guarda los cambios en MySQL
        await _context.SaveChangesAsync();
    }

    public async Task<BaggageRecord?> GetByIdAsync(int id)
    {
        // valida el id antes de consultar la tabla
        if (id <= 0)
            return null;

        // busca el registro por llave primaria sin seguimiento porque solo se va a leer
        var entity = await _context.Set<BaggageRecordEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        // si no existe, retorna null para que el caso de uso decida que hacer
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<BaggageRegistrationContext?> GetRegistrationContextByTicketIdAsync(int ticketId)
    {
        // busca todos los datos relacionados partiendo desde el tiquete
        var query =
            from ticket in _context.Set<TicketEntity>().AsNoTracking()
            // ticket -> reservationPassenger
            join reservationPassenger in _context.Set<ReservationPassengerEntity>().AsNoTracking()
                on ticket.ReservationPassengerId equals reservationPassenger.Id
            // reservationPassenger -> reservationFlight
            join reservationFlight in _context.Set<ReservationFlightEntity>().AsNoTracking()
                on reservationPassenger.ReservationFlightId equals reservationFlight.Id
            // reservationFlight -> reservation
            join reservation in _context.Set<ReservationEntity>().AsNoTracking()
                on reservationFlight.ReservationId equals reservation.Id
            // reservationFlight -> flight
            join flight in _context.Set<FlightEntity>().AsNoTracking()
                on reservationFlight.FlightId equals flight.Id
            // reservationPassenger -> passenger
            join passenger in _context.Set<PassengerEntity>().AsNoTracking()
                on reservationPassenger.PassengerId equals passenger.Id
            // passenger -> person para construir el nombre
            join person in _context.Set<PersonEntity>().AsNoTracking()
                on passenger.PersonId equals person.Id
            where ticket.Id == ticketId
            // proyecta solo los datos que necesita el registro de equipaje
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

        // devuelve null si no existe el tiquete o no tiene las relaciones necesarias
        return await query.FirstOrDefaultAsync();
    }

    public async Task<BaggageRegistrationContext?> GetRegistrationContextByReservationIdAsync(int reservationId)
    {
        // consulta el contexto partiendo desde reserva, usando joins opcionales
        var query =
            from reservation in _context.Set<ReservationEntity>().AsNoTracking()
            where reservation.Id == reservationId
            // left join con vuelos de la reserva
            join reservationFlightOptional in _context.Set<ReservationFlightEntity>().AsNoTracking()
                on reservation.Id equals reservationFlightOptional.ReservationId into reservationFlights
            from reservationFlight in reservationFlights.DefaultIfEmpty()
            // left join con flights porque puede no existir informacion completa
            join flightOptional in _context.Set<FlightEntity>().AsNoTracking()
                on reservationFlight.FlightId equals flightOptional.Id into flights
            from flight in flights.DefaultIfEmpty()
            // left join con pasajeros de la reserva
            join reservationPassengerOptional in _context.Set<ReservationPassengerEntity>().AsNoTracking()
                on reservationFlight.Id equals reservationPassengerOptional.ReservationFlightId into reservationPassengers
            from reservationPassenger in reservationPassengers.DefaultIfEmpty()
            // left join con tiquetes emitidos
            join ticketOptional in _context.Set<TicketEntity>().AsNoTracking()
                on reservationPassenger.Id equals ticketOptional.ReservationPassengerId into tickets
            from ticket in tickets.DefaultIfEmpty()
            // left join con pasajero
            join passengerOptional in _context.Set<PassengerEntity>().AsNoTracking()
                on reservationPassenger.PassengerId equals passengerOptional.Id into passengers
            from passenger in passengers.DefaultIfEmpty()
            // left join con persona para mostrar nombre
            join personOptional in _context.Set<PersonEntity>().AsNoTracking()
                on passenger.PersonId equals personOptional.Id into people
            from person in people.DefaultIfEmpty()
            // toma primero el vuelo/pasajero asociado segun el orden natural
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

        // devuelve el primer contexto encontrado para esa reserva
        return await query.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<CabinTypeOption>> GetCabinTypesAsync()
    {
        // lista cabinas como opciones simples para el menu de consola
        return await _context.Set<CabinTypeEntity>()
            .AsNoTracking()
            .OrderBy(e => e.Id)
            .Select(e => new CabinTypeOption(e.Id, e.Name ?? string.Empty))
            .ToListAsync();
    }

    public Task<bool> CabinTypeExistsAsync(int cabinTypeId)
    {
        // valida existencia real de la cabina en la base de datos
        return _context.Set<CabinTypeEntity>().AnyAsync(e => e.Id == cabinTypeId);
    }

    public async Task AddSurchargeToReservationAsync(int reservationId, decimal surcharge)
    {
        // si no hay recargo, no se modifica la reserva
        if (surcharge <= 0)
            return;

        // busca la reserva que debe recibir el recargo
        var reservation = await _context.Set<ReservationEntity>()
            .FirstOrDefaultAsync(e => e.Id == reservationId);

        if (reservation is null)
            throw new InvalidOperationException("La reserva no existe.");

        // suma el recargo al total actual y actualiza la fecha de modificacion
        reservation.TotalAmount = decimal.Round(reservation.TotalAmount + surcharge, 2);
        reservation.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<BaggageRecordView>> GetByCustomerIdAsync(int customerId)
    {
        // construye la vista filtrando por cliente de la reserva
        return await BuildViewQuery(customerId: customerId).ToListAsync();
    }

    public async Task<IReadOnlyList<BaggageRecordView>> GetByFlightIdAsync(int flightId)
    {
        // construye la vista filtrando por vuelo
        return await BuildViewQuery(flightId: flightId).ToListAsync();
    }

    public async Task<IReadOnlyList<BaggageRecordView>> GetWithSurchargesAsync()
    {
        // construye la vista filtrando solo registros con recargo mayor que cero
        return await BuildViewQuery(onlySurcharges: true).ToListAsync();
    }

    private IQueryable<BaggageRecordView> BuildViewQuery(
        int? customerId = null,
        int? flightId = null,
        bool onlySurcharges = false)
    {
        // consulta base con joins para armar una vista completa de equipaje
        var query =
            from baggage in _context.Set<BaggageRecordEntity>().AsNoTracking()
            // baggage -> reservation para codigo y customer_id
            join reservation in _context.Set<ReservationEntity>().AsNoTracking()
                on baggage.ReservationId equals reservation.Id
            // left join con tickets porque el registro puede haberse hecho solo por reserva
            join ticketOptional in _context.Set<TicketEntity>().AsNoTracking()
                on baggage.TicketId equals ticketOptional.Id into tickets
            from ticket in tickets.DefaultIfEmpty()
            // left join con flights para mostrar codigo de vuelo si existe
            join flightOptional in _context.Set<FlightEntity>().AsNoTracking()
                on baggage.FlightId equals flightOptional.Id into flights
            from flight in flights.DefaultIfEmpty()
            // left join con passengers para mostrar datos del pasajero si existen
            join passengerOptional in _context.Set<PassengerEntity>().AsNoTracking()
                on baggage.PassengerId equals passengerOptional.Id into passengers
            from passenger in passengers.DefaultIfEmpty()
            // left join con people para construir nombre completo
            join personOptional in _context.Set<PersonEntity>().AsNoTracking()
                on passenger.PersonId equals personOptional.Id into people
            from person in people.DefaultIfEmpty()
            // cabina es obligatoria porque define la politica aplicada
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

        // filtro opcional para consultas del menu por cliente
        if (customerId.HasValue)
            query = query.Where(x => x.Reservation.CustomerId == customerId.Value);

        // filtro opcional para consultas del menu por vuelo
        if (flightId.HasValue)
            query = query.Where(x => x.Baggage.FlightId == flightId.Value);

        // filtro opcional para ver solo equipaje que genero cobro adicional
        if (onlySurcharges)
            query = query.Where(x => x.Baggage.TotalSurcharge > 0);

        // ordena los registros mas recientes primero
        query = query
            .OrderByDescending(x => x.Baggage.RegisteredAt)
            .ThenByDescending(x => x.Baggage.Id);

        // proyecta al modelo de vista que usa la UI; se hace al final para evitar errores de traduccion
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
        // extrae los valores de los value objects para guardarlos en columnas simples
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

    private static BaggageRecord MapToDomain(BaggageRecordEntity entity)
    {
        // reconstruye el agregado de dominio desde la entidad plana de EF
        return BaggageRecord.Create(
            entity.Id,
            entity.ReservationId,
            entity.TicketId,
            entity.ReservationPassengerId,
            entity.FlightId,
            entity.PassengerId,
            entity.CabinTypeId,
            entity.BaggageType ?? string.Empty,
            entity.Quantity,
            entity.WeightKg,
            entity.Description,
            entity.AllowedQuantity,
            entity.AllowedWeightPerBagKg,
            entity.AllowedTotalWeightKg,
            entity.ExcessQuantity,
            entity.ExcessWeightKg,
            entity.QuantitySurcharge,
            entity.WeightSurcharge,
            entity.TotalSurcharge,
            entity.RegisteredAt);
    }

    private static string BuildPersonName(string? firstNames, string? lastNames)
    {
        // une nombres y apellidos, y evita mostrar texto vacio en la consulta
        var fullName = $"{firstNames} {lastNames}".Trim();
        return string.IsNullOrWhiteSpace(fullName) ? "Sin nombre" : fullName;
    }
}
