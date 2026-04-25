// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Domain\ValueObject\UserUsername.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Users.Domain.ValueObject;

public sealed record UserUsername
{
    public string Value { get; }

    private UserUsername(string value)
    {
        Value = value;
    }

    public static UserUsername Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El username no puede estar vacio");

        var trimmed = value.Trim();

        if (trimmed.Length > 50)
            throw new ArgumentException("El username no puede tener mas de 50 caracteres");

        return new UserUsername(trimmed);
    }

    public static string Normalize(string value)
    {
        return (value ?? string.Empty).Trim().ToUpperInvariant();
    }
}
