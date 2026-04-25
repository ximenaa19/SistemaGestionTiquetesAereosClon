// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Domain\Aggregate\Reservation.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;

public class Reservation
{
    public ReservationId Id { get; private set; }
    public ReservationCode? Code { get; private set; }
    public ReservationCustomerId CustomerId { get; private set; }
    public ReservationReservedAt ReservedAt { get; private set; }
    public ReservationStatusId StatusId { get; private set; }
    public ReservationTotalAmount TotalAmount { get; private set; }
    public ReservationExpiresAt ExpiresAt { get; private set; }
    public ReservationCreatedAt CreatedAt { get; private set; }
    public ReservationUpdatedAt UpdatedAt { get; private set; }

    private Reservation(
        ReservationId id,
        ReservationCode? code,
        ReservationCustomerId customerId,
        ReservationReservedAt reservedAt,
        ReservationStatusId statusId,
        ReservationTotalAmount totalAmount,
        ReservationExpiresAt expiresAt,
        ReservationCreatedAt createdAt,
        ReservationUpdatedAt updatedAt)
    {
        Id = id;
        Code = code;
        CustomerId = customerId;
        ReservedAt = reservedAt;
        StatusId = statusId;
        TotalAmount = totalAmount;
        ExpiresAt = expiresAt;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Reservation Create(
        ReservationId id,
        ReservationCode? code,
        ReservationCustomerId customerId,
        ReservationReservedAt reservedAt,
        ReservationStatusId statusId,
        ReservationTotalAmount totalAmount,
        ReservationExpiresAt expiresAt,
        ReservationCreatedAt createdAt,
        ReservationUpdatedAt updatedAt)
    {
        return new Reservation(id, code, customerId, reservedAt, statusId, totalAmount, expiresAt, createdAt, updatedAt);
    }

    public static Reservation CreateNew(
        ReservationCode code,
        ReservationCustomerId customerId,
        ReservationStatusId statusId,
        ReservationExpiresAt expiresAt)
    {
        var now = DateTime.Now;

        return new Reservation(
            ReservationId.CreateEmpty(),
            code,
            customerId,
            ReservationReservedAt.Create(now),
            statusId,
            ReservationTotalAmount.Create(0),
            expiresAt,
            ReservationCreatedAt.CreateOptional(null),
            ReservationUpdatedAt.CreateOptional(null));
    }
}

