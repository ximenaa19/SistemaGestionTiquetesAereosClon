// [DocHeader]
// Modulo: Reservations
// Capa: Application
// Archivo: src\Modules\Reservations\Application\UseCases\CancelReservationForCustomerUseCase.cs
// Responsabilidad: Cancela una reserva del cliente activo, libera cupos y promueve lista de espera.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Application.UseCases;

public sealed class CancelReservationForCustomerUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationFlightRepository _reservationFlightRepository;
    private readonly IReservationPassengerRepository _reservationPassengerRepository;
    private readonly FlightRepository _flightRepository;
    private readonly ReservationStatusRepository _reservationStatusRepository;
    private readonly UpdateReservationStatusUseCase _updateReservationStatus;
    private readonly PromoteWaitlistForFlightUseCase _promoteWaitlist;

    public CancelReservationForCustomerUseCase(
        IReservationRepository reservationRepository,
        IReservationFlightRepository reservationFlightRepository,
        IReservationPassengerRepository reservationPassengerRepository,
        FlightRepository flightRepository,
        ReservationStatusRepository reservationStatusRepository,
        UpdateReservationStatusUseCase updateReservationStatus,
        PromoteWaitlistForFlightUseCase promoteWaitlist)
    {
        _reservationRepository = reservationRepository;
        _reservationFlightRepository = reservationFlightRepository;
        _reservationPassengerRepository = reservationPassengerRepository;
        _flightRepository = flightRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _updateReservationStatus = updateReservationStatus;
        _promoteWaitlist = promoteWaitlist;
    }

    public async Task CancelAsync(int customerId, int reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationId));
        if (reservation is null)
            throw new Exception("La reserva no existe.");

        if (reservation.CustomerId.Value != customerId)
            throw new Exception("La reserva no pertenece al cliente activo.");

        var cancelStatus = (await _reservationStatusRepository.GetAllAsync())
            .FirstOrDefault(x => x.Name.Value.Trim().ToUpperInvariant().Contains("CANCEL"));
        if (cancelStatus is null)
            throw new Exception("No existe estado Cancelada en catalogos.");

        var reservationFlights = (await _reservationFlightRepository.GetByReservationIdAsync(
            ReservationFlightReservationId.Create(reservationId))).ToList();

        foreach (var rf in reservationFlights)
        {
            var passengers = (await _reservationPassengerRepository.GetByReservationFlightIdAsync(
                ReservationPassengerReservationFlightId.Create(rf.Id.Value))).ToList();
            var seatsToReturn = passengers.Count;
            if (seatsToReturn <= 0)
                seatsToReturn = 1;

            var flight = await _flightRepository.GetByIdAsync(FlightId.Create(rf.FlightId.Value));
            if (flight is not null)
            {
                await UpdateFlightSeatsAsync(flight, +seatsToReturn);
                await _promoteWaitlist.ExecuteAsync(flight.Id.Value, "CANCELACION_RESERVA");
            }
        }

        await _updateReservationStatus.ExecuteAsync(reservationId, cancelStatus.Id.Value);
    }

    private async Task UpdateFlightSeatsAsync(Flight flight, int delta)
    {
        var newValue = flight.AvailableSeats.Value + delta;
        if (newValue < 0) newValue = 0;
        if (newValue > flight.TotalCapacity.Value) newValue = flight.TotalCapacity.Value;

        var updated = Flight.Create(
            flight.Id,
            flight.Code,
            flight.AirlineId,
            flight.RouteId,
            flight.AircraftId,
            flight.DepartureDateTime,
            flight.EstimatedArrivalDateTime,
            flight.TotalCapacity,
            FlightAvailableSeats.Create(newValue),
            flight.StateId,
            flight.RescheduledAt);

        await _flightRepository.UpdateAsync(updated);
    }
}

