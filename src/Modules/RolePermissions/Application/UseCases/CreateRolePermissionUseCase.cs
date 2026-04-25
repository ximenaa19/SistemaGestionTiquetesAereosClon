// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RolePermissions\Application\UseCases\CreateRolePermissionUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RolePermissions.Application.Interfaces;
using GestionAerolineas.src.Modules.RolePermissions.Domain.Aggregate;
using GestionAerolineas.src.Modules.RolePermissions.Domain.Repositories;
using GestionAerolineas.src.Modules.RolePermissions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RolePermissions.Application.UseCases;

public class CreateRolePermissionUseCase
{
    private readonly IRolePermissionRepository _repository;
    private readonly IRolePermissionValidator _validator;

    public CreateRolePermissionUseCase(IRolePermissionRepository repository, IRolePermissionValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int roleId, int permissionId)
    {
        var roleIdVO = SystemRoleId.Create(roleId);
        var permissionIdVO = PermissionId.Create(permissionId);

        await _validator.ValidatePairAsync(roleIdVO, permissionIdVO);

        var entity = RolePermission.CreateNew(roleIdVO, permissionIdVO);

        await _repository.AddAsync(entity);
    }
}

