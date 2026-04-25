// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentStates\Domain\ValueObject\PaymentStateId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.PaymentStates.Domain.ValueObject;

public sealed record PaymentStateId
{
    public int Value { get; }

    private PaymentStateId(int value)
    {
        Value = value;
    }

    public static PaymentStateId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new PaymentStateId(value);
    }

    public static PaymentStateId CreateEmpty()
    {
        return new PaymentStateId(0);
    }
}
