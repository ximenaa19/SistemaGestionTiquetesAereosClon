// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RolePermissions\Application\UseCases\GetAllRolePermissionsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RolePermissions.Domain.Aggregate;
using GestionAerolineas.src.Modules.RolePermissions.Domain.Repositories;

namespace GestionAerolineas.src.Modules.RolePermissions.Application.UseCases;

public class GetAllRolePermissionsUseCase
{
    private readonly IRolePermissionRepository _repository;

    public GetAllRolePermissionsUseCase(IRolePermissionRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<RolePermission>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

