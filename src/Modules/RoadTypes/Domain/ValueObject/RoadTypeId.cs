// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RoadTypes\Domain\ValueObject\RoadTypeId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;

namespace GestionAerolineas.src.Modules.RoadTypes.Domain.ValueObject;

public sealed record RoadTypeId
{
    public int Value { get; }

    private RoadTypeId(int value)
    {
        Value = value;
    }

    public static RoadTypeId Create(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException("El valor no puede ser menor a 1");
        }

        return new RoadTypeId(value);
    }
}
