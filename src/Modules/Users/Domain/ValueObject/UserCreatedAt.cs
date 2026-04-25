// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Domain\ValueObject\UserCreatedAt.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Users.Domain.ValueObject;

public sealed record UserCreatedAt
{
    public DateTime Value { get; }

    private UserCreatedAt(DateTime value)
    {
        Value = value;
    }

    public static UserCreatedAt Create(DateTime value)
    {
        return new UserCreatedAt(value);
    }

    public static UserCreatedAt Create(DateTime? value)
    {
        return new UserCreatedAt(value ?? DateTime.Now);
    }
}
