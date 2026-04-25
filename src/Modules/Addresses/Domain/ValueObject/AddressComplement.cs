// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Addresses\Domain\ValueObject\AddressComplement.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Addresses.Domain.ValueObject;

public sealed record AddressComplement
{
    public string? Value { get; }

    private AddressComplement(string? value)
    {
        Value = value;
    }

    public static AddressComplement Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new AddressComplement((string?)null);

        value = value.Trim();

        if (value.Length > 100)
            throw new ArgumentException("El complemento no puede tener más de 100 caracteres");

        return new AddressComplement(value);
    }
}

