// [DocHeader]
// Modulo: Reservations
// Capa: Infrastructure
// Archivo: src\Modules\Reservations\Infrastructure\Entity\ReservationRescheduleHistoryEntity.cs
// Responsabilidad: Modelo EF para trazabilidad de reprogramaciones/promociones.
namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;

public sealed class ReservationRescheduleHistoryEntity
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public int OldFlightId { get; set; }
    public int NewFlightId { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? Reason { get; set; }
    public string ActionStatus { get; set; } = string.Empty;
}

