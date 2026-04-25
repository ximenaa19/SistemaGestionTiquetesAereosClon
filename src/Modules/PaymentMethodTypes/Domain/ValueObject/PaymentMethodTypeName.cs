// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethodTypes\Domain\ValueObject\PaymentMethodTypeName.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Text.RegularExpressions;

namespace GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.ValueObject;

public sealed record PaymentMethodTypeName
{
    public string Value { get; }

    private PaymentMethodTypeName(string value)
    {
        Value = value;
    }

    public static PaymentMethodTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre no puede estar vacio");

        if (value.Length > 50)
            throw new ArgumentException("Maximo 50 caracteres");

        var trimmed = value.Trim();
        var regex = new Regex("^[a-zA-ZÃ¡Ã©Ã­Ã³ÃºÃÃ‰ÃÃ“ÃšÃ±Ã‘ ]+$");

        if (!regex.IsMatch(trimmed))
            throw new ArgumentException("Solo letras y espacios");

        return new PaymentMethodTypeName(trimmed);
    }

    public override string ToString() => Value;
}
