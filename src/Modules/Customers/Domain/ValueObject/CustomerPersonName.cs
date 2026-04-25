// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\Domain\ValueObject\CustomerPersonName.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Customers.Domain.ValueObject;

public sealed record CustomerPersonName
{
    public string Value { get; }

    private CustomerPersonName(string value)
    {
        Value = value;
    }

    public static CustomerPersonName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El nombre de la persona no puede estar vacio");

        return new CustomerPersonName(value.Trim());
    }

    public static string Normalize(string value)
    {
        return (value ?? string.Empty).Trim().ToUpperInvariant();
    }
}
