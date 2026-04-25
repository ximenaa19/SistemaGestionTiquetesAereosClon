// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\FlightSeatModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.CabinTypes.Application.UseCases;
using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.FlightSeats.Application.Interfaces;
using GestionAerolineas.src.Modules.FlightSeats.Application.Services;
using GestionAerolineas.src.Modules.FlightSeats.Application.UseCases;
using GestionAerolineas.src.Modules.FlightSeats.Infrastructure.Repository;
using GestionAerolineas.src.Modules.FlightSeats.UI;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.SeatLocationTypes.Application.UseCases;
using GestionAerolineas.src.Modules.SeatLocationTypes.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.FlightSeats;

public static class FlightSeatModule
{
    public static FlightSeatMenu Build(AppDbContext context)
    {
        var repository = new FlightSeatRepository(context);

        var flightRepository = new FlightRepository(context);
        var cabinTypeRepository = new CabinTypeRepository(context);
        var seatLocationTypeRepository = new SeatLocationTypeRepository(context);
        IFlightSeatValidator validator = new FlightSeatValidator(repository, flightRepository, cabinTypeRepository, seatLocationTypeRepository);

        var create = new CreateFlightSeatUseCase(repository, validator);
        var getAll = new GetAllFlightSeatsUseCase(repository);
        var getById = new GetFlightSeatByIdUseCase(repository);
        var getByFlightId = new GetFlightSeatsByFlightIdUseCase(repository);
        var getByFlightAndCode = new GetFlightSeatByFlightAndCodeUseCase(repository);
        var getAvailableByFlightId = new GetAvailableSeatsByFlightIdUseCase(repository);
        var getOccupiedByFlightId = new GetOccupiedSeatsByFlightIdUseCase(repository);
        var update = new UpdateFlightSeatUseCase(repository, validator);
        var delete = new DeleteFlightSeatUseCase(repository);

        var routeRepository = new RouteRepository(context);
        var airportRepository = new AirportRepository(context);
        var airlineRepository = new AirlineRepository(context);

        var getAllFlights = new GetAllFlightsUseCase(flightRepository);
        var getAllRoutes = new GetAllRoutesUseCase(routeRepository);
        var getAllAirports = new GetAllAirportsUseCase(airportRepository);
        var getAllAirlines = new GetAllAirlinesUseCase(airlineRepository);
        var getAllCabinTypes = new GetAllCabinTypeUseCase(cabinTypeRepository);
        var getAllSeatLocationTypes = new GetAllSeatLocationTypesUseCase(seatLocationTypeRepository);

        return new FlightSeatMenu(
            create,
            getAll,
            getById,
            getByFlightId,
            getByFlightAndCode,
            getAvailableByFlightId,
            getOccupiedByFlightId,
            update,
            delete,
            getAllFlights,
            getAllRoutes,
            getAllAirports,
            getAllAirlines,
            getAllCabinTypes,
            getAllSeatLocationTypes);
    }
}

