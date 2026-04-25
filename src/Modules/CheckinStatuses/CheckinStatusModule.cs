// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CheckinStatuses\CheckinStatusModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CheckinStatuses.Application.Interfaces;
using GestionAerolineas.src.Modules.CheckinStatuses.Application.Services;
using GestionAerolineas.src.Modules.CheckinStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.CheckinStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.CheckinStatuses.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.CheckinStatuses;

public static class CheckinStatusModule
{
    public static CheckinStatusMenu Build(AppDbContext context)
    {
        var repository = new CheckinStatusRepository(context);
        ICheckinStatusValidator validator = new CheckinStatusValidator(repository);

        var create = new CreateCheckinStatusUseCase(repository, validator);
        var getAll = new GetAllCheckinStatusesUseCase(repository);
        var getById = new GetCheckinStatusByIdUseCase(repository);
        var getByName = new GetCheckinStatusByNameUseCase(repository);
        var update = new UpdateCheckinStatusUseCase(repository, validator);
        var delete = new DeleteCheckinStatusUseCase(repository);

        return new CheckinStatusMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}
