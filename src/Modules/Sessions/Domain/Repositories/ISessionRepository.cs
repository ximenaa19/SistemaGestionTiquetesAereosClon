// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Domain\Repositories\ISessionRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Sessions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Sessions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Sessions.Domain.Repositories;

public interface ISessionRepository
{
    Task<IEnumerable<Session>> GetAllAsync();
    Task<Session?> GetByIdAsync(SessionId id);
    Task<IEnumerable<Session>> GetByUserIdAsync(SessionUserId userId);
    Task<IEnumerable<Session>> GetByIsActiveAsync(SessionIsActive isActive);
    Task<IEnumerable<Session>> GetByDateRangeAsync(DateTime from, DateTime to);
    Task<IEnumerable<Session>> GetActiveByUserIdAsync(SessionUserId userId);
    Task AddAsync(Session session);
    Task UpdateAsync(Session session);
    Task DeleteAsync(Session session);
    Task<bool> ExistsAsync(SessionId id);
}
