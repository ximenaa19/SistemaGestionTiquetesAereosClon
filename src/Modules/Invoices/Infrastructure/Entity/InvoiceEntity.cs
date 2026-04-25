// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Infrastructure\Entity\InvoiceEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Invoices.Infrastructure.Entity;

public class InvoiceEntity
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateTime IssuedAt { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Taxes { get; set; }
    public decimal Total { get; set; }
    public DateTime? CreatedAt { get; set; }
}
