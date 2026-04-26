namespace GestionAerolineas.src.Modules.Baggage.Domain.Models;

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
