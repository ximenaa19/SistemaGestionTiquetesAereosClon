// [DocHeader]
// Modulo: Reservations
// Capa: Domain
// Archivo: src\Modules\Reservations\Domain\Aggregate\ReservationRescheduleHistory.cs
// Responsabilidad: Traza cambios de reprogramacion/lista de espera asociados a reservas.
namespace GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;

public sealed class ReservationRescheduleHistory
{
    public int Id { get; private set; }
    public int ReservationId { get; private set; }
    public int OldFlightId { get; private set; }
    public int NewFlightId { get; private set; }
    public DateTime ChangedAt { get; private set; }
    public string? Reason { get; private set; }
    public string ActionStatus { get; private set; }

    private ReservationRescheduleHistory(
        int id,
        int reservationId,
        int oldFlightId,
        int newFlightId,
        DateTime changedAt,
        string? reason,
        string actionStatus)
    {
        Id = id;
        ReservationId = reservationId;
        OldFlightId = oldFlightId;
        NewFlightId = newFlightId;
        ChangedAt = changedAt;
        Reason = reason;
        ActionStatus = actionStatus;
    }

    public static ReservationRescheduleHistory CreateNew(
        int reservationId,
        int oldFlightId,
        int newFlightId,
        string? reason,
        string actionStatus)
    {
        if (reservationId <= 0) throw new ArgumentException("reservation_id invalido");
        if (oldFlightId <= 0) throw new ArgumentException("old_flight_id invalido");
        if (newFlightId <= 0) throw new ArgumentException("new_flight_id invalido");
        if (string.IsNullOrWhiteSpace(actionStatus)) throw new ArgumentException("action_status es obligatorio");

        return new ReservationRescheduleHistory(
            id: 0,
            reservationId: reservationId,
            oldFlightId: oldFlightId,
            newFlightId: newFlightId,
            changedAt: DateTime.Now,
            reason: string.IsNullOrWhiteSpace(reason) ? null : reason.Trim(),
            actionStatus: actionStatus.Trim().ToUpperInvariant());
    }

    public static ReservationRescheduleHistory Rehydrate(
        int id,
        int reservationId,
        int oldFlightId,
        int newFlightId,
        DateTime changedAt,
        string? reason,
        string actionStatus)
    {
        return new ReservationRescheduleHistory(
            id,
            reservationId,
            oldFlightId,
            newFlightId,
            changedAt,
            reason,
            actionStatus);
    }
}

