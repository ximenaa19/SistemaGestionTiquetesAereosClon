// [DocHeader]
// Modulo: Reservations
// Capa: Application
// Archivo: src\Modules\Reservations\Application\UseCases\AddReservationToWaitlistUseCase.cs
// Responsabilidad: Registra una solicitud en lista de espera para reprogramacion.
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Application.UseCases;

public sealed class AddReservationToWaitlistUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationFlightRepository _reservationFlightRepository;
    private readonly IReservationWaitlistRepository _waitlistRepository;
    private readonly IReservationRescheduleHistoryRepository _historyRepository;

    public AddReservationToWaitlistUseCase(
        IReservationRepository reservationRepository,
        IReservationFlightRepository reservationFlightRepository,
        IReservationWaitlistRepository waitlistRepository,
        IReservationRescheduleHistoryRepository historyRepository)
    {
        _reservationRepository = reservationRepository;
        _reservationFlightRepository = reservationFlightRepository;
        _waitlistRepository = waitlistRepository;
        _historyRepository = historyRepository;
    }

    public async Task AddAsync(int reservationId, int reservationFlightId, int requestedFlightId, string? reason)
    {
        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationId));
        if (reservation is null)
            throw new Exception("La reserva no existe.");

        var reservationFlight = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(reservationFlightId));
        if (reservationFlight is null || reservationFlight.ReservationId.Value != reservationId)
            throw new Exception("El tramo de reserva no existe o no pertenece a la reserva.");

        if (reservationFlight.FlightId.Value == requestedFlightId)
            throw new Exception("El nuevo vuelo debe ser diferente al vuelo actual.");

        var existsPending = await _waitlistRepository
            .ExistsPendingByReservationFlightAndRequestedFlightAsync(reservationFlightId, requestedFlightId);
        if (existsPending)
            throw new Exception("La reserva ya esta en lista de espera para ese vuelo.");

        var queueOrder = await _waitlistRepository.GetNextQueueOrderForRequestedFlightAsync(requestedFlightId);
        var entry = ReservationWaitlistEntry.CreateNew(
            reservationId,
            reservationFlightId,
            requestedFlightId,
            queueOrder,
            reason);

        await _waitlistRepository.AddAsync(entry);

        var history = ReservationRescheduleHistory.CreateNew(
            reservationId,
            reservationFlight.FlightId.Value,
            requestedFlightId,
            reason,
            "WAITLIST");
        await _historyRepository.AddAsync(history);
    }
}

