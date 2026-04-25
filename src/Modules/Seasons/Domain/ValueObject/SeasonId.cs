// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Domain\ValueObject\SeasonId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;

public sealed record SeasonId
{
    public int Value { get; }

    private SeasonId(int value)
    {
        Value = value;
    }

    public static SeasonId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new SeasonId(value);
    }

    public static SeasonId CreateEmpty()
    {
        return new SeasonId(0);
    }
}
