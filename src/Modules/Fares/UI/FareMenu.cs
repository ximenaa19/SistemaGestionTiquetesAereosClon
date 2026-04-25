// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\UI\FareMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Globalization;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.CabinTypes.Application.UseCases;
using GestionAerolineas.src.Modules.Fares.Application.UseCases;
using GestionAerolineas.src.Modules.Fares.Domain.Aggregate;
using GestionAerolineas.src.Modules.PassengerTypes.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.Seasons.Application.UseCases;

namespace GestionAerolineas.src.Modules.Fares.UI;

public class FareMenu
{
    private readonly CreateFareUseCase _create;
    private readonly GetAllFaresUseCase _getAll;
    private readonly GetFareByIdUseCase _getById;
    private readonly GetFaresByRouteIdUseCase _getByRouteId;
    private readonly GetFareByKeysUseCase _getByKeys;
    private readonly UpdateFareUseCase _update;
    private readonly DeleteFareUseCase _delete;

    private readonly GetAllRoutesUseCase _getAllRoutes;
    private readonly GetAllAirportsUseCase _getAllAirports;
    private readonly GetAllCabinTypeUseCase _getAllCabinTypes;
    private readonly GetAllPassengerTypesUseCase _getAllPassengerTypes;
    private readonly GetAllSeasonsUseCase _getAllSeasons;

    public FareMenu(
        CreateFareUseCase create,
        GetAllFaresUseCase getAll,
        GetFareByIdUseCase getById,
        GetFaresByRouteIdUseCase getByRouteId,
        GetFareByKeysUseCase getByKeys,
        UpdateFareUseCase update,
        DeleteFareUseCase delete,
        GetAllRoutesUseCase getAllRoutes,
        GetAllAirportsUseCase getAllAirports,
        GetAllCabinTypeUseCase getAllCabinTypes,
        GetAllPassengerTypesUseCase getAllPassengerTypes,
        GetAllSeasonsUseCase getAllSeasons)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByRouteId = getByRouteId;
        _getByKeys = getByKeys;
        _update = update;
        _delete = delete;
        _getAllRoutes = getAllRoutes;
        _getAllAirports = getAllAirports;
        _getAllCabinTypes = getAllCabinTypes;
        _getAllPassengerTypes = getAllPassengerTypes;
        _getAllSeasons = getAllSeasons;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear fare",
            "Listar fares",
            "Get fare by ID",
            "Get fares by route_id",
            "Get fare by route+cabin+passenger+season",
            "Actualizar fare",
            "Eliminar fare",
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
                        await PrintCabinTypesAsync();
                        await PrintPassengerTypesAsync();
                        await PrintSeasonsAsync();

                        Console.Write("\nIngrese ruta_id: ");
                        int routeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese tipo_cabina_id: ");
                        int cabinTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese tipo_pasajero_id: ");
                        int passengerTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese temporada_id: ");
                        int seasonId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese precio_base: ");
                        decimal basePrice = ReadDecimal(Console.ReadLine()!);

                        Console.Write("Ingrese vigencia_desde (yyyy-MM-dd) [opcional]: ");
                        var fromInput = Console.ReadLine();
                        DateTime? validFrom = string.IsNullOrWhiteSpace(fromInput) ? null : DateTime.Parse(fromInput!);

                        Console.Write("Ingrese vigencia_hasta (yyyy-MM-dd) [opcional]: ");
                        var untilInput = Console.ReadLine();
                        DateTime? validUntil = string.IsNullOrWhiteSpace(untilInput) ? null : DateTime.Parse(untilInput!);

                        await _create.ExecuteAsync(routeId, cabinTypeId, passengerTypeId, seasonId, basePrice, validFrom, validUntil);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var routeMap = await GetRouteDisplayMapAsync();
                        var cabinMap = await GetCabinTypeDisplayMapAsync();
                        var passengerMap = await GetPassengerTypeDisplayMapAsync();
                        var seasonMap = await GetSeasonDisplayMapAsync();

                        var list = await _getAll.ExecuteAsync();
                        foreach (var item in list)
                            Console.WriteLine(Format(item, routeMap, cabinMap, passengerMap, seasonMap));
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
                        var cabinMapById = await GetCabinTypeDisplayMapAsync();
                        var passengerMapById = await GetPassengerTypeDisplayMapAsync();
                        var seasonMapById = await GetSeasonDisplayMapAsync();
                        Console.WriteLine(Format(byId, routeMapById, cabinMapById, passengerMapById, seasonMapById));
                        break;

                    case 3:
                        await PrintRoutesAsync();

                        Console.Write("\nIngrese ruta_id: ");
                        int searchRouteId = int.Parse(Console.ReadLine()!);

                        var faresByRoute = await _getByRouteId.ExecuteAsync(searchRouteId);
                        var routeMapByRoute = await GetRouteDisplayMapAsync();
                        var cabinMapByRoute = await GetCabinTypeDisplayMapAsync();
                        var passengerMapByRoute = await GetPassengerTypeDisplayMapAsync();
                        var seasonMapByRoute = await GetSeasonDisplayMapAsync();

                        foreach (var item in faresByRoute)
                            Console.WriteLine(Format(item, routeMapByRoute, cabinMapByRoute, passengerMapByRoute, seasonMapByRoute));
                        break;

                    case 4:
                        await PrintRoutesAsync();
                        await PrintCabinTypesAsync();
                        await PrintPassengerTypesAsync();
                        await PrintSeasonsAsync();

                        Console.Write("\nIngrese ruta_id: ");
                        int keyRouteId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese tipo_cabina_id: ");
                        int keyCabinTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese tipo_pasajero_id: ");
                        int keyPassengerTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese temporada_id: ");
                        int keySeasonId = int.Parse(Console.ReadLine()!);

                        var byKeys = await _getByKeys.ExecuteAsync(keyRouteId, keyCabinTypeId, keyPassengerTypeId, keySeasonId);
                        if (byKeys is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var routeMapByKeys = await GetRouteDisplayMapAsync();
                        var cabinMapByKeys = await GetCabinTypeDisplayMapAsync();
                        var passengerMapByKeys = await GetPassengerTypeDisplayMapAsync();
                        var seasonMapByKeys = await GetSeasonDisplayMapAsync();
                        Console.WriteLine(Format(byKeys, routeMapByKeys, cabinMapByKeys, passengerMapByKeys, seasonMapByKeys));
                        break;

                    case 5:
                        await PrintRoutesAsync();
                        await PrintCabinTypesAsync();
                        await PrintPassengerTypesAsync();
                        await PrintSeasonsAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese ruta_id: ");
                        int newRouteId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese tipo_cabina_id: ");
                        int newCabinTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese tipo_pasajero_id: ");
                        int newPassengerTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese temporada_id: ");
                        int newSeasonId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese precio_base: ");
                        decimal newBasePrice = ReadDecimal(Console.ReadLine()!);

                        Console.Write("Ingrese vigencia_desde (yyyy-MM-dd) [opcional]: ");
                        var newFromInput = Console.ReadLine();
                        DateTime? newValidFrom = string.IsNullOrWhiteSpace(newFromInput) ? null : DateTime.Parse(newFromInput!);

                        Console.Write("Ingrese vigencia_hasta (yyyy-MM-dd) [opcional]: ");
                        var newUntilInput = Console.ReadLine();
                        DateTime? newValidUntil = string.IsNullOrWhiteSpace(newUntilInput) ? null : DateTime.Parse(newUntilInput!);

                        await _update.ExecuteAsync(updateId, newRouteId, newCabinTypeId, newPassengerTypeId, newSeasonId, newBasePrice, newValidFrom, newValidUntil);
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

    private async Task PrintCabinTypesAsync()
    {
        Console.WriteLine("\nTipos de cabina disponibles:");
        var cabinTypes = (await _getAllCabinTypes.ExecuteAsync()).ToList();

        foreach (var cabinType in cabinTypes.Take(30))
            Console.WriteLine($"{cabinType.Id.Value} - {cabinType.Name.Value}");

        if (cabinTypes.Count > 30)
            Console.WriteLine("(Mostrando solo los primeros 30)");
    }

    private async Task PrintPassengerTypesAsync()
    {
        Console.WriteLine("\nTipos de pasajero disponibles:");
        var passengerTypes = (await _getAllPassengerTypes.ExecuteAsync()).ToList();

        foreach (var pt in passengerTypes.Take(30))
        {
            var ageMin = pt.AgeMin?.ToString() ?? "NULL";
            var ageMax = pt.AgeMax?.ToString() ?? "NULL";
            Console.WriteLine($"{pt.Id.Value} - {pt.Name.Value} - edad_min={ageMin} - edad_max={ageMax}");
        }

        if (passengerTypes.Count > 30)
            Console.WriteLine("(Mostrando solo los primeros 30)");
    }

    private async Task PrintSeasonsAsync()
    {
        Console.WriteLine("\nTemporadas disponibles:");
        var seasons = (await _getAllSeasons.ExecuteAsync()).ToList();

        foreach (var s in seasons.Take(30))
            Console.WriteLine($"{s.Id.Value} - {s.Name.Value} - factor={s.PriceFactor.Value}");

        if (seasons.Count > 30)
            Console.WriteLine("(Mostrando solo las primeras 30)");
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

    private async Task<Dictionary<int, string>> GetCabinTypeDisplayMapAsync()
    {
        var cabinTypes = await _getAllCabinTypes.ExecuteAsync();
        return cabinTypes.ToDictionary(c => c.Id.Value, c => c.Name.Value);
    }

    private async Task<Dictionary<int, string>> GetPassengerTypeDisplayMapAsync()
    {
        var passengerTypes = await _getAllPassengerTypes.ExecuteAsync();
        return passengerTypes.ToDictionary(p => p.Id.Value, p => p.Name.Value);
    }

    private async Task<Dictionary<int, string>> GetSeasonDisplayMapAsync()
    {
        var seasons = await _getAllSeasons.ExecuteAsync();
        return seasons.ToDictionary(s => s.Id.Value, s => s.Name.Value);
    }

    private static string Format(
        Fare item,
        Dictionary<int, string> routeMap,
        Dictionary<int, string> cabinTypeMap,
        Dictionary<int, string> passengerTypeMap,
        Dictionary<int, string> seasonMap)
    {
        string routeDisplay = GetDisplay(routeMap, item.RouteId.Value);
        string cabinDisplay = GetDisplay(cabinTypeMap, item.CabinTypeId.Value);
        string passengerDisplay = GetDisplay(passengerTypeMap, item.PassengerTypeId.Value);
        string seasonDisplay = GetDisplay(seasonMap, item.SeasonId.Value);
        var fromDisplay = item.ValidFrom.Value?.ToString("yyyy-MM-dd") ?? "NULL";
        var untilDisplay = item.ValidUntil.Value?.ToString("yyyy-MM-dd") ?? "NULL";

        return $"{item.Id.Value} - route={routeDisplay} - cabin={cabinDisplay} - passenger={passengerDisplay} - season={seasonDisplay} - base_price={item.BasePrice.Value} - from={fromDisplay} - until={untilDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }

    private static decimal ReadDecimal(string input)
    {
        var normalized = (input ?? string.Empty).Trim().Replace(',', '.');
        return decimal.Parse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture);
    }
}

