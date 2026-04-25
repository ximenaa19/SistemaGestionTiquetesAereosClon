// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffRoles\Domain\Repositories\IStaffRoleRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.StaffRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.StaffRoles.Domain.Repositories;

public interface IStaffRoleRepository
{
    Task<IEnumerable<StaffRole>> GetAllAsync();
    Task<StaffRole?> GetByIdAsync(StaffRoleId id);
    Task<StaffRole?> GetByNameAsync(StaffRoleName name);
    Task AddAsync(StaffRole staffRole);
    Task UpdateAsync(StaffRole staffRole);
    Task DeleteAsync(StaffRole staffRole);
    Task<bool> ExistsAsync(StaffRoleId id);
}
