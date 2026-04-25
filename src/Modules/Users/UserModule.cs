// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\UserModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Users.Application.Interfaces;
using GestionAerolineas.src.Modules.Users.Application.Services;
using GestionAerolineas.src.Modules.Users.Application.UseCases;
using GestionAerolineas.src.Modules.Users.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Users.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Users;

public static class UserModule
{
    public static UserMenu Build(AppDbContext context)
    {
        var repository = new UserRepository(context);

        var personRepository = new PersonRepository(context);
        var systemRoleRepository = new SystemRoleRepository(context);

        IUserValidator validator = new UserValidator(repository, personRepository, systemRoleRepository);

        var create = new CreateUserUseCase(repository, validator);
        var getAll = new GetAllUsersUseCase(repository);
        var getById = new GetUserByIdUseCase(repository);
        var getByUsername = new GetUserByUsernameUseCase(repository);
        var getByRoleId = new GetUsersByRoleIdUseCase(repository);
        var update = new UpdateUserUseCase(repository, validator);
        var setActive = new SetUserActiveStatusUseCase(repository, validator);
        var deleteHard = new DeleteUserHardUseCase(repository);

        var getAllPeople = new GetAllPeopleUseCase(personRepository);
        var getAllRoles = new GetAllSystemRolesUseCase(systemRoleRepository);

        return new UserMenu(
            create,
            getAll,
            getById,
            getByUsername,
            getByRoleId,
            update,
            setActive,
            deleteHard,
            getAllPeople,
            getAllRoles);
    }

    public static AdminCreateUserFlow BuildAdminCreateFlow(AppDbContext context)
    {
        var userRepository = new UserRepository(context);
        var personRepository = new PersonRepository(context);
        var systemRoleRepository = new SystemRoleRepository(context);

        IUserValidator validator = new UserValidator(userRepository, personRepository, systemRoleRepository);

        var create = new CreateUserUseCase(userRepository, validator);
        var getAllPeople = new GetAllPeopleUseCase(personRepository);
        var getAllRoles = new GetAllSystemRolesUseCase(systemRoleRepository);

        return new AdminCreateUserFlow(create, getAllPeople, getAllRoles);
    }
}
