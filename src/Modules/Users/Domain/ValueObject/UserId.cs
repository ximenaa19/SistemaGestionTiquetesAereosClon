// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Domain\ValueObject\UserId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Users.Domain.ValueObject;

public sealed record UserId
{
    public int Value { get; }

    private UserId(int value)
    {
        Value = value;
    }

    public static UserId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new UserId(value);
    }

    public static UserId CreateEmpty()
    {
        return new UserId(0);
    }
}
