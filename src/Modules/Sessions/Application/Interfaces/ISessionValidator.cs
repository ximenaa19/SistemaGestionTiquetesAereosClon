// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Application\Interfaces\ISessionValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Sessions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Sessions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Sessions.Application.Interfaces;

public interface ISessionValidator
{
    Task ValidateUserExistsAsync(SessionUserId userId);
    Task ValidateLifecycleAsync(SessionStartedAt startedAt, SessionEndedAt endedAt, SessionIsActive isActive);
    Task ValidateCanForceEndAsync(int actingUserId, int targetUserId);
    Task ValidateSessionExistsAsync(SessionId id);
    Task ValidateSessionBelongsToOtherUserAsync(Session session, int actingUserId);
}
