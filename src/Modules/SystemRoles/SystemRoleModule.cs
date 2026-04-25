// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SystemRoles\SystemRoleModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SystemRoles.Application.Interfaces;
using GestionAerolineas.src.Modules.SystemRoles.Application.Services;
using GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Repository;
using GestionAerolineas.src.Modules.SystemRoles.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.SystemRoles;

public static class SystemRoleModule
{
    public static SystemRoleMenu Build(AppDbContext context)
    {
        var repository = new SystemRoleRepository(context);
        ISystemRoleValidator validator = new SystemRoleValidator(repository);

        var create = new CreateSystemRoleUseCase(repository, validator);
        var getAll = new GetAllSystemRolesUseCase(repository);
        var getById = new GetSystemRoleByIdUseCase(repository);
        var getByName = new GetSystemRoleByNameUseCase(repository);
        var update = new UpdateSystemRoleUseCase(repository, validator);
        var delete = new DeleteSystemRoleUseCase(repository);

        return new SystemRoleMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}
