// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\TicketModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;
using GestionAerolineas.src.Modules.TicketStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.TicketStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Tickets.Application.Interfaces;
using GestionAerolineas.src.Modules.Tickets.Application.Services;
using GestionAerolineas.src.Modules.Tickets.Application.UseCases;
using GestionAerolineas.src.Modules.Tickets.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Tickets.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Tickets;

public static class TicketModule
{
    public static TicketMenu Build(AppDbContext context)
    {
        var ticketRepository = new TicketRepository(context);

        var reservationPassengerRepository = new ReservationPassengerRepository(context);
        var reservationFlightRepository = new ReservationFlightRepository(context);
        var reservationRepository = new ReservationRepository(context);
        var reservationStatusRepository = new ReservationStatusRepository(context);
        var ticketStatusRepository = new TicketStatusRepository(context);
        var passengerRepository = new PassengerRepository(context);

        ITicketValidator validator = new TicketValidator(
            ticketRepository,
            reservationPassengerRepository,
            reservationFlightRepository,
            reservationRepository,
            reservationStatusRepository,
            ticketStatusRepository);

        var create = new CreateTicketUseCase(ticketRepository, validator);
        var getAll = new GetAllTicketsUseCase(ticketRepository);
        var getById = new GetTicketByIdUseCase(ticketRepository);
        var getByCode = new GetTicketByCodeUseCase(ticketRepository);
        var getByReservationPassengerId = new GetTicketByReservationPassengerIdUseCase(ticketRepository);
        var getByStatusId = new GetTicketsByStatusIdUseCase(ticketRepository);
        var getByPassengerId = new GetTicketsByPassengerIdUseCase(ticketRepository, passengerRepository);
        var getByReservationCode = new GetTicketsByReservationCodeUseCase(ticketRepository);
        var update = new UpdateTicketUseCase(ticketRepository, validator);
        var delete = new DeleteTicketUseCase(ticketRepository);

        var getAllStatuses = new GetAllTicketStatusesUseCase(ticketStatusRepository);
        var getAllReservationPassengers = new GetAllReservationPassengersUseCase(reservationPassengerRepository);
        var getAllReservationFlights = new GetAllReservationFlightsUseCase(reservationFlightRepository);
        var getAllReservations = new GetAllReservationsUseCase(reservationRepository);

        var flightRepository = new FlightRepository(context);
        var getAllFlights = new GetAllFlightsUseCase(flightRepository);

        var getAllPassengers = new GetAllPassengersUseCase(passengerRepository);

        var peopleRepository = new PersonRepository(context);
        var getAllPeople = new GetAllPeopleUseCase(peopleRepository);

        return new TicketMenu(
            create,
            getAll,
            getById,
            getByCode,
            getByReservationPassengerId,
            getByStatusId,
            getByPassengerId,
            getByReservationCode,
            update,
            delete,
            getAllStatuses,
            getAllReservationPassengers,
            getAllReservationFlights,
            getAllReservations,
            getAllFlights,
            getAllPassengers,
            getAllPeople);
    }
}

