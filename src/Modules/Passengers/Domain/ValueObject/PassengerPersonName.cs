// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\Domain\ValueObject\PassengerPersonName.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Passengers.Domain.ValueObject;

public sealed record PassengerPersonName
{
    public string Value { get; }

    private PassengerPersonName(string value)
    {
        Value = value;
    }

    public static PassengerPersonName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre de la persona no puede estar vacio");

        return new PassengerPersonName(value.Trim());
    }

    public static string Normalize(string value)
    {
        return (value ?? string.Empty).Trim().ToUpperInvariant();
    }
}
