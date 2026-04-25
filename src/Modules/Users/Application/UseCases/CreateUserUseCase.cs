// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Application\UseCases\CreateUserUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Users.Application.Interfaces;
using GestionAerolineas.src.Modules.Users.Application.Services;
using GestionAerolineas.src.Modules.Users.Domain.Aggregate;
using GestionAerolineas.src.Modules.Users.Domain.Repositories;
using GestionAerolineas.src.Modules.Users.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Users.Application.UseCases;

public class CreateUserUseCase
{
    private readonly IUserRepository _repository;
    private readonly IUserValidator _validator;

    public CreateUserUseCase(IUserRepository repository, IUserValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(
        string username,
        string plainPassword,
        int? personId,
        int roleId)
    {
        var usernameVO = UserUsername.Create(username);
        var passwordHashVO = UserPasswordHasher.Hash(plainPassword);
        var personVO = UserPersonId.Create(personId);
        var roleVO = UserRoleId.Create(roleId);
        var isActiveVO = UserIsActive.Create(true);
        var lastAccessVO = UserLastAccess.Create(null);

        await _validator.ValidateUsernameAsync(usernameVO);
        await _validator.ValidatePersonExistsAsync(personVO);
        await _validator.ValidatePersonIsUniqueAsync(personVO);
        await _validator.ValidateRoleExistsAsync(roleVO);

        var entity = User.CreateNew(usernameVO, passwordHashVO, personVO, roleVO, isActiveVO, lastAccessVO);
        await _repository.AddAsync(entity);
    }
}
