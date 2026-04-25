// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Permissions\Application\UseCases\CreatePermissionUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Permissions.Application.Interfaces;
using GestionAerolineas.src.Modules.Permissions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Permissions.Domain.Repositories;
using GestionAerolineas.src.Modules.Permissions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Permissions.Application.UseCases;

public class CreatePermissionUseCase
{
    private readonly IPermissionRepository _repository;
    private readonly IPermissionValidator _validator;

    public CreatePermissionUseCase(
        IPermissionRepository repository,
        IPermissionValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(string name, string? description)
    {
        var nameVO = PermissionName.Create(name);
        var descriptionVO = PermissionDescription.Create(description);

        await _validator.ValidateNameAsync(nameVO);

        var entity = Permission.CreateNew(nameVO, descriptionVO);

        await _repository.AddAsync(entity);
    }
}
