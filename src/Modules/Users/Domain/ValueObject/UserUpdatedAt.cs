// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Domain\ValueObject\UserUpdatedAt.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Users.Domain.ValueObject;

public sealed record UserUpdatedAt
{
    public DateTime Value { get; }

    private UserUpdatedAt(DateTime value)
    {
        Value = value;
    }

    public static UserUpdatedAt Create(DateTime value)
    {
        return new UserUpdatedAt(value);
    }

    public static UserUpdatedAt Create(DateTime? value)
    {
        return new UserUpdatedAt(value ?? DateTime.Now);
    }
}
