// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PassengerTypes\Domain\ValueObject\PassengerTypeId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;

namespace GestionAerolineas.src.Modules.PassengerTypes.Domain.ValueObject;

public sealed record PassengerTypeId
{
    public int Value { get; }

    private PassengerTypeId(int value)
    {
        Value = value;
    }

    public static PassengerTypeId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new PassengerTypeId(value);
    }

    public static PassengerTypeId CreateEmpty()
    {
        return new PassengerTypeId(0);
    }
}

