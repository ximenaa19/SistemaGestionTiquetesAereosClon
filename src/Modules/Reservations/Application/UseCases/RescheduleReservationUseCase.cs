// [DocHeader]
// Modulo: Reservations
// Capa: Application
// Archivo: src\Modules\Reservations\Application\UseCases\RescheduleReservationUseCase.cs
// Responsabilidad: Reprograma una reserva confirmada a otro vuelo compatible con cupo.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Application.UseCases;

public sealed class RescheduleReservationUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationFlightRepository _reservationFlightRepository;
    private readonly IReservationPassengerRepository _reservationPassengerRepository;
    private readonly FlightRepository _flightRepository;
    private readonly ReservationStatusRepository _reservationStatusRepository;
    private readonly UpdateReservationStatusUseCase _updateReservationStatus;
    private readonly UpdateReservationFlightUseCase _updateReservationFlight;
    private readonly IReservationRescheduleHistoryRepository _historyRepository;
    private readonly PromoteWaitlistForFlightUseCase _promoteWaitlist;

    public RescheduleReservationUseCase(
        IReservationRepository reservationRepository,
        IReservationFlightRepository reservationFlightRepository,
        IReservationPassengerRepository reservationPassengerRepository,
        FlightRepository flightRepository,
        ReservationStatusRepository reservationStatusRepository,
        UpdateReservationStatusUseCase updateReservationStatus,
        UpdateReservationFlightUseCase updateReservationFlight,
        IReservationRescheduleHistoryRepository historyRepository,
        PromoteWaitlistForFlightUseCase promoteWaitlist)
    {
        _reservationRepository = reservationRepository;
        _reservationFlightRepository = reservationFlightRepository;
        _reservationPassengerRepository = reservationPassengerRepository;
        _flightRepository = flightRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _updateReservationStatus = updateReservationStatus;
        _updateReservationFlight = updateReservationFlight;
        _historyRepository = historyRepository;
        _promoteWaitlist = promoteWaitlist;
    }

    public async Task ExecuteAsync(int customerId, int reservationId, int reservationFlightId, int newFlightId, string? reason)
    {
        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationId));
        if (reservation is null)
            throw new Exception("La reserva no existe.");

        if (reservation.CustomerId.Value != customerId)
            throw new Exception("La reserva no pertenece al cliente activo.");

        var status = await _reservationStatusRepository.GetByIdAsync(
            GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject.ReservationStatusId.Create(reservation.StatusId.Value));
        var statusName = (status?.Name.Value ?? string.Empty).Trim().ToUpperInvariant();
        if (!statusName.Contains("CONFIRM"))
            throw new Exception("Solo se pueden reprogramar reservas confirmadas.");

        var rf = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(reservationFlightId));
        if (rf is null || rf.ReservationId.Value != reservationId)
            throw new Exception("El vuelo asociado a la reserva no es valido.");

        if (rf.FlightId.Value == newFlightId)
            throw new Exception("El nuevo vuelo no puede ser el mismo vuelo actual.");

        var oldFlight = await _flightRepository.GetByIdAsync(FlightId.Create(rf.FlightId.Value));
        var newFlight = await _flightRepository.GetByIdAsync(FlightId.Create(newFlightId));
        if (oldFlight is null || newFlight is null)
            throw new Exception("No se encontraron los vuelos de la reprogramacion.");

        if (newFlight.RouteId.Value != oldFlight.RouteId.Value)
            throw new Exception("El nuevo vuelo no es compatible con la misma ruta.");
        if (newFlight.DepartureDateTime.Value <= DateTime.Now)
            throw new Exception("La fecha del nuevo vuelo ya no es valida.");

        var passengers = (await _reservationPassengerRepository.GetByReservationFlightIdAsync(
            ReservationPassengerReservationFlightId.Create(rf.Id.Value))).ToList();
        var seatsNeeded = passengers.Count;
        if (seatsNeeded <= 0)
            seatsNeeded = 1;

        if (newFlight.AvailableSeats.Value < seatsNeeded)
            throw new Exception("El vuelo seleccionado no tiene cupo disponible.");

        await _updateReservationFlight.ExecuteAsync(
            rf.Id.Value,
            rf.ReservationId.Value,
            newFlightId,
            rf.PartialAmount.Value);

        await UpdateFlightSeatsAsync(oldFlight, +seatsNeeded);
        await _promoteWaitlist.ExecuteAsync(oldFlight.Id.Value, "REPROGRAMACION_RESERVA");
        var refreshedNewFlight = await _flightRepository.GetByIdAsync(FlightId.Create(newFlightId));
        if (refreshedNewFlight is not null)
            await UpdateFlightSeatsAsync(refreshedNewFlight, -seatsNeeded);

        var reprogrammedStatus = await FindStatusIdByNameContainsAsync("REPROGRAM");
        if (reprogrammedStatus.HasValue && reprogrammedStatus.Value != reservation.StatusId.Value)
        {
            try
            {
                await _updateReservationStatus.ExecuteAsync(reservationId, reprogrammedStatus.Value);
            }
            catch
            {
                // Si la transición no existe en catálogo, mantenemos el estado actual para no romper flujo.
            }
        }

        var history = ReservationRescheduleHistory.CreateNew(
            reservationId,
            oldFlight.Id.Value,
            newFlightId,
            reason,
            "RESCHEDULED");
        await _historyRepository.AddAsync(history);
    }

    private async Task<int?> FindStatusIdByNameContainsAsync(string token)
    {
        var statuses = await _reservationStatusRepository.GetAllAsync();
        var found = statuses.FirstOrDefault(x => x.Name.Value.Trim().ToUpperInvariant().Contains(token));
        return found?.Id.Value;
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
