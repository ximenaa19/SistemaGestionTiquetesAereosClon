// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\UI\RouteMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.Routes.UI;

public class RouteMenu
{
    private readonly CreateRouteUseCase _create;
    private readonly GetAllRoutesUseCase _getAll;
    private readonly GetRouteByIdUseCase _getById;
    private readonly GetRouteByOriginAndDestinationUseCase _getByPair;
    private readonly UpdateRouteUseCase _update;
    private readonly DeleteRouteUseCase _delete;

    private readonly GetAllAirportsUseCase _getAllAirports;

    public RouteMenu(
        CreateRouteUseCase create,
        GetAllRoutesUseCase getAll,
        GetRouteByIdUseCase getById,
        GetRouteByOriginAndDestinationUseCase getByPair,
        UpdateRouteUseCase update,
        DeleteRouteUseCase delete,
        GetAllAirportsUseCase getAllAirports)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByPair = getByPair;
        _update = update;
        _delete = delete;
        _getAllAirports = getAllAirports;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear route",
            "Listar routes",
            "Get route by ID",
            "Get route by origin+destination",
            "Actualizar route",
            "Eliminar route",
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
                        await PrintAirportsAsync();

                        Console.Write("\nIngrese aeropuerto_origen_id: ");
                        int originId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aeropuerto_destino_id: ");
                        int destinationId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese distancia_km [opcional]: ");
                        var distanceInput = Console.ReadLine();
                        int? distanceKm = string.IsNullOrWhiteSpace(distanceInput) ? null : int.Parse(distanceInput!);

                        Console.Write("Ingrese duracion_estimada_min [opcional]: ");
                        var durationInput = Console.ReadLine();
                        int? durationMin = string.IsNullOrWhiteSpace(durationInput) ? null : int.Parse(durationInput!);

                        await _create.ExecuteAsync(originId, destinationId, distanceKm, durationMin);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var airportMap = await GetAirportDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, airportMap));
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

                        var airportMapById = await GetAirportDisplayMapAsync();
                        Console.WriteLine(Format(byId, airportMapById));
                        break;

                    case 3:
                        await PrintAirportsAsync();

                        Console.Write("\nIngrese aeropuerto_origen_id: ");
                        int pairOriginId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aeropuerto_destino_id: ");
                        int pairDestinationId = int.Parse(Console.ReadLine()!);

                        var byPair = await _getByPair.ExecuteAsync(pairOriginId, pairDestinationId);
                        if (byPair is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var airportMapByPair = await GetAirportDisplayMapAsync();
                        Console.WriteLine(Format(byPair, airportMapByPair));
                        break;

                    case 4:
                        await PrintAirportsAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aeropuerto_origen_id: ");
                        int newOriginId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aeropuerto_destino_id: ");
                        int newDestinationId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese distancia_km [opcional]: ");
                        var newDistanceInput = Console.ReadLine();
                        int? newDistanceKm = string.IsNullOrWhiteSpace(newDistanceInput) ? null : int.Parse(newDistanceInput!);

                        Console.Write("Ingrese duracion_estimada_min [opcional]: ");
                        var newDurationInput = Console.ReadLine();
                        int? newDurationMin = string.IsNullOrWhiteSpace(newDurationInput) ? null : int.Parse(newDurationInput!);

                        await _update.ExecuteAsync(updateId, newOriginId, newDestinationId, newDistanceKm, newDurationMin);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 5:
                        Console.Write("Ingrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("âœ” Eliminado");
                        break;

                    case 6:
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

    private async Task PrintAirportsAsync()
    {
        Console.WriteLine("Aeropuertos disponibles:");
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

    private static string Format(Route item, Dictionary<int, string> airportMap)
    {
        string originDisplay = GetDisplay(airportMap, item.OriginAirportId.Value);
        string destinationDisplay = GetDisplay(airportMap, item.DestinationAirportId.Value);
        var distanceDisplay = item.DistanceKm.Value?.ToString() ?? "NULL";
        var durationDisplay = item.EstimatedDurationMin.Value?.ToString() ?? "NULL";

        return $"{item.Id.Value} - origin={originDisplay} - destination={destinationDisplay} - km={distanceDisplay} - duration_min={durationDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}


