// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Domain\ValueObject\PhoneCodeId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;

namespace GestionAerolineas.src.Modules.PhoneCodes.Domain.ValueObject;

public sealed record PhoneCodeId
{
    public int Value { get; }

    private PhoneCodeId(int value)
    {
        Value = value;
    }

    public static PhoneCodeId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new PhoneCodeId(value);
    }

    public static PhoneCodeId CreateEmpty()
    {
        return new PhoneCodeId(0);
    }
}

