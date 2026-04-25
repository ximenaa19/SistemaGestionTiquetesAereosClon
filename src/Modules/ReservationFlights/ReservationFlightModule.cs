// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\ReservationFlightModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.Customers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.FlightStates.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationFlights.Application.Services;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.ReservationFlights;

public static class ReservationFlightModule
{
    public static ReservationFlightMenu Build(AppDbContext context)
    {
        var reservationRepository = new ReservationRepository(context);
        var reservationFlightRepository = new ReservationFlightRepository(context);

        var flightRepository = new FlightRepository(context);
        var flightStateRepository = new FlightStateRepository(context);
        var reservationStatusRepository = new ReservationStatusRepository(context);
        var reservationPassengerRepository = new ReservationPassengerRepository(context);

        IReservationFlightValidator validator = new ReservationFlightValidator(
            reservationFlightRepository,
            reservationRepository,
            flightRepository,
            flightStateRepository,
            reservationStatusRepository,
            reservationPassengerRepository);

        var create = new CreateReservationFlightUseCase(reservationFlightRepository, validator, reservationRepository);
        var getAll = new GetAllReservationFlightsUseCase(reservationFlightRepository);
        var getById = new GetReservationFlightByIdUseCase(reservationFlightRepository);
        var getByReservationId = new GetReservationFlightsByReservationIdUseCase(reservationFlightRepository);
        var getByFlightId = new GetReservationFlightsByFlightIdUseCase(reservationFlightRepository);
        var getByPair = new GetReservationFlightByReservationAndFlightUseCase(reservationFlightRepository);
        var getByReservationCode = new GetReservationFlightsByReservationCodeUseCase(reservationRepository, reservationFlightRepository);
        var update = new UpdateReservationFlightUseCase(reservationFlightRepository, validator, reservationRepository);
        var delete = new DeleteReservationFlightUseCase(reservationFlightRepository, reservationRepository, validator);

        var customerRepository = new CustomerRepository(context);
        var personRepository = new PersonRepository(context);
        var airlineRepository = new AirlineRepository(context);
        var routeRepository = new RouteRepository(context);
        var airportRepository = new AirportRepository(context);

        var getAllReservations = new GetAllReservationsUseCase(reservationRepository);
        var getAllCustomers = new GetAllCustomersUseCase(customerRepository);
        var getAllPeople = new GetAllPeopleUseCase(personRepository);
        var getAllReservationStatuses = new GetAllReservationStatusesUseCase(reservationStatusRepository);

        var getAllFlights = new GetAllFlightsUseCase(flightRepository);
        var getAllAirlines = new GetAllAirlinesUseCase(airlineRepository);
        var getAllRoutes = new GetAllRoutesUseCase(routeRepository);
        var getAllAirports = new GetAllAirportsUseCase(airportRepository);

        return new ReservationFlightMenu(
            create,
            getAll,
            getById,
            getByReservationId,
            getByFlightId,
            getByPair,
            getByReservationCode,
            update,
            delete,
            getAllReservations,
            getAllCustomers,
            getAllPeople,
            getAllReservationStatuses,
            getAllFlights,
            getAllAirlines,
            getAllRoutes,
            getAllAirports);
    }
}

