// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Infrastructure\Entity\InvoiceItemEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.InvoiceItems.Infrastructure.Entity;

public class InvoiceItemEntity
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int ItemTypeId { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    public int? ReservationPassengerId { get; set; }
}

