// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\AircraftModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Application.Interfaces;
using GestionAerolineas.src.Modules.Aircraft.Application.Services;
using GestionAerolineas.src.Modules.Aircraft.Application.UseCases;
using GestionAerolineas.src.Modules.Aircraft.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Aircraft.UI;
using GestionAerolineas.src.Modules.AircraftModels.Application.UseCases;
using GestionAerolineas.src.Modules.AircraftModels.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Aircraft;

public static class AircraftModule
{
    public static AircraftMenu Build(AppDbContext context)
    {
        var repository = new AircraftRepository(context);

        var aircraftModelRepository = new AircraftModelRepository(context);
        var airlineRepository = new AirlineRepository(context);
        IAircraftValidator validator = new AircraftValidator(repository, aircraftModelRepository, airlineRepository);

        var create = new CreateAircraftUseCase(repository, validator);
        var getAll = new GetAllAircraftUseCase(repository);
        var getById = new GetAircraftByIdUseCase(repository);
        var getByRegistration = new GetAircraftByRegistrationUseCase(repository);
        var update = new UpdateAircraftUseCase(repository, validator);
        var delete = new DeleteAircraftUseCase(repository);

        var getAllModels = new GetAllAircraftModelsUseCase(aircraftModelRepository);
        var getAllAirlines = new GetAllAirlinesUseCase(airlineRepository);

        return new AircraftMenu(
            create,
            getAll,
            getById,
            getByRegistration,
            update,
            delete,
            getAllModels,
            getAllAirlines
        );
    }

    public static AdminCreateAircraftFlow BuildAdminCreateFlow(AppDbContext context)
    {
        var repository = new AircraftRepository(context);
        var aircraftModelRepository = new AircraftModelRepository(context);
        var airlineRepository = new AirlineRepository(context);

        IAircraftValidator validator = new AircraftValidator(repository, aircraftModelRepository, airlineRepository);

        var create = new CreateAircraftUseCase(repository, validator);
        var getAllModels = new GetAllAircraftModelsUseCase(aircraftModelRepository);
        var getAllAirlines = new GetAllAirlinesUseCase(airlineRepository);

        return new AdminCreateAircraftFlow(create, getAllModels, getAllAirlines);
    }

    public static AdminUpdateAircraftFlow BuildAdminUpdateFlow(AppDbContext context)
    {
        var repository = new AircraftRepository(context);
        var aircraftModelRepository = new AircraftModelRepository(context);
        var airlineRepository = new AirlineRepository(context);

        IAircraftValidator validator = new AircraftValidator(repository, aircraftModelRepository, airlineRepository);

        var getAll = new GetAllAircraftUseCase(repository);
        var getById = new GetAircraftByIdUseCase(repository);
        var update = new UpdateAircraftUseCase(repository, validator);
        var getAllModels = new GetAllAircraftModelsUseCase(aircraftModelRepository);
        var getAllAirlines = new GetAllAirlinesUseCase(airlineRepository);

        return new AdminUpdateAircraftFlow(getAll, getById, update, getAllModels, getAllAirlines);
    }

    public static AdminDeleteAircraftFlow BuildAdminDeleteFlow(AppDbContext context)
    {
        var repository = new AircraftRepository(context);

        var getAll = new GetAllAircraftUseCase(repository);
        var getById = new GetAircraftByIdUseCase(repository);
        var delete = new DeleteAircraftUseCase(repository);

        return new AdminDeleteAircraftFlow(getAll, getById, delete);
    }
}

