// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RolePermissions\Application\Services\RolePermissionValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RolePermissions.Application.Interfaces;
using GestionAerolineas.src.Modules.RolePermissions.Domain.Repositories;
using GestionAerolineas.src.Modules.RolePermissions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RolePermissions.Application.Services;

public class RolePermissionValidator : IRolePermissionValidator
{
    private readonly IRolePermissionRepository _repository;

    public RolePermissionValidator(IRolePermissionRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidatePairAsync(SystemRoleId roleId, PermissionId permissionId, RolePermissionId? currentId = null)
    {
        var existing = await _repository.GetByPairAsync(roleId, permissionId);

        if (existing is null)
            return;

        if (currentId != null && existing.Id.Value == currentId.Value)
            return;

        throw new Exception("Ya existe ese permiso asignado a ese rol");
    }
}

