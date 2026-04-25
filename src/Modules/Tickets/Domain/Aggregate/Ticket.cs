// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Domain\Aggregate\Ticket.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Tickets.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Tickets.Domain.Aggregate;

public class Ticket
{
    public TicketId Id { get; private set; }
    public TicketReservationPassengerId ReservationPassengerId { get; private set; }
    public TicketCode Code { get; private set; }
    public TicketIssuedAt IssuedAt { get; private set; }
    public TicketStatusId StatusId { get; private set; }
    public TicketCreatedAt CreatedAt { get; private set; }
    public TicketUpdatedAt UpdatedAt { get; private set; }

    private Ticket(
        TicketId id,
        TicketReservationPassengerId reservationPassengerId,
        TicketCode code,
        TicketIssuedAt issuedAt,
        TicketStatusId statusId,
        TicketCreatedAt createdAt,
        TicketUpdatedAt updatedAt)
    {
        Id = id;
        ReservationPassengerId = reservationPassengerId;
        Code = code;
        IssuedAt = issuedAt;
        StatusId = statusId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Ticket Create(
        TicketId id,
        TicketReservationPassengerId reservationPassengerId,
        TicketCode code,
        TicketIssuedAt issuedAt,
        TicketStatusId statusId,
        TicketCreatedAt createdAt,
        TicketUpdatedAt updatedAt)
    {
        return new Ticket(id, reservationPassengerId, code, issuedAt, statusId, createdAt, updatedAt);
    }

    public static Ticket CreateNew(
        TicketReservationPassengerId reservationPassengerId,
        TicketCode code,
        TicketIssuedAt issuedAt,
        TicketStatusId statusId)
    {
        return new Ticket(
            TicketId.CreateEmpty(),
            reservationPassengerId,
            code,
            issuedAt,
            statusId,
            TicketCreatedAt.CreateOptional(null),
            TicketUpdatedAt.CreateOptional(null));
    }
}

