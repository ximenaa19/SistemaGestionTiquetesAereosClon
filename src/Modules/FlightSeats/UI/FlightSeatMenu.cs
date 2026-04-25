// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\UI\FlightSeatMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.CabinTypes.Application.UseCases;
using GestionAerolineas.src.Modules.FlightSeats.Application.UseCases;
using GestionAerolineas.src.Modules.FlightSeats.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.SeatLocationTypes.Application.UseCases;

namespace GestionAerolineas.src.Modules.FlightSeats.UI;

public class FlightSeatMenu
{
    private const int TopCount = 10;

    private readonly CreateFlightSeatUseCase _create;
    private readonly GetAllFlightSeatsUseCase _getAll;
    private readonly GetFlightSeatByIdUseCase _getById;
    private readonly GetFlightSeatsByFlightIdUseCase _getByFlightId;
    private readonly GetFlightSeatByFlightAndCodeUseCase _getByFlightAndCode;
    private readonly GetAvailableSeatsByFlightIdUseCase _getAvailableByFlightId;
    private readonly GetOccupiedSeatsByFlightIdUseCase _getOccupiedByFlightId;
    private readonly UpdateFlightSeatUseCase _update;
    private readonly DeleteFlightSeatUseCase _delete;

    private readonly GetAllFlightsUseCase _getAllFlights;
    private readonly GetAllRoutesUseCase _getAllRoutes;
    private readonly GetAllAirportsUseCase _getAllAirports;
    private readonly GetAllAirlinesUseCase _getAllAirlines;
    private readonly GetAllCabinTypeUseCase _getAllCabinTypes;
    private readonly GetAllSeatLocationTypesUseCase _getAllSeatLocationTypes;

    public FlightSeatMenu(
        CreateFlightSeatUseCase create,
        GetAllFlightSeatsUseCase getAll,
        GetFlightSeatByIdUseCase getById,
        GetFlightSeatsByFlightIdUseCase getByFlightId,
        GetFlightSeatByFlightAndCodeUseCase getByFlightAndCode,
        GetAvailableSeatsByFlightIdUseCase getAvailableByFlightId,
        GetOccupiedSeatsByFlightIdUseCase getOccupiedByFlightId,
        UpdateFlightSeatUseCase update,
        DeleteFlightSeatUseCase delete,
        GetAllFlightsUseCase getAllFlights,
        GetAllRoutesUseCase getAllRoutes,
        GetAllAirportsUseCase getAllAirports,
        GetAllAirlinesUseCase getAllAirlines,
        GetAllCabinTypeUseCase getAllCabinTypes,
        GetAllSeatLocationTypesUseCase getAllSeatLocationTypes)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByFlightId = getByFlightId;
        _getByFlightAndCode = getByFlightAndCode;
        _getAvailableByFlightId = getAvailableByFlightId;
        _getOccupiedByFlightId = getOccupiedByFlightId;
        _update = update;
        _delete = delete;
        _getAllFlights = getAllFlights;
        _getAllRoutes = getAllRoutes;
        _getAllAirports = getAllAirports;
        _getAllAirlines = getAllAirlines;
        _getAllCabinTypes = getAllCabinTypes;
        _getAllSeatLocationTypes = getAllSeatLocationTypes;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear flight seat",
            "Listar flight seats",
            "Get flight seat by ID",
            "Get seats by flight_id",
            "Get seat by flight_id + seat_code",
            "Get available seats by flight_id",
            "Get occupied seats by flight_id",
            "Actualizar flight seat",
            "Eliminar flight seat",
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
                        await PrintFlightsAsync();
                        await PrintCabinTypesAsync();
                        await PrintSeatLocationTypesAsync();

                        Console.Write("\nIngrese vuelo_id: ");
                        int flightId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese codigo_asiento (ej: 12A): ");
                        string seatCode = Console.ReadLine()!;

                        Console.Write("Ingrese tipo_cabina_id: ");
                        int cabinTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese tipo_ubicacion_id: ");
                        int locationTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese esta_ocupado (true/false) [default=false]: ");
                        var occupiedInput = Console.ReadLine();
                        bool isOccupied = string.IsNullOrWhiteSpace(occupiedInput) ? false : bool.Parse(occupiedInput);

                        await _create.ExecuteAsync(flightId, seatCode, cabinTypeId, locationTypeId, isOccupied);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        await PrintListAsync(await _getAll.ExecuteAsync());
                        break;

                    case 2:
                        await PrintSeatsForSelectionAsync();

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
                        await PrintFlightsAsync();

                        Console.Write("\nIngrese vuelo_id: ");
                        int searchFlightId = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getByFlightId.ExecuteAsync(searchFlightId));
                        break;

                    case 4:
                        await PrintFlightsAsync();

                        Console.Write("\nIngrese vuelo_id: ");
                        int keyFlightId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese codigo_asiento: ");
                        string keySeatCode = Console.ReadLine()!;

                        var byKey = await _getByFlightAndCode.ExecuteAsync(keyFlightId, keySeatCode);
                        if (byKey is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        await PrintOneAsync(byKey);
                        break;

                    case 5:
                        await PrintFlightsAsync();

                        Console.Write("\nIngrese vuelo_id: ");
                        int availFlightId = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getAvailableByFlightId.ExecuteAsync(availFlightId));
                        break;

                    case 6:
                        await PrintFlightsAsync();

                        Console.Write("\nIngrese vuelo_id: ");
                        int occFlightId = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getOccupiedByFlightId.ExecuteAsync(occFlightId));
                        break;

                    case 7:
                        await PrintSeatsForSelectionAsync();
                        await PrintFlightsAsync();
                        await PrintCabinTypesAsync();
                        await PrintSeatLocationTypesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese vuelo_id: ");
                        int newFlightId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese codigo_asiento (ej: 12A): ");
                        string newSeatCode = Console.ReadLine()!;

                        Console.Write("Ingrese tipo_cabina_id: ");
                        int newCabinTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese tipo_ubicacion_id: ");
                        int newLocationTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese esta_ocupado (true/false): ");
                        bool newIsOccupied = bool.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newFlightId, newSeatCode, newCabinTypeId, newLocationTypeId, newIsOccupied);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 8:
                        await PrintSeatsForSelectionAsync();

                        Console.Write("\nIngrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("âœ” Eliminado");
                        break;

                    case 9:
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

    private async Task PrintSeatsForSelectionAsync()
    {
        Console.WriteLine($"FlightSeats (top {TopCount}):");
        var list = (await _getAll.ExecuteAsync()).Take(TopCount).ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        await PrintListAsync(list);
    }

    private async Task PrintListAsync(IEnumerable<FlightSeat> list)
    {
        var items = list.ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("(sin resultados)");
            return;
        }

        var flightMap = await GetFlightDisplayMapAsync();
        var cabinMap = await GetCabinTypeDisplayMapAsync();
        var locationMap = await GetSeatLocationTypeDisplayMapAsync();

        foreach (var item in items)
            Console.WriteLine(Format(item, flightMap, cabinMap, locationMap));
    }

    private async Task PrintOneAsync(FlightSeat item)
    {
        var flightMap = await GetFlightDisplayMapAsync();
        var cabinMap = await GetCabinTypeDisplayMapAsync();
        var locationMap = await GetSeatLocationTypeDisplayMapAsync();
        Console.WriteLine(Format(item, flightMap, cabinMap, locationMap));
    }

    private async Task PrintFlightsAsync()
    {
        var flights = (await _getAllFlights.ExecuteAsync()).ToList();
        var flightMap = await GetFlightDisplayMapAsync();

        Console.WriteLine("Vuelos disponibles:");
        PrintTopWithFormat(flights, f => $"{f.Id.Value} - {GetDisplay(flightMap, f.Id.Value)}");

        Console.Write("Buscar vuelo (codigo/ruta) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = flights
            .Where(f => GetDisplay(flightMap, f.Id.Value).ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, f => $"{f.Id.Value} - {GetDisplay(flightMap, f.Id.Value)}");
    }

    private async Task PrintCabinTypesAsync()
    {
        var cabinTypes = (await _getAllCabinTypes.ExecuteAsync()).ToList();
        Console.WriteLine("\nTipos de cabina disponibles:");
        PrintTopWithFormat(cabinTypes, c => $"{c.Id.Value} - {c.Name.Value}");

        Console.Write("Buscar tipo de cabina (texto) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = cabinTypes
            .Where(c => c.Name.Value.ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, c => $"{c.Id.Value} - {c.Name.Value}");
    }

    private async Task PrintSeatLocationTypesAsync()
    {
        var types = (await _getAllSeatLocationTypes.ExecuteAsync()).ToList();
        Console.WriteLine("\nTipos de ubicacion disponibles:");
        PrintTopWithFormat(types, t => $"{t.Id.Value} - {t.Name.Value}");

        Console.Write("Buscar tipo de ubicacion (texto) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = types
            .Where(t => t.Name.Value.ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, t => $"{t.Id.Value} - {t.Name.Value}");
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

    private async Task<Dictionary<int, string>> GetFlightDisplayMapAsync()
    {
        var flights = await _getAllFlights.ExecuteAsync();
        var airlines = await _getAllAirlines.ExecuteAsync();
        var routes = await _getAllRoutes.ExecuteAsync();
        var airports = await _getAllAirports.ExecuteAsync();

        var airlineMap = airlines.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
        var airportMap = airports.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
        var routeMap = routes.ToDictionary(
            r => r.Id.Value,
            r =>
            {
                var origin = GetDisplay(airportMap, r.OriginAirportId.Value);
                var dest = GetDisplay(airportMap, r.DestinationAirportId.Value);
                return $"{origin} -> {dest}";
            });

        return flights.ToDictionary(
            f => f.Id.Value,
            f =>
            {
                var airline = GetDisplay(airlineMap, f.AirlineId.Value);
                var route = GetDisplay(routeMap, f.RouteId.Value);
                return $"{f.Code.Value} - {airline} - {route} - dep={f.DepartureDateTime.Value:yyyy-MM-dd HH:mm}";
            });
    }

    private async Task<Dictionary<int, string>> GetCabinTypeDisplayMapAsync()
    {
        var cabinTypes = await _getAllCabinTypes.ExecuteAsync();
        return cabinTypes.ToDictionary(c => c.Id.Value, c => c.Name.Value);
    }

    private async Task<Dictionary<int, string>> GetSeatLocationTypeDisplayMapAsync()
    {
        var types = await _getAllSeatLocationTypes.ExecuteAsync();
        return types.ToDictionary(t => t.Id.Value, t => t.Name.Value);
    }

    private static string Format(
        FlightSeat item,
        Dictionary<int, string> flightMap,
        Dictionary<int, string> cabinMap,
        Dictionary<int, string> locationMap)
    {
        var flightDisplay = GetDisplay(flightMap, item.FlightId.Value);
        var cabinDisplay = GetDisplay(cabinMap, item.CabinTypeId.Value);
        var locationDisplay = GetDisplay(locationMap, item.LocationTypeId.Value);
        var occupiedDisplay = item.IsOccupied.Value ? "occupied" : "available";

        return $"{item.Id.Value} - flight={flightDisplay} [{item.FlightId.Value}] - seat={item.Code.Value} - cabin={cabinDisplay} [{item.CabinTypeId.Value}] - location={locationDisplay} [{item.LocationTypeId.Value}] - {occupiedDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? display : $"#{id}";
    }
}


