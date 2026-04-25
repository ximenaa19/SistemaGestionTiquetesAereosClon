// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RolePermissions\Application\UseCases\GetRolePermissionByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RolePermissions.Domain.Aggregate;
using GestionAerolineas.src.Modules.RolePermissions.Domain.Repositories;
using GestionAerolineas.src.Modules.RolePermissions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RolePermissions.Application.UseCases;

public class GetRolePermissionByIdUseCase
{
    private readonly IRolePermissionRepository _repository;

    public GetRolePermissionByIdUseCase(IRolePermissionRepository repository)
    {
        _repository = repository;
    }

    public Task<RolePermission?> ExecuteAsync(int id)
    {
        var idVO = RolePermissionId.Create(id);
        return _repository.GetByIdAsync(idVO);
    }
}

