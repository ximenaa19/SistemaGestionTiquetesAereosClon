// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffRoles\StaffRoleModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffRoles.Application.Interfaces;
using GestionAerolineas.src.Modules.StaffRoles.Application.Services;
using GestionAerolineas.src.Modules.StaffRoles.Application.UseCases;
using GestionAerolineas.src.Modules.StaffRoles.Infrastructure.Repository;
using GestionAerolineas.src.Modules.StaffRoles.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.StaffRoles;

public static class StaffRoleModule
{
    public static StaffRoleMenu Build(AppDbContext context)
    {
        var repository = new StaffRoleRepository(context);
        IStaffRoleValidator validator = new StaffRoleValidator(repository);

        var create = new CreateStaffRoleUseCase(repository, validator);
        var getAll = new GetAllStaffRolesUseCase(repository);
        var getById = new GetStaffRoleByIdUseCase(repository);
        var getByName = new GetStaffRoleByNameUseCase(repository);
        var update = new UpdateStaffRoleUseCase(repository, validator);
        var delete = new DeleteStaffRoleUseCase(repository);

        return new StaffRoleMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}
