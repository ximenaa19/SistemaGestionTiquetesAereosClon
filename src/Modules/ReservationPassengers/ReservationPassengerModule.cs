// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\ReservationPassengerModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.Customers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.Services;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.ReservationPassengers;

public static class ReservationPassengerModule
{
    public static ReservationPassengerMenu Build(AppDbContext context)
    {
        var reservationPassengerRepository = new ReservationPassengerRepository(context);
        var reservationFlightRepository = new ReservationFlightRepository(context);
        var passengerRepository = new PassengerRepository(context);
        var reservationRepository = new ReservationRepository(context);
        var reservationStatusRepository = new ReservationStatusRepository(context);
        var flightRepository = new FlightRepository(context);

        var validator = new ReservationPassengerValidator(
            reservationPassengerRepository,
            reservationFlightRepository,
            passengerRepository,
            reservationRepository,
            reservationStatusRepository,
            flightRepository);

        var create = new CreateReservationPassengerUseCase(reservationPassengerRepository, validator, reservationFlightRepository, flightRepository);
        var getAll = new GetAllReservationPassengersUseCase(reservationPassengerRepository);
        var getById = new GetReservationPassengerByIdUseCase(reservationPassengerRepository);
        var getByReservationFlightId = new GetReservationPassengersByReservationFlightIdUseCase(reservationPassengerRepository);
        var getByPassengerId = new GetReservationPassengersByPassengerIdUseCase(reservationPassengerRepository);
        var getByPair = new GetReservationPassengerByReservationFlightAndPassengerUseCase(reservationPassengerRepository);
        var getByReservationCode = new GetReservationPassengersByReservationCodeUseCase(reservationRepository, reservationFlightRepository, reservationPassengerRepository);
        var update = new UpdateReservationPassengerUseCase(reservationPassengerRepository, validator, reservationFlightRepository, flightRepository);
        var delete = new DeleteReservationPassengerUseCase(reservationPassengerRepository, validator, reservationFlightRepository, flightRepository);

        var customerRepository = new CustomerRepository(context);
        var personRepository = new PersonRepository(context);
        var airlineRepository = new AirlineRepository(context);
        var routeRepository = new RouteRepository(context);
        var airportRepository = new AirportRepository(context);

        var getAllReservationFlights = new GetAllReservationFlightsUseCase(reservationFlightRepository);
        var getAllReservations = new GetAllReservationsUseCase(reservationRepository);
        var getAllCustomers = new GetAllCustomersUseCase(customerRepository);
        var getAllPeople = new GetAllPeopleUseCase(personRepository);
        var getAllReservationStatuses = new GetAllReservationStatusesUseCase(reservationStatusRepository);

        var getAllFlights = new GetAllFlightsUseCase(flightRepository);
        var getAllAirlines = new GetAllAirlinesUseCase(airlineRepository);
        var getAllRoutes = new GetAllRoutesUseCase(routeRepository);
        var getAllAirports = new GetAllAirportsUseCase(airportRepository);
        var getAllPassengers = new GetAllPassengersUseCase(passengerRepository);

        return new ReservationPassengerMenu(
            create,
            getAll,
            getById,
            getByReservationFlightId,
            getByPassengerId,
            getByPair,
            getByReservationCode,
            update,
            delete,
            getAllReservationFlights,
            getAllReservations,
            getAllCustomers,
            getAllPeople,
            getAllReservationStatuses,
            getAllFlights,
            getAllAirlines,
            getAllRoutes,
            getAllAirports,
            getAllPassengers);
    }
}

