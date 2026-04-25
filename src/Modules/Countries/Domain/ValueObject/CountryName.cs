// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Countries\Domain\ValueObject\CountryName.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Countries.Domain.ValueObject;

public sealed record CountryName
{
    public string Value { get; }

    private CountryName(string value)
    {
        Value = value;
    }

    public static CountryName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre no puede estar vacío");

        var trimmed = value.Trim();

        if (trimmed.Length > 100)
            throw new ArgumentException("Máximo 100 caracteres");

        return new CountryName(trimmed);
    }

    public override string ToString() => Value;
}

