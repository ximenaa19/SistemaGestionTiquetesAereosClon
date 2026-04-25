// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\UI\RouteStopMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.RouteStops.Application.UseCases;
using GestionAerolineas.src.Modules.RouteStops.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.RouteStops.UI;

public class RouteStopMenu
{
    private readonly CreateRouteStopUseCase _create;
    private readonly GetAllRouteStopsUseCase _getAll;
    private readonly GetRouteStopByIdUseCase _getById;
    private readonly GetRouteStopsByRouteIdUseCase _getByRouteId;
    private readonly GetRouteStopByRouteAndOrderUseCase _getByRouteAndOrder;
    private readonly UpdateRouteStopUseCase _update;
    private readonly DeleteRouteStopUseCase _delete;

    private readonly GetAllRoutesUseCase _getAllRoutes;
    private readonly GetAllAirportsUseCase _getAllAirports;

    public RouteStopMenu(
        CreateRouteStopUseCase create,
        GetAllRouteStopsUseCase getAll,
        GetRouteStopByIdUseCase getById,
        GetRouteStopsByRouteIdUseCase getByRouteId,
        GetRouteStopByRouteAndOrderUseCase getByRouteAndOrder,
        UpdateRouteStopUseCase update,
        DeleteRouteStopUseCase delete,
        GetAllRoutesUseCase getAllRoutes,
        GetAllAirportsUseCase getAllAirports)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByRouteId = getByRouteId;
        _getByRouteAndOrder = getByRouteAndOrder;
        _update = update;
        _delete = delete;
        _getAllRoutes = getAllRoutes;
        _getAllAirports = getAllAirports;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear route stop",
            "Listar route stops",
            "Get route stop by ID",
            "Get route stops by route_id",
            "Get route stop by route_id + order",
            "Actualizar route stop",
            "Eliminar route stop",
            "Salir"
        });

        while (true)
        {
            int option = menu.Show();

            try
            {
                switch (option)
                {
                    case 0:
                        await PrintRoutesAsync();
                        await PrintAirportsAsync();

                        Console.Write("\nIngrese ruta_id: ");
                        int routeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aeropuerto_escala_id: ");
                        int stopAirportId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese orden: ");
                        int order = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese duracion_escala_min: ");
                        int durationMinutes = int.Parse(Console.ReadLine()!);

                        await _create.ExecuteAsync(routeId, stopAirportId, order, durationMinutes);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var routeMap = await GetRouteDisplayMapAsync();
                        var airportMap = await GetAirportDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, routeMap, airportMap));
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);

                        var byId = await _getById.ExecuteAsync(searchId);
                        if (byId is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var routeMapById = await GetRouteDisplayMapAsync();
                        var airportMapById = await GetAirportDisplayMapAsync();
                        Console.WriteLine(Format(byId, routeMapById, airportMapById));
                        break;

                    case 3:
                        await PrintRoutesAsync();

                        Console.Write("\nIngrese ruta_id: ");
                        int searchRouteId = int.Parse(Console.ReadLine()!);

                        var byRouteId = await _getByRouteId.ExecuteAsync(searchRouteId);
                        var routeMapByRoute = await GetRouteDisplayMapAsync();
                        var airportMapByRoute = await GetAirportDisplayMapAsync();

                        foreach (var item in byRouteId)
                            Console.WriteLine(Format(item, routeMapByRoute, airportMapByRoute));
                        break;

                    case 4:
                        await PrintRoutesAsync();

                        Console.Write("\nIngrese ruta_id: ");
                        int searchRouteId2 = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese orden: ");
                        int searchOrder = int.Parse(Console.ReadLine()!);

                        var byPair = await _getByRouteAndOrder.ExecuteAsync(searchRouteId2, searchOrder);
                        if (byPair is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var routeMapByPair = await GetRouteDisplayMapAsync();
                        var airportMapByPair = await GetAirportDisplayMapAsync();
                        Console.WriteLine(Format(byPair, routeMapByPair, airportMapByPair));
                        break;

                    case 5:
                        await PrintRoutesAsync();
                        await PrintAirportsAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese ruta_id: ");
                        int newRouteId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aeropuerto_escala_id: ");
                        int newStopAirportId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese orden: ");
                        int newOrder = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese duracion_escala_min: ");
                        int newDurationMinutes = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newRouteId, newStopAirportId, newOrder, newDurationMinutes);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 6:
                        Console.Write("Ingrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("âœ” Eliminado");
                        break;

                    case 7:
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"âŒ Error: {ex.GetBaseException().Message}");
            }

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private async Task PrintRoutesAsync()
    {
        Console.WriteLine("Rutas disponibles:");
        var routes = (await _getAllRoutes.ExecuteAsync()).ToList();
        var airportMap = await GetAirportDisplayMapAsync();

        foreach (var route in routes.Take(30))
        {
            var originDisplay = GetDisplay(airportMap, route.OriginAirportId.Value);
            var destinationDisplay = GetDisplay(airportMap, route.DestinationAirportId.Value);
            Console.WriteLine($"{route.Id.Value} - origin={originDisplay} - destination={destinationDisplay}");
        }

        if (routes.Count > 30)
            Console.WriteLine("(Mostrando solo las primeras 30)");
    }

    private async Task PrintAirportsAsync()
    {
        Console.WriteLine("\nAeropuertos disponibles:");
        var airports = (await _getAllAirports.ExecuteAsync()).ToList();

        foreach (var airport in airports.Take(30))
            Console.WriteLine($"{airport.Id.Value} - {airport.Name.Value} - iata={airport.IataCode.Value} - city_id={airport.CityId.Value}");

        if (airports.Count > 30)
            Console.WriteLine("(Mostrando solo los primeros 30)");
    }

    private async Task<Dictionary<int, string>> GetAirportDisplayMapAsync()
    {
        var airports = await _getAllAirports.ExecuteAsync();
        return airports.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
    }

    private async Task<Dictionary<int, string>> GetRouteDisplayMapAsync()
    {
        var routes = await _getAllRoutes.ExecuteAsync();
        var airportMap = await GetAirportDisplayMapAsync();

        return routes.ToDictionary(
            r => r.Id.Value,
            r =>
            {
                var origin = GetDisplay(airportMap, r.OriginAirportId.Value);
                var destination = GetDisplay(airportMap, r.DestinationAirportId.Value);
                return $"{origin} -> {destination}";
            });
    }

    private static string Format(RouteStop item, Dictionary<int, string> routeMap, Dictionary<int, string> airportMap)
    {
        string routeDisplay = GetDisplay(routeMap, item.RouteId.Value);
        string stopAirportDisplay = GetDisplay(airportMap, item.StopAirportId.Value);

        return $"{item.Id.Value} - route={routeDisplay} - stop_airport={stopAirportDisplay} - order={item.Order.Value} - duration_min={item.DurationMinutes.Value}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}


