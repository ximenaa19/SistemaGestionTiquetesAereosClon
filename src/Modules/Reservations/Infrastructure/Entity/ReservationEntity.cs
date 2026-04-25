// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Infrastructure\Entity\ReservationEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;

public class ReservationEntity
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public int CustomerId { get; set; }
    public DateTime ReservedAt { get; set; }
    public int StatusId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

