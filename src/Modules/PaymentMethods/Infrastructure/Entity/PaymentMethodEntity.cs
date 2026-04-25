// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\Infrastructure\Entity\PaymentMethodEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.PaymentMethods.Infrastructure.Entity;

public class PaymentMethodEntity
{
    public int Id { get; set; }
    public int PaymentMethodTypeId { get; set; }
    public int? CardTypeId { get; set; }
    public int? CardIssuerId { get; set; }
    public string? CommercialName { get; set; }
}

