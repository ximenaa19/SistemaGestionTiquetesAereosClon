// [DocHeader]
// Modulo: Reservations
// Capa: Application
// Archivo: src\Modules\Reservations\Application\UseCases\PromoteWaitlistForFlightUseCase.cs
// Responsabilidad: Promueve automaticamente al primer candidato en espera cuando se libera cupo.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Reservations.Application.UseCases;

public sealed class PromoteWaitlistForFlightUseCase
{
    private readonly IReservationWaitlistRepository _waitlistRepository;
    private readonly IReservationRescheduleHistoryRepository _historyRepository;
    private readonly IReservationFlightRepository _reservationFlightRepository;
    private readonly IReservationPassengerRepository _reservationPassengerRepository;
    private readonly FlightRepository _flightRepository;
    private readonly UpdateReservationFlightUseCase _updateReservationFlight;

    public PromoteWaitlistForFlightUseCase(
        IReservationWaitlistRepository waitlistRepository,
        IReservationRescheduleHistoryRepository historyRepository,
        IReservationFlightRepository reservationFlightRepository,
        IReservationPassengerRepository reservationPassengerRepository,
        FlightRepository flightRepository,
        UpdateReservationFlightUseCase updateReservationFlight)
    {
        _waitlistRepository = waitlistRepository;
        _historyRepository = historyRepository;
        _reservationFlightRepository = reservationFlightRepository;
        _reservationPassengerRepository = reservationPassengerRepository;
        _flightRepository = flightRepository;
        _updateReservationFlight = updateReservationFlight;
    }

    public async Task<bool> ExecuteAsync(int releasedFlightId, string trigger)
    {
        var pending = await _waitlistRepository.GetFirstPendingByRequestedFlightIdAsync(releasedFlightId);
        if (pending is null)
            return false;

        var requestedFlight = await _flightRepository.GetByIdAsync(FlightId.Create(releasedFlightId));
        if (requestedFlight is null)
            return false;

        var reservationFlight = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(pending.ReservationFlightId));
        if (reservationFlight is null)
            return false;

        var passengers = (await _reservationPassengerRepository.GetByReservationFlightIdAsync(
            ReservationPassengerReservationFlightId.Create(reservationFlight.Id.Value))).ToList();
        var seatsNeeded = passengers.Count;
        if (seatsNeeded <= 0)
            seatsNeeded = 1;

        if (requestedFlight.AvailableSeats.Value < seatsNeeded)
            return false;

        var oldFlight = await _flightRepository.GetByIdAsync(FlightId.Create(reservationFlight.FlightId.Value));
        if (oldFlight is null)
            return false;

        await _updateReservationFlight.ExecuteAsync(
            reservationFlight.Id.Value,
            reservationFlight.ReservationId.Value,
            releasedFlightId,
            reservationFlight.PartialAmount.Value);

        if (oldFlight.Id.Value != releasedFlightId)
            await UpdateFlightSeatsAsync(oldFlight, +seatsNeeded);

        var freshRequested = await _flightRepository.GetByIdAsync(FlightId.Create(releasedFlightId));
        if (freshRequested is not null)
            await UpdateFlightSeatsAsync(freshRequested, -seatsNeeded);

        var completed = ReservationWaitlistEntry.Rehydrate(
            pending.Id,
            pending.ReservationId,
            pending.ReservationFlightId,
            pending.RequestedFlightId,
            pending.RequestedAt,
            pending.QueueOrder,
            "PROMOVIDA",
            pending.Reason,
            DateTime.Now);
        await _waitlistRepository.UpdateAsync(completed);

        var history = ReservationRescheduleHistory.CreateNew(
            pending.ReservationId,
            oldFlight.Id.Value,
            releasedFlightId,
            $"Promocion automatica por cupo liberado ({trigger})",
            "PROMOTED");
        await _historyRepository.AddAsync(history);

        return true;
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

