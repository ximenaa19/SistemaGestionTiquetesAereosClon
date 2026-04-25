// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\DocumentTypes\Domain\ValueObject\DocumentTypeCode.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Text.RegularExpressions;

namespace GestionAerolineas.src.Modules.DocumentTypes.Domain.ValueObject;

public sealed record DocumentTypeCode
{
    public string Value { get; }

    private DocumentTypeCode(string value)
    {
        Value = value;
    }

    public static DocumentTypeCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El código no puede estar vacío");

        value = value.Trim().ToUpper();

        if (value.Length > 10)
            throw new ArgumentException("El código no puede tener más de 10 caracteres");

        var regex = new Regex("^[A-Z0-9]+$");

        if (!regex.IsMatch(value))
            throw new ArgumentException("El código solo puede contener letras mayúsculas y números");

        return new DocumentTypeCode(value);
    }

    public override string ToString() => Value;
}