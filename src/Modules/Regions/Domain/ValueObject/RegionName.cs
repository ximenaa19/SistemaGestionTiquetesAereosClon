// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Regions\Domain\ValueObject\RegionName.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Regions.Domain.ValueObject;

public sealed record RegionName
{
    public string Value { get; }

    private RegionName(string value)
    {
        Value = value;
    }

    public static RegionName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("El valor no puede ser nulo ni vacío");
        }

        value = value.Trim();

        var letterCount = value.Count(char.IsLetter);

        if (letterCount < 3)
        {
            throw new ArgumentException("El nombre debe tener al menos 3 letras");
        }

        if (value.Any(char.IsDigit))
        {
            throw new ArgumentException("El nombre no puede contener números");
        }

        if (value.Length > 100)
        {
            throw new ArgumentException("El valor no puede tener más de 100 caracteres");
        }

        return new RegionName(value);
    }

    public static string Normalize(string value)
    {
        return (value ?? string.Empty).Trim().ToUpperInvariant();
    }
}
