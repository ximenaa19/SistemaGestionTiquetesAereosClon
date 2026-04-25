// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Domain\ValueObject\SeasonDescription.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;

public sealed record SeasonDescription
{
    public string? Value { get; }

    private SeasonDescription(string? value)
    {
        Value = value;
    }

    public static SeasonDescription Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new SeasonDescription((string?)null);

        var trimmed = value.Trim();

        if (trimmed.Length > 150)
            throw new ArgumentException("La descripcion no puede superar 150 caracteres");

        return new SeasonDescription(trimmed);
    }

    public override string ToString() => Value ?? string.Empty;
}
