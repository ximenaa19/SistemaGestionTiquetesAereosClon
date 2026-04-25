// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Auth\AuthModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Auth.Application.UseCases;
using GestionAerolineas.src.Modules.Auth.UI;
using GestionAerolineas.src.Modules.Sessions.Application.Interfaces;
using GestionAerolineas.src.Modules.Sessions.Application.Services;
using GestionAerolineas.src.Modules.Sessions.Application.UseCases;
using GestionAerolineas.src.Modules.Sessions.Infrastructure.Repository;
using GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Users.Application.Interfaces;
using GestionAerolineas.src.Modules.Users.Application.Services;
using GestionAerolineas.src.Modules.Users.Application.UseCases;
using GestionAerolineas.src.Modules.Users.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Auth;

public static class AuthModule
{
    public static AuthMenu Build(AppDbContext context)
    {
        var userRepository = new UserRepository(context);
        var roleRepository = new SystemRoleRepository(context);
        var sessionRepository = new SessionRepository(context);

        var personRepository = new GestionAerolineas.src.Modules.People.Infrastructure.Repository.PersonRepository(context);
        IUserValidator userValidator = new UserValidator(userRepository, personRepository, roleRepository);

        ISessionValidator sessionValidator = new SessionValidator(sessionRepository, userRepository, roleRepository);

        var createUser = new CreateUserUseCase(userRepository, userValidator);
        var getUserByUsername = new GetUserByUsernameUseCase(userRepository);
        var getAllRoles = new GetAllSystemRolesUseCase(roleRepository);
        var createSession = new CreateSessionUseCase(sessionRepository, sessionValidator);

        var register = new RegisterAuthUserUseCase(createUser);
        var login = new LoginUserUseCase(getUserByUsername, userRepository, createSession);

        return new AuthMenu(register, login, getAllRoles);
    }
}
