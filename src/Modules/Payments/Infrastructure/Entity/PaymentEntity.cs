// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Infrastructure\Entity\PaymentEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Payments.Infrastructure.Entity;

public class PaymentEntity
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
    public int StateId { get; set; }
    public int MethodId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

