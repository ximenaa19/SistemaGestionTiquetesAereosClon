// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffRoles\Domain\Aggregate\StaffRole.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.StaffRoles.Domain.Aggregate;

public class StaffRole
{
    public StaffRoleId Id { get; private set; }
    public StaffRoleName Name { get; private set; }

    private StaffRole(StaffRoleId id, StaffRoleName name)
    {
        Id = id;
        Name = name;
    }

    public static StaffRole Create(StaffRoleId id, StaffRoleName name)
    {
        return new StaffRole(id, name);
    }

    public static StaffRole CreateNew(StaffRoleName name)
    {
        return new StaffRole(StaffRoleId.CreateEmpty(), name);
    }
}
