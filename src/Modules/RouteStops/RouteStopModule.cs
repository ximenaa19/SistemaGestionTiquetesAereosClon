// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\RouteStopModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.RouteStops.Application.Interfaces;
using GestionAerolineas.src.Modules.RouteStops.Application.Services;
using GestionAerolineas.src.Modules.RouteStops.Application.UseCases;
using GestionAerolineas.src.Modules.RouteStops.Infrastructure.Repository;
using GestionAerolineas.src.Modules.RouteStops.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.RouteStops;

public static class RouteStopModule
{
    public static RouteStopMenu Build(AppDbContext context)
    {
        var repository = new RouteStopRepository(context);

        var routeRepository = new RouteRepository(context);
        var airportRepository = new AirportRepository(context);
        IRouteStopValidator validator = new RouteStopValidator(repository, routeRepository, airportRepository);

        var create = new CreateRouteStopUseCase(repository, validator);
        var getAll = new GetAllRouteStopsUseCase(repository);
        var getById = new GetRouteStopByIdUseCase(repository);
        var getByRouteId = new GetRouteStopsByRouteIdUseCase(repository);
        var getByRouteAndOrder = new GetRouteStopByRouteAndOrderUseCase(repository);
        var update = new UpdateRouteStopUseCase(repository, validator);
        var delete = new DeleteRouteStopUseCase(repository);

        var getAllRoutes = new GetAllRoutesUseCase(routeRepository);
        var getAllAirports = new GetAllAirportsUseCase(airportRepository);

        return new RouteStopMenu(create, getAll, getById, getByRouteId, getByRouteAndOrder, update, delete, getAllRoutes, getAllAirports);
    }
}
