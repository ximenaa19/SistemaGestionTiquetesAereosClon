// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Domain\ValueObject\PhoneCountryCode.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Text.RegularExpressions;

namespace GestionAerolineas.src.Modules.PhoneCodes.Domain.ValueObject;

public sealed record PhoneCountryCode
{
    public string Value { get; }

    private PhoneCountryCode(string value)
    {
        Value = value;
    }

    public static PhoneCountryCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El código país no puede estar vacío");

        var normalized = value.Trim();

        if (normalized.Length > 5)
            throw new ArgumentException("Máximo 5 caracteres");

        var regex = new Regex("^\\+[0-9]{1,4}$");

        if (!regex.IsMatch(normalized))
            throw new ArgumentException("Formato inválido. Ej: +57");

        return new PhoneCountryCode(normalized);
    }

    public override string ToString() => Value;
}

