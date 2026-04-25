// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Domain\ValueObject\PaymentUpdatedAt.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Payments.Domain.ValueObject;

public sealed record PaymentUpdatedAt
{
    public DateTime? Value { get; }

    private PaymentUpdatedAt(DateTime? value)
    {
        Value = value;
    }

    public static PaymentUpdatedAt CreateOptional(DateTime? value)
    {
        return new PaymentUpdatedAt(value);
    }
}

