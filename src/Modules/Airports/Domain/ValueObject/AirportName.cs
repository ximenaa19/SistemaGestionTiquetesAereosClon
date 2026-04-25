// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\Domain\ValueObject\AirportName.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Airports.Domain.ValueObject;

public sealed record AirportName
{
    public string Value { get; }

    private AirportName(string value)
    {
        Value = value;
    }

    public static AirportName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El valor no puede ser nulo ni vacio");

        var trimmed = value.Trim();

        if (trimmed.Length > 150)
            throw new ArgumentException("El valor no puede tener mas de 150 caracteres");

        return new AirportName(trimmed);
    }

    public static string Normalize(string value)
    {
        return (value ?? string.Empty).Trim().ToUpperInvariant();
    }
}
