// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Permissions\Application\Services\PermissionValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Permissions.Application.Interfaces;
using GestionAerolineas.src.Modules.Permissions.Domain.Repositories;
using GestionAerolineas.src.Modules.Permissions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Permissions.Application.Services;

public class PermissionValidator : IPermissionValidator
{
    private readonly IPermissionRepository _repository;

    public PermissionValidator(IPermissionRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateNameAsync(PermissionName name, PermissionId? currentId = null)
    {
        var existingByName = await _repository.GetByNameAsync(name);

        if (existingByName is not null && existingByName.Id != currentId)
            throw new Exception("Ya existe un permiso con ese nombre");
    }
}
