// [DocHeader]
// Modulo: General
// Capa: General
// Archivo: src\shared\Seed\OperationalSampleSeed.cs
// Responsabilidad: Crea datos operativos minimos para probar reservas de punta a punta.
// Flujo: Se ejecuta despues de catalogos y datos maestros.
using GestionAerolineas.src.Modules.Aircraft.Infrastructure.Entity;
using GestionAerolineas.src.Modules.AircraftManufacturers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.AircraftModels.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Customers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.FlightSeats.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.People.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Staff.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Tickets.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.shared.Seed;

public static class OperationalSampleSeed
{
    public static async Task SeedAsync(AppDbContext context)
    {
        var colombia = await context.Countries.AsNoTracking()
            .FirstAsync(x => x.IsoCode == "COL");

        var documentType = await context.DocumentTypes.AsNoTracking()
            .OrderBy(x => x.Id)
            .FirstAsync();

        var passengerType = await context.PassengerTypes.AsNoTracking()
            .FirstAsync(x => x.Name != null && x.Name.Trim().ToUpper() == "ADULTO");

        var cabinType = await context.CabinTypes.AsNoTracking()
            .OrderBy(x => x.Id)
            .FirstAsync();

        var seatLocationType = await context.SeatLocationTypes.AsNoTracking()
            .OrderBy(x => x.Id)
            .FirstAsync();

        var staffRole = await context.StaffRoles.AsNoTracking()
            .FirstAsync(x => x.Name != null && x.Name.Trim().ToUpper().Contains("CHECK"));

        var reservationStatus = await context.ReservationStatuses.AsNoTracking()
            .FirstAsync(x => x.Name != null && x.Name.Trim().ToUpper().Contains("PEND"));

        var ticketStatus = await context.TicketStatuses.AsNoTracking()
            .FirstAsync(x => x.Name != null && x.Name.Trim().ToUpper() == "EMITIDO");

        var origin = await context.Airports.AsNoTracking()
            .FirstAsync(x => x.IataCode == "BOG");

        var destination = await context.Airports.AsNoTracking()
            .FirstAsync(x => x.IataCode == "MDE");

        var flightState = await context.FlightStates.AsNoTracking()
            .FirstAsync(x => x.Name != null && x.Name.Trim().ToUpper() == "PROGRAMADO");

        var person = await EnsurePersonAsync(
            context,
            documentType.Id,
            "RES-PROBE-001",
            "Cliente",
            "Prueba Reserva");
        var customer = await EnsureCustomerAsync(context, person.Id);
        var passenger = await EnsurePassengerAsync(context, person.Id, passengerType.Id);
        var secondPerson = await EnsurePersonAsync(
            context,
            documentType.Id,
            "RES-PROBE-002",
            "Pasajero",
            "Prueba Checkin");
        var secondPassenger = await EnsurePassengerAsync(context, secondPerson.Id, passengerType.Id);

        var airline = await EnsureAirlineAsync(context, colombia.Id);
        await EnsureAirportStaffAsync(context, documentType.Id, staffRole.Id, airline.Id, origin.Id);
        var manufacturer = await EnsureManufacturerAsync(context, colombia.Id);
        var model = await EnsureAircraftModelAsync(context, manufacturer.Id);
        var aircraft = await EnsureAircraftAsync(context, model.Id, airline.Id);
        var route = await EnsureRouteAsync(context, origin.Id, destination.Id);
        var flight = await EnsureFlightAsync(context, airline.Id, route.Id, aircraft.Id, flightState.Id);
        await EnsureFlightSeatsAsync(context, flight.Id, cabinType.Id, seatLocationType.Id);

        var reservation = await EnsureReservationAsync(context, customer.Id, reservationStatus.Id);
        var reservationFlight = await EnsureReservationFlightAsync(context, reservation.Id, flight.Id);
        var reservationPassenger = await EnsureReservationPassengerAsync(context, reservationFlight.Id, passenger.Id);
        await EnsureTicketAsync(context, reservationPassenger.Id, ticketStatus.Id, "TCK-DEMO-001");
        var secondReservationPassenger = await EnsureReservationPassengerAsync(context, reservationFlight.Id, secondPassenger.Id);
        await EnsureTicketAsync(context, secondReservationPassenger.Id, ticketStatus.Id, "TCK-DEMO-002");
    }

    private static async Task<PersonEntity> EnsurePersonAsync(
        AppDbContext context,
        int documentTypeId,
        string documentNumber,
        string firstNames,
        string lastNames)
    {
        var existing = await context.People.FirstOrDefaultAsync(x => x.DocumentNumber == documentNumber);
        if (existing is not null)
        {
            if (!string.Equals(existing.Gender, "N", StringComparison.OrdinalIgnoreCase))
            {
                existing.Gender = "N";
                await context.SaveChangesAsync();
            }

            return existing;
        }

        var person = new PersonEntity
        {
            DocumentTypeId = documentTypeId,
            DocumentNumber = documentNumber,
            FirstNames = firstNames,
            LastNames = lastNames,
            BirthDate = new DateTime(1990, 1, 1),
            Gender = "N"
        };

        context.People.Add(person);
        await context.SaveChangesAsync();
        return person;
    }

    private static async Task<CustomerEntity> EnsureCustomerAsync(AppDbContext context, int personId)
    {
        var existing = await context.Customers.FirstOrDefaultAsync(x => x.PersonId == personId);
        if (existing is not null)
            return existing;

        var customer = new CustomerEntity { PersonId = personId };
        context.Customers.Add(customer);
        await context.SaveChangesAsync();
        return customer;
    }

    private static async Task<PassengerEntity> EnsurePassengerAsync(AppDbContext context, int personId, int passengerTypeId)
    {
        var existing = await context.Passengers.FirstOrDefaultAsync(x => x.PersonId == personId);
        if (existing is not null)
            return existing;

        var passenger = new PassengerEntity
        {
            PersonId = personId,
            PassengerTypeId = passengerTypeId
        };

        context.Passengers.Add(passenger);
        await context.SaveChangesAsync();
        return passenger;
    }

    private static async Task<AirlineEntity> EnsureAirlineAsync(AppDbContext context, int countryId)
    {
        const string iata = "AD";
        var existing = await context.Airlines.FirstOrDefaultAsync(x => x.IataCode == iata);
        if (existing is not null)
            return existing;

        var airline = new AirlineEntity
        {
            Name = "Aero Demo",
            IataCode = iata,
            OriginCountryId = countryId,
            IsActive = true
        };

        context.Airlines.Add(airline);
        await context.SaveChangesAsync();
        return airline;
    }

    private static async Task<StaffEntity> EnsureAirportStaffAsync(
        AppDbContext context,
        int documentTypeId,
        int roleId,
        int airlineId,
        int airportId)
    {
        const string documentNumber = "STAFF-CHECKIN-001";
        var person = await context.People.FirstOrDefaultAsync(x => x.DocumentNumber == documentNumber);
        if (person is null)
        {
            person = new PersonEntity
            {
                DocumentTypeId = documentTypeId,
                DocumentNumber = documentNumber,
                FirstNames = "Agente",
                LastNames = "Checkin Demo",
                BirthDate = new DateTime(1988, 5, 15),
                Gender = "N"
            };

            context.People.Add(person);
            await context.SaveChangesAsync();
        }
        else if (!string.Equals(person.Gender, "N", StringComparison.OrdinalIgnoreCase))
        {
            person.Gender = "N";
            await context.SaveChangesAsync();
        }

        var staff = await context.Staff.FirstOrDefaultAsync(x => x.PersonId == person.Id);
        if (staff is not null)
        {
            staff.RoleId = roleId;
            staff.AirlineId = airlineId;
            staff.AirportId = airportId;
            staff.IsActive = true;
            await context.SaveChangesAsync();
            return staff;
        }

        staff = new StaffEntity
        {
            PersonId = person.Id,
            RoleId = roleId,
            AirlineId = airlineId,
            AirportId = airportId,
            HireDate = DateTime.Today.AddYears(-2),
            IsActive = true
        };

        context.Staff.Add(staff);
        await context.SaveChangesAsync();
        return staff;
    }

    private static async Task<AircraftManufacturerEntity> EnsureManufacturerAsync(AppDbContext context, int countryId)
    {
        const string name = "Demo Aircraft Co";
        var existing = await context.AircraftManufacturers.FirstOrDefaultAsync(x => x.Name == name);
        if (existing is not null)
            return existing;

        var manufacturer = new AircraftManufacturerEntity
        {
            Name = name,
            CountryId = countryId
        };

        context.AircraftManufacturers.Add(manufacturer);
        await context.SaveChangesAsync();
        return manufacturer;
    }

    private static async Task<AircraftModelEntity> EnsureAircraftModelAsync(AppDbContext context, int manufacturerId)
    {
        const string modelName = "DemoJet 100";
        var existing = await context.AircraftModels.FirstOrDefaultAsync(x =>
            x.ManufacturerId == manufacturerId &&
            x.ModelName == modelName);

        if (existing is not null)
            return existing;

        var model = new AircraftModelEntity
        {
            ManufacturerId = manufacturerId,
            ModelName = modelName,
            MaxCapacity = 120
        };

        context.AircraftModels.Add(model);
        await context.SaveChangesAsync();
        return model;
    }

    private static async Task<AircraftEntity> EnsureAircraftAsync(AppDbContext context, int modelId, int airlineId)
    {
        const string registration = "HK-DEMO";
        var existing = await context.Aircraft.FirstOrDefaultAsync(x => x.Registration == registration);
        if (existing is not null)
            return existing;

        var aircraft = new AircraftEntity
        {
            ModelId = modelId,
            AirlineId = airlineId,
            Registration = registration,
            ManufactureDate = new DateTime(2020, 1, 1),
            IsActive = true
        };

        context.Aircraft.Add(aircraft);
        await context.SaveChangesAsync();
        return aircraft;
    }

    private static async Task<RouteEntity> EnsureRouteAsync(AppDbContext context, int originAirportId, int destinationAirportId)
    {
        var existing = await context.Routes.FirstOrDefaultAsync(x =>
            x.OriginAirportId == originAirportId &&
            x.DestinationAirportId == destinationAirportId);

        if (existing is not null)
            return existing;

        var route = new RouteEntity
        {
            OriginAirportId = originAirportId,
            DestinationAirportId = destinationAirportId,
            DistanceKm = 215,
            EstimatedDurationMin = 55
        };

        context.Routes.Add(route);
        await context.SaveChangesAsync();
        return route;
    }

    private static async Task<FlightEntity> EnsureFlightAsync(
        AppDbContext context,
        int airlineId,
        int routeId,
        int aircraftId,
        int stateId)
    {
        const string code = "AD100";
        var existing = await context.Flights.FirstOrDefaultAsync(x => x.Code == code);
        if (existing is not null)
            return existing;

        var departure = DateTime.Today.AddDays(7).AddHours(8);
        var flight = new FlightEntity
        {
            Code = code,
            AirlineId = airlineId,
            RouteId = routeId,
            AircraftId = aircraftId,
            DepartureDateTime = departure,
            EstimatedArrivalDateTime = departure.AddMinutes(55),
            TotalCapacity = 120,
            AvailableSeats = 120,
            StateId = stateId
        };

        context.Flights.Add(flight);
        await context.SaveChangesAsync();
        return flight;
    }

    private static async Task EnsureFlightSeatsAsync(AppDbContext context, int flightId, int cabinTypeId, int locationTypeId)
    {
        var desired = new[] { "1A", "1B", "1C", "2A", "2B", "2C" };
        var existing = await context.FlightSeats
            .Where(x => x.FlightId == flightId)
            .ToListAsync();

        foreach (var code in desired)
        {
            if (existing.Any(x => x.SeatCode == code))
                continue;

            context.FlightSeats.Add(new FlightSeatEntity
            {
                FlightId = flightId,
                SeatCode = code,
                CabinTypeId = cabinTypeId,
                LocationTypeId = locationTypeId,
                IsOccupied = false
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task<ReservationEntity> EnsureReservationAsync(AppDbContext context, int customerId, int statusId)
    {
        const string code = "DEMO01";
        var existing = await context.Reservations.FirstOrDefaultAsync(x => x.Code == code);
        if (existing is not null)
            return existing;

        var reservation = new ReservationEntity
        {
            Code = code,
            CustomerId = customerId,
            ReservedAt = DateTime.Now,
            StatusId = statusId,
            TotalAmount = 250000m,
            ExpiresAt = DateTime.Now.AddHours(2)
        };

        context.Reservations.Add(reservation);
        await context.SaveChangesAsync();
        return reservation;
    }

    private static async Task<ReservationFlightEntity> EnsureReservationFlightAsync(
        AppDbContext context,
        int reservationId,
        int flightId)
    {
        var existing = await context.ReservationFlights.FirstOrDefaultAsync(x =>
            x.ReservationId == reservationId &&
            x.FlightId == flightId);

        if (existing is not null)
            return existing;

        var reservationFlight = new ReservationFlightEntity
        {
            ReservationId = reservationId,
            FlightId = flightId,
            PartialAmount = 250000m
        };

        context.ReservationFlights.Add(reservationFlight);
        await context.SaveChangesAsync();
        return reservationFlight;
    }

    private static async Task<ReservationPassengerEntity> EnsureReservationPassengerAsync(
        AppDbContext context,
        int reservationFlightId,
        int passengerId)
    {
        var existing = await context.ReservationPassengers.FirstOrDefaultAsync(x =>
            x.ReservationFlightId == reservationFlightId &&
            x.PassengerId == passengerId);

        if (existing is not null)
            return existing;

        var reservationPassenger = new ReservationPassengerEntity
        {
            ReservationFlightId = reservationFlightId,
            PassengerId = passengerId
        };

        context.ReservationPassengers.Add(reservationPassenger);
        await context.SaveChangesAsync();
        return reservationPassenger;
    }

    private static async Task<TicketEntity> EnsureTicketAsync(
        AppDbContext context,
        int reservationPassengerId,
        int statusId,
        string ticketCode)
    {
        var existing = await context.Tickets.FirstOrDefaultAsync(x => x.ReservationPassengerId == reservationPassengerId);
        if (existing is not null)
            return existing;

        var ticket = new TicketEntity
        {
            ReservationPassengerId = reservationPassengerId,
            Code = ticketCode,
            IssuedAt = DateTime.Now,
            StatusId = statusId
        };

        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();
        return ticket;
    }
}
