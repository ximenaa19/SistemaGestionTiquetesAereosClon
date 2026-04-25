// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Application\Interfaces\IStaffValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Staff.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Staff.Application.Interfaces;

public interface IStaffValidator
{
    Task ValidatePersonExistsAsync(StaffPersonId personId);
    Task ValidateRoleExistsAsync(StaffRoleId roleId);
    Task ValidateOptionalAirlineExistsAsync(StaffAirlineId airlineId);
    Task ValidateOptionalAirportExistsAsync(StaffAirportId airportId);
    Task ValidateUniquePersonAsync(StaffPersonId personId, StaffId? currentId = null);
    void ValidateHasAirlineOrAirport(StaffAirlineId airlineId, StaffAirportId airportId);
}

