// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Permissions\Domain\Repositories\IPermissionRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Permissions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Permissions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Permissions.Domain.Repositories;

public interface IPermissionRepository
{
    Task<IEnumerable<Permission>> GetAllAsync();
    Task<Permission?> GetByIdAsync(PermissionId id);
    Task<Permission?> GetByNameAsync(PermissionName name);
    Task AddAsync(Permission permission);
    Task UpdateAsync(Permission permission);
    Task DeleteAsync(Permission permission);
    Task<bool> ExistsAsync(PermissionId id);
}
