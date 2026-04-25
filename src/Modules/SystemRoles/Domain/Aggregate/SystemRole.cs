// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SystemRoles\Domain\Aggregate\SystemRole.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SystemRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.SystemRoles.Domain.Aggregate;

public class SystemRole
{
    public SystemRoleId Id { get; private set; }
    public SystemRoleName Name { get; private set; }
    public SystemRoleDescription Description { get; private set; }

    private SystemRole(SystemRoleId id, SystemRoleName name, SystemRoleDescription description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public static SystemRole Create(SystemRoleId id, SystemRoleName name, SystemRoleDescription description)
    {
        return new SystemRole(id, name, description);
    }

    public static SystemRole CreateNew(SystemRoleName name, SystemRoleDescription description)
    {
        return new SystemRole(SystemRoleId.CreateEmpty(), name, description);
    }
}
