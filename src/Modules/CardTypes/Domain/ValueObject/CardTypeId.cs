// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardTypes\Domain\ValueObject\CardTypeId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.CardTypes.Domain.ValueObject;

public sealed record CardTypeId
{
    public int Value { get; }

    private CardTypeId(int value)
    {
        Value = value;
    }

    public static CardTypeId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new CardTypeId(value);
    }

    public static CardTypeId CreateEmpty()
    {
        return new CardTypeId(0);
    }
}
