// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffRoles\Application\Interfaces\IStaffRoleValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.StaffRoles.Application.Interfaces;

public interface IStaffRoleValidator
{
    Task ValidateNameAsync(StaffRoleName name, StaffRoleId? currentId = null);
}
