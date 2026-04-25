// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SystemRoles\Application\Interfaces\ISystemRoleValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SystemRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.SystemRoles.Application.Interfaces;

public interface ISystemRoleValidator
{
    Task ValidateNameAsync(SystemRoleName name, SystemRoleId? currentId = null);
}
