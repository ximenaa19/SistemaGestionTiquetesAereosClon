// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Domain\ValueObject\UserPasswordHash.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Users.Domain.ValueObject;

public sealed record UserPasswordHash
{
    public string Value { get; }

    private UserPasswordHash(string value)
    {
        Value = value;
    }

    public static UserPasswordHash Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El password hash no puede estar vacio");

        var trimmed = value.Trim();

        if (trimmed.Length > 255)
            throw new ArgumentException("El password hash no puede tener mas de 255 caracteres");

        return new UserPasswordHash(trimmed);
    }
}
