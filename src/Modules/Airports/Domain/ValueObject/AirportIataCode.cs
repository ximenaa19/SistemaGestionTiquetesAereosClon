// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\Domain\ValueObject\AirportIataCode.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Airports.Domain.ValueObject;

public sealed record AirportIataCode
{
    public string Value { get; }

    private AirportIataCode(string value)
    {
        Value = value;
    }

    public static AirportIataCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El codigo IATA no puede estar vacio");

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length != 3)
            throw new ArgumentException("El codigo IATA debe tener exactamente 3 letras");

        if (!normalized.All(char.IsLetter))
            throw new ArgumentException("El codigo IATA solo puede contener letras");

        return new AirportIataCode(normalized);
    }

    public static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        return value.Trim().ToUpperInvariant();
    }
}
