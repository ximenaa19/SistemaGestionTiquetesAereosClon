// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Permissions\PermissionModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Permissions.Application.Interfaces;
using GestionAerolineas.src.Modules.Permissions.Application.Services;
using GestionAerolineas.src.Modules.Permissions.Application.UseCases;
using GestionAerolineas.src.Modules.Permissions.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Permissions.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Permissions;

public static class PermissionModule
{
    public static PermissionMenu Build(AppDbContext context)
    {
        var repository = new PermissionRepository(context);
        IPermissionValidator validator = new PermissionValidator(repository);

        var create = new CreatePermissionUseCase(repository, validator);
        var getAll = new GetAllPermissionsUseCase(repository);
        var getById = new GetPermissionByIdUseCase(repository);
        var getByName = new GetPermissionByNameUseCase(repository);
        var update = new UpdatePermissionUseCase(repository, validator);
        var delete = new DeletePermissionUseCase(repository);

        return new PermissionMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}
