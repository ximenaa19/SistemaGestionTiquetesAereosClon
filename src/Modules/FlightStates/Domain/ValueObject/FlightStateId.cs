// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStates\Domain\ValueObject\FlightStateId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.FlightStates.Domain.ValueObject;

public sealed record FlightStateId
{
    public int Value { get; }

    private FlightStateId(int value)
    {
        Value = value;
    }

    public static FlightStateId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new FlightStateId(value);
    }

    public static FlightStateId CreateEmpty()
    {
        return new FlightStateId(0);
    }
}
