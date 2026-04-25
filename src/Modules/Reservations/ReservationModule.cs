// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\ReservationModule.cs
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
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationFlights.Application.Services;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.Services;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Application.Interfaces;
using GestionAerolineas.src.Modules.Reservations.Application.Services;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.UI;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Reservations;

public static class ReservationModule
{
    public static ReservationMenu Build(AppDbContext context)
    {
        var reservationRepository = new ReservationRepository(context);

        var customerRepository = new CustomerRepository(context);
        var reservationStatusRepository = new ReservationStatusRepository(context);
        var reservationStatusTransitionRepository = new ReservationStatusTransitionRepository(context);
        IReservationValidator reservationValidator = new ReservationValidator(
            reservationRepository,
            customerRepository,
            reservationStatusRepository,
            reservationStatusTransitionRepository);

        var createReservation = new CreateReservationUseCase(reservationRepository, reservationValidator);
        var getAll = new GetAllReservationsUseCase(reservationRepository);
        var getById = new GetReservationByIdUseCase(reservationRepository);
        var getByCode = new GetReservationByCodeUseCase(reservationRepository);
        var getByCustomerId = new GetReservationsByCustomerIdUseCase(reservationRepository);
        var getByStatusId = new GetReservationsByStatusIdUseCase(reservationRepository);
        var getByDateRange = new GetReservationsByDateRangeUseCase(reservationRepository);

        var reservationFlightRepository = new ReservationFlightRepository(context);
        var reservationPassengerRepository = new ReservationPassengerRepository(context);

        var flightRepository = new FlightRepository(context);
        var flightStateRepository = new FlightStateRepository(context);
        IReservationFlightValidator reservationFlightValidator = new ReservationFlightValidator(
            reservationFlightRepository,
            reservationRepository,
            flightRepository,
            flightStateRepository,
            reservationStatusRepository,
            reservationPassengerRepository);

        var createReservationFlight = new CreateReservationFlightUseCase(reservationFlightRepository, reservationFlightValidator, reservationRepository);

        var passengerRepository = new PassengerRepository(context);
        var reservationPassengerValidator = new ReservationPassengerValidator(
            reservationPassengerRepository,
            reservationFlightRepository,
            passengerRepository,
            reservationRepository,
            reservationStatusRepository,
            flightRepository);

        var createReservationPassenger = new CreateReservationPassengerUseCase(
            reservationPassengerRepository,
            reservationPassengerValidator,
            reservationFlightRepository,
            flightRepository);

        var getDetailsById = new GetReservationDetailsByIdUseCase(reservationRepository, reservationFlightRepository, reservationPassengerRepository);
        var updateStatus = new GestionAerolineas.src.Modules.Reservations.Application.UseCases.UpdateReservationStatusUseCase(reservationRepository, reservationValidator);
        var deleteReservation = new DeleteReservationUseCase(reservationRepository, reservationFlightRepository, reservationPassengerRepository, flightRepository);

        var personRepository = new PersonRepository(context);
        var airlineRepository = new AirlineRepository(context);
        var routeRepository = new RouteRepository(context);
        var airportRepository = new AirportRepository(context);

        var getAllCustomers = new GetAllCustomersUseCase(customerRepository);
        var getAllPeople = new GetAllPeopleUseCase(personRepository);
        var getAllReservationStatuses = new GetAllReservationStatusesUseCase(reservationStatusRepository);
        var getAllFlights = new GetAllFlightsUseCase(flightRepository);
        var getAllAirlines = new GetAllAirlinesUseCase(airlineRepository);
        var getAllRoutes = new GetAllRoutesUseCase(routeRepository);
        var getAllAirports = new GetAllAirportsUseCase(airportRepository);
        var getAllPassengers = new GetAllPassengersUseCase(passengerRepository);

        return new ReservationMenu(
            createReservation,
            getAll,
            getById,
            getByCode,
            getByCustomerId,
            getByStatusId,
            getByDateRange,
            getDetailsById,
            updateStatus,
            deleteReservation,
            createReservationFlight,
            createReservationPassenger,
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
