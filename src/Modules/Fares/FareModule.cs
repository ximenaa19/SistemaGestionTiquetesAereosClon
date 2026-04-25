// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\FareModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.CabinTypes.Application.UseCases;
using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Fares.Application.Interfaces;
using GestionAerolineas.src.Modules.Fares.Application.Services;
using GestionAerolineas.src.Modules.Fares.Application.UseCases;
using GestionAerolineas.src.Modules.Fares.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Fares.UI;
using GestionAerolineas.src.Modules.PassengerTypes.Application.UseCases;
using GestionAerolineas.src.Modules.PassengerTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Seasons.Application.UseCases;
using GestionAerolineas.src.Modules.Seasons.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Fares;

public static class FareModule
{
    public static FareMenu Build(AppDbContext context)
    {
        var repository = new FareRepository(context);

        var routeRepository = new RouteRepository(context);
        var airportRepository = new AirportRepository(context);
        var cabinTypeRepository = new CabinTypeRepository(context);
        var passengerTypeRepository = new PassengerTypeRepository(context);
        var seasonRepository = new SeasonRepository(context);

        IFareValidator validator = new FareValidator(repository, routeRepository, cabinTypeRepository, passengerTypeRepository, seasonRepository);

        var create = new CreateFareUseCase(repository, validator);
        var getAll = new GetAllFaresUseCase(repository);
        var getById = new GetFareByIdUseCase(repository);
        var getByRouteId = new GetFaresByRouteIdUseCase(repository);
        var getByKeys = new GetFareByKeysUseCase(repository);
        var update = new UpdateFareUseCase(repository, validator);
        var delete = new DeleteFareUseCase(repository);

        var getAllRoutes = new GetAllRoutesUseCase(routeRepository);
        var getAllAirports = new GetAllAirportsUseCase(airportRepository);
        var getAllCabinTypes = new GetAllCabinTypeUseCase(cabinTypeRepository);
        var getAllPassengerTypes = new GetAllPassengerTypesUseCase(passengerTypeRepository);
        var getAllSeasons = new GetAllSeasonsUseCase(seasonRepository);

        return new FareMenu(
            create,
            getAll,
            getById,
            getByRouteId,
            getByKeys,
            update,
            delete,
            getAllRoutes,
            getAllAirports,
            getAllCabinTypes,
            getAllPassengerTypes,
            getAllSeasons);
    }
}

