// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Infrastructure\Entity\TicketEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Tickets.Infrastructure.Entity;

public class TicketEntity
{
    public int Id { get; set; }
    public int ReservationPassengerId { get; set; }
    public string? Code { get; set; }
    public DateTime IssuedAt { get; set; }
    public int StatusId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

