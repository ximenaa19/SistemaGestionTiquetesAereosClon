// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Infrastructure\Entity\CheckinEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Checkins.Infrastructure.Entity;

public class CheckinEntity
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int StaffId { get; set; }
    public int FlightSeatId { get; set; }
    public DateTime CheckedAt { get; set; }
    public int StatusId { get; set; }
    public string? BoardingPassNumber { get; set; }
    public bool HasHoldBaggage { get; set; }
    public decimal? BaggageWeightKg { get; set; }
}

