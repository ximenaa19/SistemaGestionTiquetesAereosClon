// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Domain\Repositories\IUserRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Users.Domain.Aggregate;
using GestionAerolineas.src.Modules.Users.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Users.Domain.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(UserId id);
    Task<User?> GetByUsernameAsync(UserUsername username);
    Task<IEnumerable<User>> GetByRoleIdAsync(UserRoleId roleId);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<bool> ExistsAsync(UserId id);
    Task<bool> ExistsByNormalizedUsernameAsync(string normalizedUsername, int? excludingId = null);
    Task<bool> ExistsByPersonIdAsync(int personId, int? excludingId = null);
}
