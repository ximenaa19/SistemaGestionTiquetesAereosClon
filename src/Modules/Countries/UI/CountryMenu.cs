// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Countries\UI\CountryMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Continents.Application.UseCases;
using GestionAerolineas.src.Modules.Countries.Application.UseCases;

namespace GestionAerolineas.src.Modules.Countries.UI;

public class CountryMenu
{
    private readonly CreateCountryUseCase _create;
    private readonly GetAllCountriesUseCase _getAll;
    private readonly GetCountryByIdUseCase _getById;
    private readonly GetCountryByNameUseCase _getByName;
    private readonly GetCountryByIsoCodeUseCase _getByIso;
    private readonly UpdateCountryUseCase _update;
    private readonly DeleteCountryUseCase _delete;

    private readonly GetAllContinentsUseCase _getAllContinents;

    public CountryMenu(
        CreateCountryUseCase create,
        GetAllCountriesUseCase getAll,
        GetCountryByIdUseCase getById,
        GetCountryByNameUseCase getByName,
        GetCountryByIsoCodeUseCase getByIso,
        UpdateCountryUseCase update,
        DeleteCountryUseCase delete,
        GetAllContinentsUseCase getAllContinents)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByName = getByName;
        _getByIso = getByIso;
        _update = update;
        _delete = delete;
        _getAllContinents = getAllContinents;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear country",
            "Listar countries",
            "Get country by ID",
            "Get country by name",
            "Get country by ISO code",
            "Actualizar country",
            "Eliminar country",
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
                        await PrintContinentsAsync();

                        Console.Write("\nIngrese el nombre: ");
                        string name = Console.ReadLine()!;

                        Console.Write("Ingrese el cÃ³digo ISO (3 letras): ");
                        string isoCode = Console.ReadLine()!;

                        Console.Write("Ingrese el ID del continente: ");
                        int continentId = int.Parse(Console.ReadLine()!);

                        await _create.ExecuteAsync(name, isoCode, continentId);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var continentMap = await GetContinentDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine($"{item.Id.Value} - {item.Name.Value} - ISO={item.IsoCode.Value} - continente={GetDisplay(continentMap, item.ContinentId.Value)}");
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

                        var continentMapById = await GetContinentDisplayMapAsync();
                        Console.WriteLine($"{byId.Id.Value} - {byId.Name.Value} - ISO={byId.IsoCode.Value} - continente={GetDisplay(continentMapById, byId.ContinentId.Value)}");
                        break;

                    case 3:
                        Console.Write("Ingrese el nombre: ");
                        string searchName = Console.ReadLine()!;

                        var byName = await _getByName.ExecuteAsync(searchName);
                        if (byName is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var continentMapByName = await GetContinentDisplayMapAsync();
                        Console.WriteLine($"{byName.Id.Value} - {byName.Name.Value} - ISO={byName.IsoCode.Value} - continente={GetDisplay(continentMapByName, byName.ContinentId.Value)}");
                        break;

                    case 4:
                        Console.Write("Ingrese el cÃ³digo ISO: ");
                        string searchIso = Console.ReadLine()!;

                        var byIso = await _getByIso.ExecuteAsync(searchIso);
                        if (byIso is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var continentMapByIso = await GetContinentDisplayMapAsync();
                        Console.WriteLine($"{byIso.Id.Value} - {byIso.Name.Value} - ISO={byIso.IsoCode.Value} - continente={GetDisplay(continentMapByIso, byIso.ContinentId.Value)}");
                        break;

                    case 5:
                        await PrintContinentsAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el nuevo nombre: ");
                        string newName = Console.ReadLine()!;

                        Console.Write("Ingrese el nuevo cÃ³digo ISO (3 letras): ");
                        string newIso = Console.ReadLine()!;

                        Console.Write("Ingrese el nuevo ID del continente: ");
                        int newContinentId = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newName, newIso, newContinentId);
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

    private async Task PrintContinentsAsync()
    {
        Console.WriteLine("Continentes disponibles:");
        var continents = await _getAllContinents.ExecuteAsync();
        foreach (var c in continents)
            Console.WriteLine($"{c.Id.Value} - {c.Name.Value}");
    }

    private async Task<Dictionary<int, string>> GetContinentDisplayMapAsync()
    {
        var continents = await _getAllContinents.ExecuteAsync();
        return continents.ToDictionary(c => c.Id.Value, c => c.Name.Value);
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}


