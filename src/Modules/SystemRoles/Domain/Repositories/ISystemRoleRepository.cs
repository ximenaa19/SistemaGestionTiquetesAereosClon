// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SystemRoles\Domain\Repositories\ISystemRoleRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SystemRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.SystemRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.SystemRoles.Domain.Repositories;

public interface ISystemRoleRepository
{
    Task<IEnumerable<SystemRole>> GetAllAsync();
    Task<SystemRole?> GetByIdAsync(SystemRoleId id);
    Task<SystemRole?> GetByNameAsync(SystemRoleName name);
    Task AddAsync(SystemRole systemRole);
    Task UpdateAsync(SystemRole systemRole);
    Task DeleteAsync(SystemRole systemRole);
    Task<bool> ExistsAsync(SystemRoleId id);
}
