// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Domain\ValueObject\SeasonName.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Text.RegularExpressions;

namespace GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;

public sealed record SeasonName
{
    public string Value { get; }

    private SeasonName(string value)
    {
        Value = value;
    }

    public static SeasonName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre no puede estar vacio");

        if (value.Length > 50)
            throw new ArgumentException("El nombre no puede superar 50 caracteres");

        var regex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$");

        if (!regex.IsMatch(value))
            throw new ArgumentException("El nombre solo puede contener letras y espacios");

        return new SeasonName(value.Trim());
    }

    public override string ToString() => Value;
}
