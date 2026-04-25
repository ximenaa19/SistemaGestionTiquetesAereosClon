// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Application\UseCases\UpdateUserUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Users.Application.Interfaces;
using GestionAerolineas.src.Modules.Users.Application.Services;
using GestionAerolineas.src.Modules.Users.Domain.Aggregate;
using GestionAerolineas.src.Modules.Users.Domain.Repositories;
using GestionAerolineas.src.Modules.Users.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Users.Application.UseCases;

public class UpdateUserUseCase
{
    private readonly IUserRepository _repository;
    private readonly IUserValidator _validator;

    public UpdateUserUseCase(IUserRepository repository, IUserValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(
        int id,
        string username,
        string? newPlainPassword,
        int? personId,
        int roleId)
    {
        var idVO = UserId.Create(id);
        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is null)
            throw new Exception("El user no existe");

        var usernameVO = UserUsername.Create(username);
        var personVO = UserPersonId.Create(personId);
        var roleVO = UserRoleId.Create(roleId);

        await _validator.ValidateUsernameAsync(usernameVO, idVO);
        await _validator.ValidatePersonExistsAsync(personVO);
        await _validator.ValidatePersonIsUniqueAsync(personVO, idVO);
        await _validator.ValidateRoleExistsAsync(roleVO);

        var passwordHashVO = string.IsNullOrWhiteSpace(newPlainPassword)
            ? existing.PasswordHash
            : UserPasswordHasher.Hash(newPlainPassword);

        var updated = User.Create(
            idVO,
            usernameVO,
            passwordHashVO,
            personVO,
            roleVO,
            existing.IsActive,
            existing.LastAccess,
            existing.CreatedAt,
            UserUpdatedAt.Create(DateTime.Now));

        await _repository.UpdateAsync(updated);
    }
}
