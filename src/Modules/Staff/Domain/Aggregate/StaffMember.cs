// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Domain\Aggregate\StaffMember.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Staff.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Staff.Domain.Aggregate;

public class StaffMember
{
    public StaffId Id { get; private set; }
    public StaffPersonId PersonId { get; private set; }
    public StaffRoleId RoleId { get; private set; }
    public StaffAirlineId AirlineId { get; private set; }
    public StaffAirportId AirportId { get; private set; }
    public StaffHireDate HireDate { get; private set; }
    public StaffIsActive IsActive { get; private set; }

    private StaffMember(
        StaffId id,
        StaffPersonId personId,
        StaffRoleId roleId,
        StaffAirlineId airlineId,
        StaffAirportId airportId,
        StaffHireDate hireDate,
        StaffIsActive isActive)
    {
        Id = id;
        PersonId = personId;
        RoleId = roleId;
        AirlineId = airlineId;
        AirportId = airportId;
        HireDate = hireDate;
        IsActive = isActive;
    }

    public static StaffMember Create(
        StaffId id,
        StaffPersonId personId,
        StaffRoleId roleId,
        StaffAirlineId airlineId,
        StaffAirportId airportId,
        StaffHireDate hireDate,
        StaffIsActive isActive)
    {
        return new StaffMember(id, personId, roleId, airlineId, airportId, hireDate, isActive);
    }

    public static StaffMember CreateNew(
        StaffPersonId personId,
        StaffRoleId roleId,
        StaffAirlineId airlineId,
        StaffAirportId airportId,
        StaffHireDate hireDate,
        StaffIsActive isActive)
    {
        return new StaffMember(StaffId.CreateEmpty(), personId, roleId, airlineId, airportId, hireDate, isActive);
    }
}

