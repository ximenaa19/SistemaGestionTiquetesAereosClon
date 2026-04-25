// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RoadTypes\RoadTypeModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RoadTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.RoadTypes.Application.Services;
using GestionAerolineas.src.Modules.RoadTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.RoadTypes.Application.UseCases;
using GestionAerolineas.src.Modules.RoadTypes.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.RoadTypes;

public static class RoadTypeModule
{
    public static RoadTypeMenu Build(AppDbContext context)
    {
        var repository = new RoadTypeRepository(context);
        IRoadTypeValidator validator = new RoadTypeValidator(repository);

        var create = new CreateRoadTypeUseCase(repository, validator);
        var getAll = new GetAllRoadTypesUseCase(repository);
        var getById = new GetRoadTypeByIdUseCase(repository);
        var getByName = new GetRoadTypeByNameUseCase(repository);
        var update = new UpdateRoadTypeUseCase(repository, validator);
        var delete = new DeleteRoadTypeUseCase(repository);

        return new RoadTypeMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}