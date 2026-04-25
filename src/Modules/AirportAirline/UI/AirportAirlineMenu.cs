// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\UI\AirportAirlineMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.AirportAirline.Application.UseCases;
using GestionAerolineas.src.Modules.AirportAirline.Domain.Aggregate;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;

namespace GestionAerolineas.src.Modules.AirportAirline.UI;

public class AirportAirlineMenu
{
    private readonly CreateAirportAirlineUseCase _create;
    private readonly GetAllAirportAirlinesUseCase _getAll;
    private readonly GetAirportAirlineByIdUseCase _getById;
    private readonly GetAirportAirlineByAirportAndAirlineUseCase _getByPair;
    private readonly UpdateAirportAirlineUseCase _update;
    private readonly DeleteAirportAirlineUseCase _delete;

    private readonly GetAllAirportsUseCase _getAllAirports;
    private readonly GetAllAirlinesUseCase _getAllAirlines;

    public AirportAirlineMenu(
        CreateAirportAirlineUseCase create,
        GetAllAirportAirlinesUseCase getAll,
        GetAirportAirlineByIdUseCase getById,
        GetAirportAirlineByAirportAndAirlineUseCase getByPair,
        UpdateAirportAirlineUseCase update,
        DeleteAirportAirlineUseCase delete,
        GetAllAirportsUseCase getAllAirports,
        GetAllAirlinesUseCase getAllAirlines)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByPair = getByPair;
        _update = update;
        _delete = delete;
        _getAllAirports = getAllAirports;
        _getAllAirlines = getAllAirlines;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear airport-airline relation",
            "Listar airport-airline relations",
            "Get relation by ID",
            "Get relation by airport+airline",
            "Actualizar relation",
            "Eliminar relation",
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
                        await PrintAirlinesAsync();

                        Console.Write("\nIngrese aeropuerto_id: ");
                        int airportId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aerolinea_id: ");
                        int airlineId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese terminal [opcional]: ");
                        var terminal = Console.ReadLine();

                        Console.Write("Ingrese fecha_inicio (yyyy-MM-dd): ");
                        var startDate = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_fin (yyyy-MM-dd) [opcional]: ");
                        var endInput = Console.ReadLine();
                        DateTime? endDate = string.IsNullOrWhiteSpace(endInput) ? null : DateTime.Parse(endInput!);

                        Console.Write("Ingrese activa (true/false) [default=true]: ");
                        var activeInput = Console.ReadLine();
                        bool isActive = string.IsNullOrWhiteSpace(activeInput) ? true : bool.Parse(activeInput);

                        await _create.ExecuteAsync(airportId, airlineId, terminal, startDate, endDate, isActive);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var airportMap = await GetAirportDisplayMapAsync();
                        var airlineMap = await GetAirlineDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, airportMap, airlineMap));
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
                        var airlineMapById = await GetAirlineDisplayMapAsync();
                        Console.WriteLine(Format(byId, airportMapById, airlineMapById));
                        break;

                    case 3:
                        await PrintAirportsAsync();
                        await PrintAirlinesAsync();

                        Console.Write("\nIngrese aeropuerto_id: ");
                        int pairAirportId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aerolinea_id: ");
                        int pairAirlineId = int.Parse(Console.ReadLine()!);

                        var byPair = await _getByPair.ExecuteAsync(pairAirportId, pairAirlineId);
                        if (byPair is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var airportMapByPair = await GetAirportDisplayMapAsync();
                        var airlineMapByPair = await GetAirlineDisplayMapAsync();
                        Console.WriteLine(Format(byPair, airportMapByPair, airlineMapByPair));
                        break;

                    case 4:
                        await PrintAirportsAsync();
                        await PrintAirlinesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aeropuerto_id: ");
                        int newAirportId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aerolinea_id: ");
                        int newAirlineId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese terminal [opcional]: ");
                        var newTerminal = Console.ReadLine();

                        Console.Write("Ingrese fecha_inicio (yyyy-MM-dd): ");
                        var newStartDate = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_fin (yyyy-MM-dd) [opcional]: ");
                        var newEndInput = Console.ReadLine();
                        DateTime? newEndDate = string.IsNullOrWhiteSpace(newEndInput) ? null : DateTime.Parse(newEndInput!);

                        Console.Write("Ingrese activa (true/false): ");
                        bool newIsActive = bool.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newAirportId, newAirlineId, newTerminal, newStartDate, newEndDate, newIsActive);
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

    private async Task PrintAirlinesAsync()
    {
        Console.WriteLine("\nAerolineas disponibles:");
        var airlines = (await _getAllAirlines.ExecuteAsync()).ToList();

        foreach (var airline in airlines.Take(30))
            Console.WriteLine($"{airline.Id.Value} - {airline.Name.Value} - iata={airline.IataCode.Value}");

        if (airlines.Count > 30)
            Console.WriteLine("(Mostrando solo las primeras 30)");
    }

    private async Task<Dictionary<int, string>> GetAirportDisplayMapAsync()
    {
        var airports = await _getAllAirports.ExecuteAsync();
        return airports.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
    }

    private async Task<Dictionary<int, string>> GetAirlineDisplayMapAsync()
    {
        var airlines = await _getAllAirlines.ExecuteAsync();
        return airlines.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
    }

    private static string Format(
        AirportAirlineRelation item,
        Dictionary<int, string> airportMap,
        Dictionary<int, string> airlineMap)
    {
        string airportDisplay = GetDisplay(airportMap, item.AirportId.Value);
        string airlineDisplay = GetDisplay(airlineMap, item.AirlineId.Value);
        var terminalDisplay = item.Terminal.Value ?? "NULL";
        var endDateDisplay = item.EndDate.Value?.ToString("yyyy-MM-dd") ?? "NULL";
        var activeDisplay = item.IsActive.Value ? "active" : "inactive";

        return $"{item.Id.Value} - airport={airportDisplay} - airline={airlineDisplay} - terminal={terminalDisplay} - start={item.StartDate.Value:yyyy-MM-dd} - end={endDateDisplay} - {activeDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}


