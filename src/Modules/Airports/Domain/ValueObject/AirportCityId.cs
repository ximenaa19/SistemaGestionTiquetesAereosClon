// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\Domain\ValueObject\AirportCityId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Airports.Domain.ValueObject;

public sealed record AirportCityId
{
    public int Value { get; }

    private AirportCityId(int value)
    {
        Value = value;
    }

    public static AirportCityId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new AirportCityId(value);
    }
}
