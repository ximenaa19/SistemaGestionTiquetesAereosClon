namespace GestionAerolineas.src.Modules.Baggage.Domain.Models;

//datos que el sistema muestra antes de registrar equipaje
public sealed record BaggageRegistrationContext(
    int ReservationId,
    string? ReservationCode,
    int? TicketId,
    string? TicketCode,
    int? ReservationPassengerId,
    int? FlightId,
    string? FlightCode,
    int? PassengerId,
    string? PassengerName,
    decimal CurrentReservationTotal);
