// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\Domain\ValueObject\PaymentMethodCommercialName.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Globalization;
using System.Text;

namespace GestionAerolineas.src.Modules.PaymentMethods.Domain.ValueObject;

public sealed record PaymentMethodCommercialName
{
    public string Value { get; }

    private PaymentMethodCommercialName(string value)
    {
        Value = value;
    }

    public static PaymentMethodCommercialName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre comercial no puede estar vacío");

        var trimmed = value.Trim();

        if (trimmed.Length > 50)
            throw new ArgumentException("Máximo 50 caracteres");

        return new PaymentMethodCommercialName(trimmed);
    }

    public static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var trimmed = value.Trim();
        var withoutDiacritics = RemoveDiacritics(trimmed);
        return CollapseSpaces(withoutDiacritics).ToLowerInvariant();
    }

    private static string RemoveDiacritics(string text)
    {
        var normalizedFormD = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(normalizedFormD.Length);

        foreach (var c in normalizedFormD)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                stringBuilder.Append(c);
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    private static string CollapseSpaces(string text)
    {
        var stringBuilder = new StringBuilder(text.Length);
        bool lastWasSpace = false;

        foreach (var c in text)
        {
            var isSpace = char.IsWhiteSpace(c);
            if (isSpace)
            {
                if (!lastWasSpace)
                    stringBuilder.Append(' ');
                lastWasSpace = true;
            }
            else
            {
                stringBuilder.Append(c);
                lastWasSpace = false;
            }
        }

        return stringBuilder.ToString().Trim();
    }

    public override string ToString() => Value;
}

