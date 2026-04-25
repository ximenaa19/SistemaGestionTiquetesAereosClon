// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Addresses\Domain\ValueObject\AddressPostalCode.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Addresses.Domain.ValueObject;

public sealed record AddressPostalCode
{
    public string? Value { get; }

    private AddressPostalCode(string? value)
    {
        Value = value;
    }

    public static AddressPostalCode Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new AddressPostalCode((string?)null);

        value = value.Trim();

        if (value.Length > 20)
            throw new ArgumentException("El código postal no puede tener más de 20 caracteres");

        return new AddressPostalCode(value);
    }
}
