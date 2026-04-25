// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\SeasonModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Seasons.Application.Interfaces;
using GestionAerolineas.src.Modules.Seasons.Application.Services;
using GestionAerolineas.src.Modules.Seasons.Application.UseCases;
using GestionAerolineas.src.Modules.Seasons.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Seasons.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Seasons;

public static class SeasonModule
{
    public static SeasonMenu Build(AppDbContext context)
    {
        var repository = new SeasonRepository(context);
        ISeasonValidator validator = new SeasonValidator(repository);

        var create = new CreateSeasonUseCase(repository, validator);
        var getAll = new GetAllSeasonsUseCase(repository);
        var getById = new GetSeasonByIdUseCase(repository);
        var getByName = new GetSeasonByNameUseCase(repository);
        var update = new UpdateSeasonUseCase(repository, validator);
        var delete = new DeleteSeasonUseCase(repository);

        return new SeasonMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}
