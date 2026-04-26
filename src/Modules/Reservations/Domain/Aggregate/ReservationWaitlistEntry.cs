// [DocHeader]
// Modulo: Reservations
// Capa: Domain
// Archivo: src\Modules\Reservations\Domain\Aggregate\ReservationWaitlistEntry.cs
// Responsabilidad: Representa una solicitud de lista de espera para mover una reserva a un vuelo sin cupo.
namespace GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;

public sealed class ReservationWaitlistEntry
{
    public int Id { get; private set; }
    public int ReservationId { get; private set; }
    public int ReservationFlightId { get; private set; }
    public int RequestedFlightId { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public int QueueOrder { get; private set; }
    public string Status { get; private set; }
    public string? Reason { get; private set; }
    public DateTime? ProcessedAt { get; private set; }

    private ReservationWaitlistEntry(
        int id,
        int reservationId,
        int reservationFlightId,
        int requestedFlightId,
        DateTime requestedAt,
        int queueOrder,
        string status,
        string? reason,
        DateTime? processedAt)
    {
        Id = id;
        ReservationId = reservationId;
        ReservationFlightId = reservationFlightId;
        RequestedFlightId = requestedFlightId;
        RequestedAt = requestedAt;
        QueueOrder = queueOrder;
        Status = status;
        Reason = reason;
        ProcessedAt = processedAt;
    }

    public static ReservationWaitlistEntry CreateNew(
        int reservationId,
        int reservationFlightId,
        int requestedFlightId,
        int queueOrder,
        string? reason)
    {
        if (reservationId <= 0) throw new ArgumentException("reservation_id invalido");
        if (reservationFlightId <= 0) throw new ArgumentException("reservation_flight_id invalido");
        if (requestedFlightId <= 0) throw new ArgumentException("requested_flight_id invalido");
        if (queueOrder <= 0) throw new ArgumentException("queue_order invalido");

        return new ReservationWaitlistEntry(
            id: 0,
            reservationId: reservationId,
            reservationFlightId: reservationFlightId,
            requestedFlightId: requestedFlightId,
            requestedAt: DateTime.Now,
            queueOrder: queueOrder,
            status: "PENDIENTE",
            reason: reason,
            processedAt: null);
    }

    public static ReservationWaitlistEntry Rehydrate(
        int id,
        int reservationId,
        int reservationFlightId,
        int requestedFlightId,
        DateTime requestedAt,
        int queueOrder,
        string status,
        string? reason,
        DateTime? processedAt)
    {
        return new ReservationWaitlistEntry(
            id,
            reservationId,
            reservationFlightId,
            requestedFlightId,
            requestedAt,
            queueOrder,
            status,
            reason,
            processedAt);
    }
}

