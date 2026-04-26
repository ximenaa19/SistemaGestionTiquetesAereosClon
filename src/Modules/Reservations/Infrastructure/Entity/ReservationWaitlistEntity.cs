// [DocHeader]
// Modulo: Reservations
// Capa: Infrastructure
// Archivo: src\Modules\Reservations\Infrastructure\Entity\ReservationWaitlistEntity.cs
// Responsabilidad: Modelo EF para solicitudes de lista de espera de reprogramacion.
namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;

public sealed class ReservationWaitlistEntity
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public int ReservationFlightId { get; set; }
    public int RequestedFlightId { get; set; }
    public DateTime RequestedAt { get; set; }
    public int QueueOrder { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public DateTime? ProcessedAt { get; set; }
}

