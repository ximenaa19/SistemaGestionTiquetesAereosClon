// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\CheckinModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CheckinStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.CheckinStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Checkins.Application.Interfaces;
using GestionAerolineas.src.Modules.Checkins.Application.Services;
using GestionAerolineas.src.Modules.Checkins.Application.UseCases;
using GestionAerolineas.src.Modules.Checkins.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Checkins.UI;
using GestionAerolineas.src.Modules.FlightSeats.Application.UseCases;
using GestionAerolineas.src.Modules.FlightSeats.Infrastructure.Repository;
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
using GestionAerolineas.src.Modules.Staff.Application.UseCases;
using GestionAerolineas.src.Modules.Staff.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Tickets.Application.UseCases;
using GestionAerolineas.src.Modules.Tickets.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Checkins;

public static class CheckinModule
{
    public static CheckinMenu Build(AppDbContext context)
    {
        var checkinRepository = new CheckinRepository(context);

        var ticketRepository = new TicketRepository(context);
        var staffRepository = new StaffRepository(context);
        var flightSeatRepository = new FlightSeatRepository(context);
        var checkinStatusRepository = new CheckinStatusRepository(context);
        var passengerRepository = new PassengerRepository(context);
        var reservationPassengerRepository = new ReservationPassengerRepository(context);
        var reservationFlightRepository = new ReservationFlightRepository(context);
        var flightRepository = new FlightRepository(context);
        var personRepository = new PersonRepository(context);

        ICheckinValidator validator = new CheckinValidator(
            checkinRepository,
            ticketRepository,
            staffRepository,
            flightSeatRepository,
            checkinStatusRepository,
            reservationPassengerRepository,
            reservationFlightRepository);

        var create = new CreateCheckinUseCase(checkinRepository, validator);
        var getAll = new GetAllCheckinsUseCase(checkinRepository);
        var getById = new GetCheckinByIdUseCase(checkinRepository);
        var getByTicketId = new GetCheckinByTicketIdUseCase(checkinRepository, validator);
        var getByPassengerId = new GetCheckinsByPassengerIdUseCase(checkinRepository, passengerRepository);
        var getByFlightId = new GetCheckinsByFlightIdUseCase(checkinRepository, flightRepository);
        var getByStatusId = new GetCheckinsByStatusIdUseCase(checkinRepository, validator);
        var getByCheckedAtRange = new GetCheckinsByCheckedAtRangeUseCase(checkinRepository);
        var update = new UpdateCheckinUseCase(checkinRepository, validator);
        var delete = new DeleteCheckinUseCase(checkinRepository);

        var getAllTickets = new GetAllTicketsUseCase(ticketRepository);
        var getAllStatuses = new GetAllCheckinStatusesUseCase(checkinStatusRepository);
        var getAllStaff = new GetAllStaffUseCase(staffRepository);
        var getAvailableSeatsByFlightId = new GetAvailableSeatsByFlightIdUseCase(flightSeatRepository);
        var getAllFlightSeats = new GetAllFlightSeatsUseCase(flightSeatRepository);
        var getAllFlights = new GetAllFlightsUseCase(flightRepository);
        var getAllReservationPassengers = new GetAllReservationPassengersUseCase(reservationPassengerRepository);
        var getAllReservationFlights = new GetAllReservationFlightsUseCase(reservationFlightRepository);
        var getAllPassengers = new GetAllPassengersUseCase(passengerRepository);
        var getAllPeople = new GetAllPeopleUseCase(personRepository);

        return new CheckinMenu(
            create,
            getAll,
            getById,
            getByTicketId,
            getByPassengerId,
            getByFlightId,
            getByStatusId,
            getByCheckedAtRange,
            update,
            delete,
            getAllTickets,
            getAllStatuses,
            getAllStaff,
            getAvailableSeatsByFlightId,
            getAllFlightSeats,
            getAllFlights,
            getAllReservationPassengers,
            getAllReservationFlights,
            getAllPassengers,
            getAllPeople);
    }
}

