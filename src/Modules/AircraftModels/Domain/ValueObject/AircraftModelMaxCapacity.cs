// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\Domain\ValueObject\AircraftModelMaxCapacity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.AircraftModels.Domain.ValueObject;

public sealed record AircraftModelMaxCapacity
{
    public int Value { get; }

    private AircraftModelMaxCapacity(int value)
    {
        Value = value;
    }

    public static AircraftModelMaxCapacity Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("La capacidad máxima debe ser mayor a 0");

        return new AircraftModelMaxCapacity(value);
    }
}

