// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Domain\Aggregate\Session.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Sessions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Sessions.Domain.Aggregate;

public class Session
{
    public SessionId Id { get; private set; }
    public SessionUserId UserId { get; private set; }
    public SessionStartedAt StartedAt { get; private set; }
    public SessionEndedAt EndedAt { get; private set; }
    public SessionIpAddress IpAddress { get; private set; }
    public SessionIsActive IsActive { get; private set; }

    private Session(
        SessionId id,
        SessionUserId userId,
        SessionStartedAt startedAt,
        SessionEndedAt endedAt,
        SessionIpAddress ipAddress,
        SessionIsActive isActive)
    {
        Id = id;
        UserId = userId;
        StartedAt = startedAt;
        EndedAt = endedAt;
        IpAddress = ipAddress;
        IsActive = isActive;
    }

    public static Session Create(
        SessionId id,
        SessionUserId userId,
        SessionStartedAt startedAt,
        SessionEndedAt endedAt,
        SessionIpAddress ipAddress,
        SessionIsActive isActive)
    {
        return new Session(id, userId, startedAt, endedAt, ipAddress, isActive);
    }

    public static Session CreateNew(
        SessionUserId userId,
        SessionStartedAt startedAt,
        SessionEndedAt endedAt,
        SessionIpAddress ipAddress,
        SessionIsActive isActive)
    {
        return new Session(SessionId.CreateEmpty(), userId, startedAt, endedAt, ipAddress, isActive);
    }
}
