// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\AircraftModelModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftManufacturers.Application.UseCases;
using GestionAerolineas.src.Modules.AircraftManufacturers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.AircraftModels.Application.Interfaces;
using GestionAerolineas.src.Modules.AircraftModels.Application.Services;
using GestionAerolineas.src.Modules.AircraftModels.Application.UseCases;
using GestionAerolineas.src.Modules.AircraftModels.Infrastructure.Repository;
using GestionAerolineas.src.Modules.AircraftModels.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.AircraftModels;

public static class AircraftModelModule
{
    public static AircraftModelMenu Build(AppDbContext context)
    {
        var repository = new AircraftModelRepository(context);

        var manufacturerRepository = new AircraftManufacturerRepository(context);
        IAircraftModelValidator validator = new AircraftModelValidator(repository, manufacturerRepository);

        var create = new CreateAircraftModelUseCase(repository, validator);
        var getAll = new GetAllAircraftModelsUseCase(repository);
        var getById = new GetAircraftModelByIdUseCase(repository);
        var getByName = new GetAircraftModelByNameUseCase(repository);
        var update = new UpdateAircraftModelUseCase(repository, validator);
        var delete = new DeleteAircraftModelUseCase(repository);

        var getAllManufacturers = new GetAllAircraftManufacturersUseCase(manufacturerRepository);

        return new AircraftModelMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete,
            getAllManufacturers
        );
    }
}

