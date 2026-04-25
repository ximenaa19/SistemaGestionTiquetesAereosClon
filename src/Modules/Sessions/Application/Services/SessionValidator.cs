// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Application\Services\SessionValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Sessions.Application.Interfaces;
using GestionAerolineas.src.Modules.Sessions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Sessions.Domain.Repositories;
using GestionAerolineas.src.Modules.Sessions.Domain.ValueObject;
using GestionAerolineas.src.Modules.SystemRoles.Domain.ValueObject;
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Users.Domain.ValueObject;
using GestionAerolineas.src.Modules.Users.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.Sessions.Application.Services;

public class SessionValidator : ISessionValidator
{
    private readonly ISessionRepository _sessionRepository;
    private readonly UserRepository _userRepository;
    private readonly SystemRoleRepository _systemRoleRepository;

    public SessionValidator(
        ISessionRepository sessionRepository,
        UserRepository userRepository,
        SystemRoleRepository systemRoleRepository)
    {
        _sessionRepository = sessionRepository;
        _userRepository = userRepository;
        _systemRoleRepository = systemRoleRepository;
    }

    public async Task ValidateUserExistsAsync(SessionUserId userId)
    {
        var exists = await _userRepository.ExistsAsync(UserId.Create(userId.Value));
        if (!exists)
            throw new Exception("El user no existe");
    }

    public Task ValidateLifecycleAsync(SessionStartedAt startedAt, SessionEndedAt endedAt, SessionIsActive isActive)
    {
        if (endedAt.Value.HasValue && endedAt.Value.Value < startedAt.Value)
            throw new Exception("ended_at no puede ser menor que started_at");

        if (endedAt.Value.HasValue && isActive.Value)
            throw new Exception("Una sesion cerrada no puede quedar activa");

        if (!endedAt.Value.HasValue && !isActive.Value)
            throw new Exception("Una sesion sin ended_at no puede quedar inactiva");

        return Task.CompletedTask;
    }

    public async Task ValidateCanForceEndAsync(int actingUserId, int targetUserId)
    {
        if (actingUserId == targetUserId)
            throw new Exception("ForceEndSession solo puede cerrar sesiones activas de otro usuario");

        var actingUser = await _userRepository.GetByIdAsync(UserId.Create(actingUserId));
        if (actingUser is null)
            throw new Exception("El usuario actor no existe");

        var role = await _systemRoleRepository.GetByIdAsync(SystemRoleId.Create(actingUser.RoleId.Value));
        if (role is null)
            throw new Exception("El rol del usuario actor no existe");

        if (!string.Equals(role.Name.Value.Trim(), "Admin", StringComparison.OrdinalIgnoreCase))
            throw new Exception("Solo Admin puede ejecutar ForceEndSession");
    }

    public async Task ValidateSessionExistsAsync(SessionId id)
    {
        var exists = await _sessionRepository.ExistsAsync(id);
        if (!exists)
            throw new Exception("La session no existe");
    }

    public Task ValidateSessionBelongsToOtherUserAsync(Session session, int actingUserId)
    {
        if (session.UserId.Value == actingUserId)
            throw new Exception("No puedes forzar el cierre de una sesion propia");

        return Task.CompletedTask;
    }
}
