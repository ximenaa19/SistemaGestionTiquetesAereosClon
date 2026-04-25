// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightRoles\Domain\Aggregate\FlightRole.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightRoles.Domain.Aggregate;

public class FlightRole
{
    public FlightRoleId Id { get; private set; }
    public FlightRoleName Name { get; private set; }

    private FlightRole(FlightRoleId id, FlightRoleName name)
    {
        Id = id;
        Name = name;
    }

    public static FlightRole Create(FlightRoleId id, FlightRoleName name)
    {
        return new FlightRole(id, name);
    }

    public static FlightRole CreateNew(FlightRoleName name)
    {
        return new FlightRole(FlightRoleId.CreateEmpty(), name);
    }
}

