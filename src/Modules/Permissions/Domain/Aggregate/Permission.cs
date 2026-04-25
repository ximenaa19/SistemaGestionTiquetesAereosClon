// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Permissions\Domain\Aggregate\Permission.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Permissions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Permissions.Domain.Aggregate;

public class Permission
{
    public PermissionId Id { get; private set; }
    public PermissionName Name { get; private set; }
    public PermissionDescription Description { get; private set; }

    private Permission(PermissionId id, PermissionName name, PermissionDescription description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public static Permission Create(PermissionId id, PermissionName name, PermissionDescription description)
    {
        return new Permission(id, name, description);
    }

    public static Permission CreateNew(PermissionName name, PermissionDescription description)
    {
        return new Permission(PermissionId.CreateEmpty(), name, description);
    }
}
