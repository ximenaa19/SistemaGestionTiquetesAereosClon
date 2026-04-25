// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Application\Interfaces\IStaffAvailabilityValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffAvailability.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.StaffAvailability.Application.Interfaces;

public interface IStaffAvailabilityValidator
{
    Task ValidateStaffExistsAndActiveAsync(StaffAvailabilityStaffId staffId);
    Task ValidateStatusExistsAsync(StaffAvailabilityStatusId statusId);
    void ValidateDateRange(StaffAvailabilityStartDateTime start, StaffAvailabilityEndDateTime end);
    Task ValidateNoOverlapAsync(StaffAvailabilityStaffId staffId, StaffAvailabilityStartDateTime start, StaffAvailabilityEndDateTime end, StaffAvailabilityId? currentId = null);
}

