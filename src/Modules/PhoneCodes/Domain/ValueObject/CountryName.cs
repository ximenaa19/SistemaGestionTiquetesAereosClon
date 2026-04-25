// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Domain\ValueObject\CountryName.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Text.RegularExpressions;

namespace GestionAerolineas.src.Modules.PhoneCodes.Domain.ValueObject;

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
            throw new ArgumentException("El nombre país no puede estar vacío");

        if (value.Length > 100)
            throw new ArgumentException("Máximo 100 caracteres");

        var trimmed = value.Trim();
        var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");

        if (!regex.IsMatch(trimmed))
            throw new ArgumentException("Solo letras y espacios");

        return new CountryName(trimmed);
    }

    public override string ToString() => Value;
}

