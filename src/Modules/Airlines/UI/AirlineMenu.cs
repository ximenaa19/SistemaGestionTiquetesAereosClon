// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\UI\AirlineMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Domain.Aggregate;
using GestionAerolineas.src.Modules.Countries.Application.UseCases;

namespace GestionAerolineas.src.Modules.Airlines.UI;

public class AirlineMenu
{
    private readonly CreateAirlineUseCase _create;
    private readonly GetAllAirlinesUseCase _getAll;
    private readonly GetAirlineByIdUseCase _getById;
    private readonly GetAirlineByNameUseCase _getByName;
    private readonly UpdateAirlineUseCase _update;
    private readonly DeleteAirlineUseCase _delete;

    private readonly GetAllCountriesUseCase _getAllCountries;

    public AirlineMenu(
        CreateAirlineUseCase create,
        GetAllAirlinesUseCase getAll,
        GetAirlineByIdUseCase getById,
        GetAirlineByNameUseCase getByName,
        UpdateAirlineUseCase update,
        DeleteAirlineUseCase delete,
        GetAllCountriesUseCase getAllCountries)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByName = getByName;
        _update = update;
        _delete = delete;
        _getAllCountries = getAllCountries;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear airline",
            "Listar airlines",
            "Get airline by ID",
            "Get airline by name",
            "Actualizar airline",
            "Eliminar airline",
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
                        await PrintCountriesAsync();

                        Console.Write("\nIngrese nombre: ");
                        string name = Console.ReadLine()!;

                        Console.Write("Ingrese codigo_iata: ");
                        string iataCode = Console.ReadLine()!;

                        Console.Write("Ingrese pais_origen_id: ");
                        int originCountryId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese activa (true/false) [default=true]: ");
                        var activeInput = Console.ReadLine();
                        bool isActive = string.IsNullOrWhiteSpace(activeInput) ? true : bool.Parse(activeInput);

                        await _create.ExecuteAsync(name, iataCode, originCountryId, isActive);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var countryMap = await GetCountryDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, countryMap));
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

                        var countryMapById = await GetCountryDisplayMapAsync();
                        Console.WriteLine(Format(byId, countryMapById));
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

                        var countryMapByName = await GetCountryDisplayMapAsync();
                        Console.WriteLine(Format(byName, countryMapByName));
                        break;

                    case 4:
                        await PrintCountriesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo nombre: ");
                        string newName = Console.ReadLine()!;

                        Console.Write("Ingrese nuevo codigo_iata: ");
                        string newIataCode = Console.ReadLine()!;

                        Console.Write("Ingrese nuevo pais_origen_id: ");
                        int newOriginCountryId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese activa (true/false): ");
                        bool newIsActive = bool.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newName, newIataCode, newOriginCountryId, newIsActive);
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

    private async Task PrintCountriesAsync()
    {
        Console.WriteLine("Paises disponibles:");
        var countries = await _getAllCountries.ExecuteAsync();
        foreach (var country in countries)
            Console.WriteLine($"{country.Id.Value} - {country.Name.Value} - iso={country.IsoCode.Value}");
    }

    private async Task<Dictionary<int, string>> GetCountryDisplayMapAsync()
    {
        var countries = await _getAllCountries.ExecuteAsync();
        return countries.ToDictionary(c => c.Id.Value, c => c.Name.Value);
    }

    private static string Format(Airline item, Dictionary<int, string> countryMap)
    {
        string countryDisplay = GetDisplay(countryMap, item.OriginCountryId.Value);
        var activeDisplay = item.IsActive.Value ? "active" : "inactive";
        return $"{item.Id.Value} - {item.Name.Value} - iata={item.IataCode.Value} - origin_country={countryDisplay} - {activeDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}


