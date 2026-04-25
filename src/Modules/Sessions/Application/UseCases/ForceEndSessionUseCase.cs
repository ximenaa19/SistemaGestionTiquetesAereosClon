// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Application\UseCases\ForceEndSessionUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Sessions.Application.Interfaces;
using GestionAerolineas.src.Modules.Sessions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Sessions.Domain.Repositories;
using GestionAerolineas.src.Modules.Sessions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Sessions.Application.UseCases;

public class ForceEndSessionUseCase
{
    private readonly ISessionRepository _repository;
    private readonly ISessionValidator _validator;

    public ForceEndSessionUseCase(ISessionRepository repository, ISessionValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<int> ExecuteAsync(int actingUserId, int targetUserId)
    {
        await _validator.ValidateCanForceEndAsync(actingUserId, targetUserId);

        var sessions = (await _repository.GetActiveByUserIdAsync(SessionUserId.Create(targetUserId))).ToList();
        foreach (var session in sessions)
        {
            await _validator.ValidateSessionBelongsToOtherUserAsync(session, actingUserId);

            var updated = Session.Create(
                session.Id,
                session.UserId,
                session.StartedAt,
                SessionEndedAt.Create(DateTime.Now),
                session.IpAddress,
                SessionIsActive.Create(false));

            await _repository.UpdateAsync(updated);
        }

        return sessions.Count;
    }
}
