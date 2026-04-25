// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Application\UseCases\SetUserActiveStatusUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Users.Application.Interfaces;
using GestionAerolineas.src.Modules.Users.Domain.Aggregate;
using GestionAerolineas.src.Modules.Users.Domain.Repositories;
using GestionAerolineas.src.Modules.Users.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Users.Application.UseCases;

public class SetUserActiveStatusUseCase
{
    private readonly IUserRepository _repository;
    private readonly IUserValidator _validator;

    public SetUserActiveStatusUseCase(IUserRepository repository, IUserValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, bool isActive, string? actingUsername)
    {
        var idVO = UserId.Create(id);
        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is null)
            throw new Exception("El user no existe");

        var isActiveVO = UserIsActive.Create(isActive);
        await _validator.ValidateCanDeactivateAsync(existing, isActiveVO, actingUsername);

        var updated = User.Create(
            existing.Id,
            existing.Username,
            existing.PasswordHash,
            existing.PersonId,
            existing.RoleId,
            isActiveVO,
            existing.LastAccess,
            existing.CreatedAt,
            UserUpdatedAt.Create(DateTime.Now));

        await _repository.UpdateAsync(updated);
    }
}
