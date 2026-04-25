// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RolePermissions\Domain\ValueObject\SystemRoleId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.RolePermissions.Domain.ValueObject;

public sealed record SystemRoleId
{
    public int Value { get; }

    private SystemRoleId(int value)
    {
        Value = value;
    }

    public static SystemRoleId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new SystemRoleId(value);
    }
}

