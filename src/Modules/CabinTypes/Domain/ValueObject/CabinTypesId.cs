// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinTypes\Domain\ValueObject\CabinTypesId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;

namespace GestionAerolineas.src.Modules.CabinTypes.Domain.ValueObject;

public class CabinTypesId
{
    public int Value { get; }

    private CabinTypesId(int value)
    {
        Value = value;
    }

    public static CabinTypesId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new CabinTypesId(value);
    }

    public static CabinTypesId CreateEmpty()
    {
        return new CabinTypesId(0);
    }


}
