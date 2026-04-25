// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\FlightModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Application.UseCases;
using GestionAerolineas.src.Modules.Aircraft.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.FlightStates.Application.UseCases;
using GestionAerolineas.src.Modules.FlightStates.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Flights.Application.Interfaces;
using GestionAerolineas.src.Modules.Flights.Application.Services;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Flights.UI;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Flights;

public static class FlightModule
{
    public static FlightMenu Build(AppDbContext context)
    {
        var repository = new FlightRepository(context);

        var airlineRepository = new AirlineRepository(context);
        var routeRepository = new RouteRepository(context);
        var airportRepository = new AirportRepository(context);
        var aircraftRepository = new AircraftRepository(context);
        var flightStateRepository = new FlightStateRepository(context);

        IFlightValidator validator = new FlightValidator(repository, airlineRepository, routeRepository, aircraftRepository, flightStateRepository);

        var create = new CreateFlightUseCase(repository, validator);
        var getAll = new GetAllFlightsUseCase(repository);
        var getById = new GetFlightByIdUseCase(repository);
        var getByCode = new GetFlightByCodeUseCase(repository);
        var getByAirlineId = new GetFlightsByAirlineIdUseCase(repository);
        var getByRouteId = new GetFlightsByRouteIdUseCase(repository);
        var getByDateRange = new GetFlightsByDateRangeUseCase(repository);
        var getByStateId = new GetFlightsByStateIdUseCase(repository);
        var update = new UpdateFlightUseCase(repository, validator);
        var delete = new DeleteFlightUseCase(repository);

        var getAllAirlines = new GetAllAirlinesUseCase(airlineRepository);
        var getAllRoutes = new GetAllRoutesUseCase(routeRepository);
        var getAllAirports = new GetAllAirportsUseCase(airportRepository);
        var getAllAircraft = new GetAllAircraftUseCase(aircraftRepository);
        var getAllFlightStates = new GetAllFlightStatesUseCase(flightStateRepository);

        return new FlightMenu(
            create,
            getAll,
            getById,
            getByCode,
            getByAirlineId,
            getByRouteId,
            getByDateRange,
            getByStateId,
            update,
            delete,
            getAllAirlines,
            getAllRoutes,
            getAllAirports,
            getAllAircraft,
            getAllFlightStates);
    }
}

