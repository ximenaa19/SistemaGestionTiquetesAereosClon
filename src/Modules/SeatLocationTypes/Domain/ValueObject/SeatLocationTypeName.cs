// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SeatLocationTypes\Domain\ValueObject\SeatLocationTypeName.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Text.RegularExpressions;

namespace GestionAerolineas.src.Modules.SeatLocationTypes.Domain.ValueObject;

public sealed record SeatLocationTypeName
{
    public string Value { get; }

    private SeatLocationTypeName(string value)
    {
        Value = value;
    }

    public static SeatLocationTypeName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre no puede estar vacío");

        if (value.Length > 50)
            throw new ArgumentException("Máximo 50 caracteres");

        var trimmed = value.Trim();
        var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");

        if (!regex.IsMatch(trimmed))
            throw new ArgumentException("Solo letras y espacios");

        return new SeatLocationTypeName(trimmed);
    }

    public override string ToString() => Value;
}

