// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\UI\FlightMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.FlightStates.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;

namespace GestionAerolineas.src.Modules.Flights.UI;

public class FlightMenu
{
    private const int TopCount = 10;

    private readonly CreateFlightUseCase _create;
    private readonly GetAllFlightsUseCase _getAll;
    private readonly GetFlightByIdUseCase _getById;
    private readonly GetFlightByCodeUseCase _getByCode;
    private readonly GetFlightsByAirlineIdUseCase _getByAirlineId;
    private readonly GetFlightsByRouteIdUseCase _getByRouteId;
    private readonly GetFlightsByDateRangeUseCase _getByDateRange;
    private readonly GetFlightsByStateIdUseCase _getByStateId;
    private readonly UpdateFlightUseCase _update;
    private readonly DeleteFlightUseCase _delete;

    private readonly GetAllAirlinesUseCase _getAllAirlines;
    private readonly GetAllRoutesUseCase _getAllRoutes;
    private readonly GetAllAirportsUseCase _getAllAirports;
    private readonly GetAllAircraftUseCase _getAllAircraft;
    private readonly GetAllFlightStatesUseCase _getAllFlightStates;

    public FlightMenu(
        CreateFlightUseCase create,
        GetAllFlightsUseCase getAll,
        GetFlightByIdUseCase getById,
        GetFlightByCodeUseCase getByCode,
        GetFlightsByAirlineIdUseCase getByAirlineId,
        GetFlightsByRouteIdUseCase getByRouteId,
        GetFlightsByDateRangeUseCase getByDateRange,
        GetFlightsByStateIdUseCase getByStateId,
        UpdateFlightUseCase update,
        DeleteFlightUseCase delete,
        GetAllAirlinesUseCase getAllAirlines,
        GetAllRoutesUseCase getAllRoutes,
        GetAllAirportsUseCase getAllAirports,
        GetAllAircraftUseCase getAllAircraft,
        GetAllFlightStatesUseCase getAllFlightStates)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByCode = getByCode;
        _getByAirlineId = getByAirlineId;
        _getByRouteId = getByRouteId;
        _getByDateRange = getByDateRange;
        _getByStateId = getByStateId;
        _update = update;
        _delete = delete;
        _getAllAirlines = getAllAirlines;
        _getAllRoutes = getAllRoutes;
        _getAllAirports = getAllAirports;
        _getAllAircraft = getAllAircraft;
        _getAllFlightStates = getAllFlightStates;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear flight",
            "Listar flights",
            "Get flight by ID",
            "Get flight by code",
            "Get flights by airline_id",
            "Get flights by route_id",
            "Get flights by departure date range",
            "Get flights by state_id",
            "Actualizar flight",
            "Eliminar flight",
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
                        await PrintAirlinesAsync();
                        await PrintRoutesAsync();

                        Console.Write("\nIngrese codigo_vuelo: ");
                        string code = Console.ReadLine()!;

                        Console.Write("Ingrese aerolinea_id: ");
                        int airlineId = int.Parse(Console.ReadLine()!);

                        await PrintAircraftAsync(airlineId);
                        await PrintFlightStatesAsync();

                        Console.Write("Ingrese ruta_id: ");
                        int routeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aeronave_id: ");
                        int aircraftId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese estado_vuelo_id: ");
                        int stateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_salida (yyyy-MM-dd HH:mm): ");
                        var departure = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_llegada_estimada (yyyy-MM-dd HH:mm): ");
                        var arrival = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese capacidad_total: ");
                        int totalCapacity = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese asientos_disponibles [default=capacidad_total]: ");
                        var seatsInput = Console.ReadLine();
                        int availableSeats = string.IsNullOrWhiteSpace(seatsInput) ? totalCapacity : int.Parse(seatsInput!);

                        Console.Write("Ingrese reprogramado_en (yyyy-MM-dd HH:mm) [opcional]: ");
                        var resInput = Console.ReadLine();
                        DateTime? rescheduledAt = string.IsNullOrWhiteSpace(resInput) ? null : DateTime.Parse(resInput!);

                        await _create.ExecuteAsync(code, airlineId, routeId, aircraftId, departure, arrival, totalCapacity, availableSeats, stateId, rescheduledAt);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        await PrintListAsync(await _getAll.ExecuteAsync());
                        break;

                    case 2:
                        await PrintFlightsForSelectionAsync();
                        Console.Write("\nIngrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);

                        var byId = await _getById.ExecuteAsync(searchId);
                        if (byId is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        await PrintOneAsync(byId);
                        break;

                    case 3:
                        Console.Write("Ingrese codigo_vuelo: ");
                        string searchCode = Console.ReadLine()!;

                        var byCode = await _getByCode.ExecuteAsync(searchCode);
                        if (byCode is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        await PrintOneAsync(byCode);
                        break;

                    case 4:
                        await PrintAirlinesAsync();
                        Console.Write("\nIngrese aerolinea_id: ");
                        int searchAirlineId = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getByAirlineId.ExecuteAsync(searchAirlineId));
                        break;

                    case 5:
                        await PrintRoutesAsync();
                        Console.Write("\nIngrese ruta_id: ");
                        int searchRouteId = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getByRouteId.ExecuteAsync(searchRouteId));
                        break;

                    case 6:
                        Console.Write("Ingrese desde (yyyy-MM-dd HH:mm): ");
                        var from = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese hasta (yyyy-MM-dd HH:mm): ");
                        var to = DateTime.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getByDateRange.ExecuteAsync(from, to));
                        break;

                    case 7:
                        await PrintFlightStatesAsync();
                        Console.Write("\nIngrese estado_vuelo_id: ");
                        int searchStateId = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getByStateId.ExecuteAsync(searchStateId));
                        break;

                    case 8:
                        await PrintFlightsForSelectionAsync();
                        await PrintAirlinesAsync();
                        await PrintRoutesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese codigo_vuelo: ");
                        string newCode = Console.ReadLine()!;

                        Console.Write("Ingrese aerolinea_id: ");
                        int newAirlineId = int.Parse(Console.ReadLine()!);

                        await PrintAircraftAsync(newAirlineId);
                        await PrintFlightStatesAsync();

                        Console.Write("Ingrese ruta_id: ");
                        int newRouteId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aeronave_id: ");
                        int newAircraftId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese estado_vuelo_id: ");
                        int newStateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_salida (yyyy-MM-dd HH:mm): ");
                        var newDeparture = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_llegada_estimada (yyyy-MM-dd HH:mm): ");
                        var newArrival = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese capacidad_total: ");
                        int newTotalCapacity = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese asientos_disponibles [default=capacidad_total]: ");
                        var newSeatsInput = Console.ReadLine();
                        int newAvailableSeats = string.IsNullOrWhiteSpace(newSeatsInput) ? newTotalCapacity : int.Parse(newSeatsInput!);

                        Console.Write("Ingrese reprogramado_en (yyyy-MM-dd HH:mm) [opcional]: ");
                        var newResInput = Console.ReadLine();
                        DateTime? newRescheduledAt = string.IsNullOrWhiteSpace(newResInput) ? null : DateTime.Parse(newResInput!);

                        await _update.ExecuteAsync(updateId, newCode, newAirlineId, newRouteId, newAircraftId, newDeparture, newArrival, newTotalCapacity, newAvailableSeats, newStateId, newRescheduledAt);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 9:
                        await PrintFlightsForSelectionAsync();
                        Console.Write("\nIngrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("âœ” Eliminado");
                        break;

                    case 10:
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

    private async Task PrintFlightsForSelectionAsync()
    {
        Console.WriteLine($"Flights (top {TopCount}):");
        var list = (await _getAll.ExecuteAsync()).Take(TopCount).ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        await PrintListAsync(list);
    }

    private async Task PrintListAsync(IEnumerable<Flight> list)
    {
        var items = list.ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("(sin resultados)");
            return;
        }

        var airlineMap = await GetAirlineDisplayMapAsync();
        var routeMap = await GetRouteDisplayMapAsync();
        var aircraftMap = await GetAircraftDisplayMapAsync();
        var stateMap = await GetStateDisplayMapAsync();

        foreach (var item in items)
            Console.WriteLine(Format(item, airlineMap, routeMap, aircraftMap, stateMap));
    }

    private async Task PrintOneAsync(Flight item)
    {
        var airlineMap = await GetAirlineDisplayMapAsync();
        var routeMap = await GetRouteDisplayMapAsync();
        var aircraftMap = await GetAircraftDisplayMapAsync();
        var stateMap = await GetStateDisplayMapAsync();
        Console.WriteLine(Format(item, airlineMap, routeMap, aircraftMap, stateMap));
    }

    private async Task PrintAirlinesAsync()
    {
        var airlines = (await _getAllAirlines.ExecuteAsync()).ToList();
        Console.WriteLine("Aerolineas disponibles:");
        PrintTopWithFormat(airlines, a => $"{a.Id.Value} - {a.Name.Value} - iata={a.IataCode.Value}");

        Console.Write("Buscar aerolinea (texto) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = airlines
            .Where(a => $"{a.Name.Value} {a.IataCode.Value}".ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, a => $"{a.Id.Value} - {a.Name.Value} - iata={a.IataCode.Value}");
    }

    private async Task PrintRoutesAsync()
    {
        var routes = (await _getAllRoutes.ExecuteAsync()).ToList();
        var airportMap = await GetAirportDisplayMapAsync();

        Console.WriteLine("Rutas disponibles:");
        PrintTopWithFormat(routes, r =>
        {
            var origin = GetDisplay(airportMap, r.OriginAirportId.Value);
            var dest = GetDisplay(airportMap, r.DestinationAirportId.Value);
            return $"{r.Id.Value} - {origin} -> {dest}";
        });

        Console.Write("Buscar ruta (texto) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = routes
            .Where(r =>
            {
                var origin = GetDisplay(airportMap, r.OriginAirportId.Value);
                var dest = GetDisplay(airportMap, r.DestinationAirportId.Value);
                var haystack = $"{origin} {dest} {r.Id.Value}".ToUpperInvariant();
                return haystack.Contains(normalized);
            })
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, r =>
        {
            var origin = GetDisplay(airportMap, r.OriginAirportId.Value);
            var dest = GetDisplay(airportMap, r.DestinationAirportId.Value);
            return $"{r.Id.Value} - {origin} -> {dest}";
        });
    }

    private async Task PrintAircraftAsync(int airlineId)
    {
        var aircraft = (await _getAllAircraft.ExecuteAsync())
            .Where(a => a.AirlineId.Value == airlineId)
            .ToList();

        Console.WriteLine("\nAeronaves disponibles para esa aerolinea:");
        PrintTopWithFormat(aircraft, a => $"{a.Id.Value} - {a.Registration.Value} - active={(a.IsActive.Value ? "true" : "false")}");

        Console.Write("Buscar aeronave (texto) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = aircraft
            .Where(a => a.Registration.Value.ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, a => $"{a.Id.Value} - {a.Registration.Value} - active={(a.IsActive.Value ? "true" : "false")}");
    }

    private async Task PrintFlightStatesAsync()
    {
        var states = (await _getAllFlightStates.ExecuteAsync()).ToList();
        Console.WriteLine("\nEstados de vuelo disponibles:");
        PrintTopWithFormat(states, s => $"{s.Id.Value} - {s.Name.Value}");

        Console.Write("Buscar estado (texto) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = states
            .Where(s => s.Name.Value.ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, s => $"{s.Id.Value} - {s.Name.Value}");
    }

    private static void PrintTopWithFormat<T>(IEnumerable<T> items, Func<T, string> formatter)
    {
        var list = items.Take(TopCount).ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        foreach (var item in list)
            Console.WriteLine(formatter(item));
    }

    private async Task<Dictionary<int, string>> GetAirlineDisplayMapAsync()
    {
        var airlines = await _getAllAirlines.ExecuteAsync();
        return airlines.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
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
                var dest = GetDisplay(airportMap, r.DestinationAirportId.Value);
                return $"{origin} -> {dest}";
            });
    }

    private async Task<Dictionary<int, string>> GetAircraftDisplayMapAsync()
    {
        var aircraft = await _getAllAircraft.ExecuteAsync();
        return aircraft.ToDictionary(a => a.Id.Value, a => a.Registration.Value);
    }

    private async Task<Dictionary<int, string>> GetStateDisplayMapAsync()
    {
        var states = await _getAllFlightStates.ExecuteAsync();
        return states.ToDictionary(s => s.Id.Value, s => s.Name.Value);
    }

    private static string Format(
        Flight item,
        Dictionary<int, string> airlineMap,
        Dictionary<int, string> routeMap,
        Dictionary<int, string> aircraftMap,
        Dictionary<int, string> stateMap)
    {
        var airlineDisplay = GetDisplay(airlineMap, item.AirlineId.Value);
        var routeDisplay = GetDisplay(routeMap, item.RouteId.Value);
        var aircraftDisplay = GetDisplay(aircraftMap, item.AircraftId.Value);
        var stateDisplay = GetDisplay(stateMap, item.StateId.Value);
        var rescheduledDisplay = item.RescheduledAt.Value?.ToString("yyyy-MM-dd HH:mm") ?? "NULL";

        return $"{item.Id.Value} - code={item.Code.Value} - airline={airlineDisplay} - route={routeDisplay} - aircraft={aircraftDisplay} - dep={item.DepartureDateTime.Value:yyyy-MM-dd HH:mm} - arr={item.EstimatedArrivalDateTime.Value:yyyy-MM-dd HH:mm} - cap={item.TotalCapacity.Value} - avail={item.AvailableSeats.Value} - state={stateDisplay} - rescheduled={rescheduledDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}


