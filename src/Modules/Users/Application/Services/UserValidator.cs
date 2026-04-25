// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Application\Services\UserValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Domain.ValueObject;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.SystemRoles.Domain.ValueObject;
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Users.Application.Interfaces;
using GestionAerolineas.src.Modules.Users.Domain.Aggregate;
using GestionAerolineas.src.Modules.Users.Domain.Repositories;
using GestionAerolineas.src.Modules.Users.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Users.Application.Services;

public class UserValidator : IUserValidator
{
    private readonly IUserRepository _repository;
    private readonly PersonRepository _personRepository;
    private readonly SystemRoleRepository _systemRoleRepository;

    public UserValidator(
        IUserRepository repository,
        PersonRepository personRepository,
        SystemRoleRepository systemRoleRepository)
    {
        _repository = repository;
        _personRepository = personRepository;
        _systemRoleRepository = systemRoleRepository;
    }

    public async Task ValidateUsernameAsync(UserUsername username, UserId? currentId = null)
    {
        var exists = await _repository.ExistsByNormalizedUsernameAsync(
            UserUsername.Normalize(username.Value),
            currentId?.Value);

        if (exists)
            throw new Exception("Ya existe un user con ese username");
    }

    public async Task ValidatePersonExistsAsync(UserPersonId personId)
    {
        if (!personId.Value.HasValue)
            return;

        var exists = await _personRepository.ExistsAsync(PersonId.Create(personId.Value.Value));
        if (!exists)
            throw new Exception("La persona no existe");
    }

    public async Task ValidatePersonIsUniqueAsync(UserPersonId personId, UserId? currentId = null)
    {
        if (!personId.Value.HasValue)
            return;

        var exists = await _repository.ExistsByPersonIdAsync(personId.Value.Value, currentId?.Value);
        if (exists)
            throw new Exception("Ya existe un user para esta persona");
    }

    public async Task ValidateRoleExistsAsync(UserRoleId roleId)
    {
        var exists = await _systemRoleRepository.ExistsAsync(SystemRoleId.Create(roleId.Value));
        if (!exists)
            throw new Exception("El rol no existe");
    }

    public Task ValidateCanDeactivateAsync(User existingUser, UserIsActive newIsActive, string? actingUsername)
    {
        if (!existingUser.IsActive.Value || newIsActive.Value)
            return Task.CompletedTask;

        if (string.IsNullOrWhiteSpace(actingUsername))
            throw new Exception("Debes indicar el username actual para desactivar un user");

        var normalizedActing = UserUsername.Normalize(actingUsername);
        var normalizedTarget = UserUsername.Normalize(existingUser.Username.Value);

        if (normalizedActing == normalizedTarget)
            throw new Exception("No puedes desactivarte a ti mismo");

        return Task.CompletedTask;
    }
}
