// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Countries\Domain\ValueObject\CountryCodigoIso.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Countries.Domain.ValueObject;

public sealed record CountryCodigoIso
{
    public string Value { get; }

    private CountryCodigoIso(string value)
    {
        Value = value;
    }

    public static CountryCodigoIso Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El código ISO no puede estar vacío");

        var trimmed = value.Trim().ToUpperInvariant();

        if (trimmed.Length != 3)
            throw new ArgumentException("El código ISO debe tener exactamente 3 caracteres");

        if (!trimmed.All(char.IsLetter))
            throw new ArgumentException("El código ISO solo puede contener letras");

        return new CountryCodigoIso(trimmed);
    }

    public static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        return value.Trim().ToUpperInvariant();
    }

    public override string ToString() => Value;
}

