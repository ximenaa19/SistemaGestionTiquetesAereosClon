// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Domain\ValueObject\UserIsActive.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Users.Domain.ValueObject;

public sealed record UserIsActive
{
    public bool Value { get; }

    private UserIsActive(bool value)
    {
        Value = value;
    }

    public static UserIsActive Create(bool value)
    {
        return new UserIsActive(value);
    }
}
