// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Regions\Domain\ValueObject\RegionId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Regions.Domain.ValueObject;

public sealed record RegionId
{
    public int Value { get; }

    private RegionId(int value)
    {
        Value = value;
    }

    public static RegionId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new RegionId(value);
    }

    public static RegionId CreateEmpty()
    {
        return new RegionId(0);
    }
}


