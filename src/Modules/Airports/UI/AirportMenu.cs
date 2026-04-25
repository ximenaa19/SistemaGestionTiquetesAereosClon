// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\UI\AirportMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Domain.Aggregate;
using GestionAerolineas.src.Modules.Cities.Application.UseCases;

namespace GestionAerolineas.src.Modules.Airports.UI;

public class AirportMenu
{
    private readonly CreateAirportUseCase _create;
    private readonly GetAllAirportsUseCase _getAll;
    private readonly GetAirportByIdUseCase _getById;
    private readonly GetAirportByNameUseCase _getByName;
    private readonly UpdateAirportUseCase _update;
    private readonly DeleteAirportUseCase _delete;

    private readonly GetAllCitiesUseCase _getAllCities;

    public AirportMenu(
        CreateAirportUseCase create,
        GetAllAirportsUseCase getAll,
        GetAirportByIdUseCase getById,
        GetAirportByNameUseCase getByName,
        UpdateAirportUseCase update,
        DeleteAirportUseCase delete,
        GetAllCitiesUseCase getAllCities)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByName = getByName;
        _update = update;
        _delete = delete;
        _getAllCities = getAllCities;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear airport",
            "Listar airports",
            "Get airport by ID",
            "Get airport by name",
            "Actualizar airport",
            "Eliminar airport",
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
                        await PrintCitiesAsync();

                        Console.Write("\nIngrese nombre: ");
                        string name = Console.ReadLine()!;

                        Console.Write("Ingrese codigo IATA (3 letras): ");
                        string iataCode = Console.ReadLine()!;

                        Console.Write("Ingrese codigo ICAO (4 letras, opcional): ");
                        string? icaoCode = Console.ReadLine();

                        Console.Write("Ingrese city_id: ");
                        int cityId = int.Parse(Console.ReadLine()!);

                        await _create.ExecuteAsync(name, iataCode, icaoCode, cityId);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var cityMap = await GetCityDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, cityMap));
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

                        var cityMapById = await GetCityDisplayMapAsync();
                        Console.WriteLine(Format(byId, cityMapById));
                        break;

                    case 3:
                        Console.Write("Ingrese nombre: ");
                        string searchName = Console.ReadLine()!;

                        var byName = await _getByName.ExecuteAsync(searchName);
                        if (byName is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var cityMapByName = await GetCityDisplayMapAsync();
                        Console.WriteLine(Format(byName, cityMapByName));
                        break;

                    case 4:
                        await PrintCitiesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo nombre: ");
                        string newName = Console.ReadLine()!;

                        Console.Write("Ingrese nuevo codigo IATA (3 letras): ");
                        string newIataCode = Console.ReadLine()!;

                        Console.Write("Ingrese nuevo codigo ICAO (4 letras, opcional): ");
                        string? newIcaoCode = Console.ReadLine();

                        Console.Write("Ingrese nuevo city_id: ");
                        int newCityId = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newName, newIataCode, newIcaoCode, newCityId);
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

    private async Task PrintCitiesAsync()
    {
        Console.WriteLine("Ciudades disponibles:");
        var cities = await _getAllCities.ExecuteAsync();
        foreach (var city in cities)
            Console.WriteLine($"{city.Id.Value} - {city.Name.Value}");
    }

    private async Task<Dictionary<int, string>> GetCityDisplayMapAsync()
    {
        var cities = await _getAllCities.ExecuteAsync();
        return cities.ToDictionary(c => c.Id.Value, c => c.Name.Value);
    }

    private static string Format(Airport item, Dictionary<int, string> cityMap)
    {
        string cityDisplay = GetDisplay(cityMap, item.CityId.Value);
        string icaoDisplay = item.IcaoCode?.Value ?? "N/A";
        return $"{item.Id.Value} - {item.Name.Value} - IATA={item.IataCode.Value} - ICAO={icaoDisplay} - city={cityDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}

