// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\SessionModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Sessions.Application.Interfaces;
using GestionAerolineas.src.Modules.Sessions.Application.Services;
using GestionAerolineas.src.Modules.Sessions.Application.UseCases;
using GestionAerolineas.src.Modules.Sessions.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Sessions.UI;
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Users.Application.UseCases;
using GestionAerolineas.src.Modules.Users.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Sessions;

public static class SessionModule
{
    public static SessionMenu Build(AppDbContext context)
    {
        var repository = new SessionRepository(context);
        var userRepository = new UserRepository(context);
        var systemRoleRepository = new SystemRoleRepository(context);

        ISessionValidator validator = new SessionValidator(repository, userRepository, systemRoleRepository);

        var create = new CreateSessionUseCase(repository, validator);
        var getAll = new GetAllSessionsUseCase(repository);
        var getById = new GetSessionByIdUseCase(repository);
        var getByUserId = new GetSessionsByUserIdUseCase(repository);
        var getActive = new GetActiveSessionsUseCase(repository);
        var getInactive = new GetInactiveSessionsUseCase(repository);
        var getByDateRange = new GetSessionsByDateRangeUseCase(repository);
        var update = new UpdateSessionUseCase(repository, validator);
        var forceEnd = new ForceEndSessionUseCase(repository, validator);
        var delete = new DeleteSessionUseCase(repository);
        var getAllUsers = new GetAllUsersUseCase(userRepository);

        return new SessionMenu(
            create,
            getAll,
            getById,
            getByUserId,
            getActive,
            getInactive,
            getByDateRange,
            update,
            forceEnd,
            delete,
            getAllUsers);
    }
}
