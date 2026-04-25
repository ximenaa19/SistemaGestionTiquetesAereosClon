// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RolePermissions\RolePermissionModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Permissions.Application.UseCases;
using GestionAerolineas.src.Modules.Permissions.Infrastructure.Repository;
using GestionAerolineas.src.Modules.RolePermissions.Application.Interfaces;
using GestionAerolineas.src.Modules.RolePermissions.Application.Services;
using GestionAerolineas.src.Modules.RolePermissions.Application.UseCases;
using GestionAerolineas.src.Modules.RolePermissions.Infrastructure.Repository;
using GestionAerolineas.src.Modules.RolePermissions.UI;
using GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.RolePermissions;

public static class RolePermissionModule
{
    public static RolePermissionMenu Build(AppDbContext context)
    {
        var repository = new RolePermissionRepository(context);
        IRolePermissionValidator validator = new RolePermissionValidator(repository);

        var create = new CreateRolePermissionUseCase(repository, validator);
        var getAll = new GetAllRolePermissionsUseCase(repository);
        var getById = new GetRolePermissionByIdUseCase(repository);
        var update = new UpdateRolePermissionUseCase(repository, validator);
        var delete = new DeleteRolePermissionUseCase(repository);

        var systemRoleRepository = new SystemRoleRepository(context);
        var permissionRepository = new PermissionRepository(context);

        var getAllSystemRoles = new GetAllSystemRolesUseCase(systemRoleRepository);
        var getAllPermissions = new GetAllPermissionsUseCase(permissionRepository);

        return new RolePermissionMenu(
            create,
            getAll,
            getById,
            update,
            delete,
            getAllSystemRoles,
            getAllPermissions
        );
    }
}

