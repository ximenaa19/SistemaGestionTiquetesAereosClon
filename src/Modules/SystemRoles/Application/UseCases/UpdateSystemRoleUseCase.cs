// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SystemRoles\Application\UseCases\UpdateSystemRoleUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SystemRoles.Application.Interfaces;
using GestionAerolineas.src.Modules.SystemRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.SystemRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.SystemRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;

public class UpdateSystemRoleUseCase
{
    private readonly ISystemRoleRepository _repository;
    private readonly ISystemRoleValidator _validator;

    public UpdateSystemRoleUseCase(
        ISystemRoleRepository repository,
        ISystemRoleValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name, string? description)
    {
        var idVO = SystemRoleId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing is null)
            throw new Exception("El rol del sistema no existe");

        var nameVO = SystemRoleName.Create(name);
        var descriptionVO = SystemRoleDescription.Create(description);

        await _validator.ValidateNameAsync(nameVO, idVO);

        var updated = SystemRole.Create(idVO, nameVO, descriptionVO);

        await _repository.UpdateAsync(updated);
    }
}
