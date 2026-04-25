// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Application\UseCases\UpdateSessionUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Sessions.Application.Interfaces;
using GestionAerolineas.src.Modules.Sessions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Sessions.Domain.Repositories;
using GestionAerolineas.src.Modules.Sessions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Sessions.Application.UseCases;

public class UpdateSessionUseCase
{
    private readonly ISessionRepository _repository;
    private readonly ISessionValidator _validator;

    public UpdateSessionUseCase(ISessionRepository repository, ISessionValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, int userId, DateTime startedAt, DateTime? endedAt, string? ipAddress, bool isActive)
    {
        var idVO = SessionId.Create(id);
        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is null)
            throw new Exception("La session no existe");

        var userIdVO = SessionUserId.Create(userId);
        var startedAtVO = SessionStartedAt.Create(startedAt);
        var endedAtVO = SessionEndedAt.Create(endedAt);
        var ipAddressVO = SessionIpAddress.Create(ipAddress);
        var isActiveVO = SessionIsActive.Create(isActive);

        await _validator.ValidateUserExistsAsync(userIdVO);
        await _validator.ValidateLifecycleAsync(startedAtVO, endedAtVO, isActiveVO);

        var updated = Session.Create(idVO, userIdVO, startedAtVO, endedAtVO, ipAddressVO, isActiveVO);
        await _repository.UpdateAsync(updated);
    }
}
