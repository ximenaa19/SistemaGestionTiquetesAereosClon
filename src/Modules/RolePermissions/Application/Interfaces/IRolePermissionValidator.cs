// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RolePermissions\Application\Interfaces\IRolePermissionValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RolePermissions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RolePermissions.Application.Interfaces;

public interface IRolePermissionValidator
{
    Task ValidatePairAsync(SystemRoleId roleId, PermissionId permissionId, RolePermissionId? currentId = null);
}

