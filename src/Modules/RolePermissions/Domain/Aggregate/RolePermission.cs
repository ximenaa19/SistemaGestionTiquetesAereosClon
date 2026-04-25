// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RolePermissions\Domain\Aggregate\RolePermission.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RolePermissions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RolePermissions.Domain.Aggregate;

public class RolePermission
{
    public RolePermissionId Id { get; private set; }
    public SystemRoleId RoleId { get; private set; }
    public PermissionId PermissionId { get; private set; }

    private RolePermission(RolePermissionId id, SystemRoleId roleId, PermissionId permissionId)
    {
        Id = id;
        RoleId = roleId;
        PermissionId = permissionId;
    }

    public static RolePermission Create(RolePermissionId id, SystemRoleId roleId, PermissionId permissionId)
    {
        return new RolePermission(id, roleId, permissionId);
    }

    public static RolePermission CreateNew(SystemRoleId roleId, PermissionId permissionId)
    {
        return new RolePermission(RolePermissionId.CreateEmpty(), roleId, permissionId);
    }
}

