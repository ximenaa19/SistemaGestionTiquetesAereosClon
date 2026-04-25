// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Domain\ValueObject\UserPersonId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Users.Domain.ValueObject;

public sealed record UserPersonId
{
    public int? Value { get; }

    private UserPersonId(int? value)
    {
        Value = value;
    }

    public static UserPersonId Create(int? value)
    {
        if (!value.HasValue)
            return new UserPersonId((int?)null);

        if (value.Value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new UserPersonId(value.Value);
    }
}
