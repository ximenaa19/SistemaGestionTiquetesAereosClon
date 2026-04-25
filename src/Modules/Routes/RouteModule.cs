// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\RouteModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Routes.Application.Interfaces;
using GestionAerolineas.src.Modules.Routes.Application.Services;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Routes.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Routes;

public static class RouteModule
{
    public static RouteMenu Build(AppDbContext context)
    {
        var repository = new RouteRepository(context);

        var airportRepository = new AirportRepository(context);
        IRouteValidator validator = new RouteValidator(repository, airportRepository);

        var create = new CreateRouteUseCase(repository, validator);
        var getAll = new GetAllRoutesUseCase(repository);
        var getById = new GetRouteByIdUseCase(repository);
        var getByPair = new GetRouteByOriginAndDestinationUseCase(repository);
        var update = new UpdateRouteUseCase(repository, validator);
        var delete = new DeleteRouteUseCase(repository);

        var getAllAirports = new GetAllAirportsUseCase(airportRepository);

        return new RouteMenu(create, getAll, getById, getByPair, update, delete, getAllAirports);
    }

    public static AdminCreateRouteFlow BuildAdminCreateFlow(AppDbContext context)
    {
        var repository = new RouteRepository(context);
        var airportRepository = new AirportRepository(context);

        IRouteValidator validator = new RouteValidator(repository, airportRepository);

        var create = new CreateRouteUseCase(repository, validator);
        var getAllAirports = new GetAllAirportsUseCase(airportRepository);

        return new AdminCreateRouteFlow(create, getAllAirports);
    }

    public static AdminUpdateRouteFlow BuildAdminUpdateFlow(AppDbContext context)
    {
        var repository = new RouteRepository(context);
        var airportRepository = new AirportRepository(context);

        IRouteValidator validator = new RouteValidator(repository, airportRepository);

        var getAll = new GetAllRoutesUseCase(repository);
        var getById = new GetRouteByIdUseCase(repository);
        var update = new UpdateRouteUseCase(repository, validator);
        var getAllAirports = new GetAllAirportsUseCase(airportRepository);

        return new AdminUpdateRouteFlow(getAll, getById, update, getAllAirports);
    }

    public static AdminDeleteRouteFlow BuildAdminDeleteFlow(AppDbContext context)
    {
        var repository = new RouteRepository(context);

        var getAll = new GetAllRoutesUseCase(repository);
        var getById = new GetRouteByIdUseCase(repository);
        var delete = new DeleteRouteUseCase(repository);

        return new AdminDeleteRouteFlow(getAll, getById, delete);
    }
}

