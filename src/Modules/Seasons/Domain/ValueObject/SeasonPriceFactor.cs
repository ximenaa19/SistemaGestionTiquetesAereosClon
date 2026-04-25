// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Domain\ValueObject\SeasonPriceFactor.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;

public sealed record SeasonPriceFactor
{
    public decimal Value { get; }

    private SeasonPriceFactor(decimal value)
    {
        Value = value;
    }

    public static SeasonPriceFactor Create(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("El precio_factor debe ser mayor que 0");

        if (value > 9.9999m)
            throw new ArgumentException("El precio_factor no puede superar 9.9999");

        if (decimal.Round(value, 4) != value)
            throw new ArgumentException("El precio_factor no puede tener mas de 4 decimales");

        return new SeasonPriceFactor(value);
    }

    public override string ToString() => Value.ToString("0.0000");
}
