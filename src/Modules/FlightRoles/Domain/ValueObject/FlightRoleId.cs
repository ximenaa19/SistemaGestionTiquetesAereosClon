// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightRoles\Domain\ValueObject\FlightRoleId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;

namespace GestionAerolineas.src.Modules.FlightRoles.Domain.ValueObject;

public sealed record FlightRoleId
{
    public int Value { get; }

    private FlightRoleId(int value)
    {
        Value = value;
    }

    public static FlightRoleId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new FlightRoleId(value);
    }

    public static FlightRoleId CreateEmpty()
    {
        return new FlightRoleId(0);
    }
}

