// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RolePermissions\Domain\ValueObject\RolePermissionId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.RolePermissions.Domain.ValueObject;

public sealed record RolePermissionId
{
    public int Value { get; }

    private RolePermissionId(int value)
    {
        Value = value;
    }

    public static RolePermissionId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new RolePermissionId(value);
    }

    public static RolePermissionId CreateEmpty()
    {
        return new RolePermissionId(0);
    }
}

