// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\Domain\ValueObject\PassengerId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Passengers.Domain.ValueObject;

public sealed record PassengerId
{
    public int Value { get; }

    private PassengerId(int value)
    {
        Value = value;
    }

    public static PassengerId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new PassengerId(value);
    }

    public static PassengerId CreateEmpty()
    {
        return new PassengerId(0);
    }
}
