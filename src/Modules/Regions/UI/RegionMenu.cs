// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Regions\UI\RegionMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Countries.Application.UseCases;
using GestionAerolineas.src.Modules.Regions.Application.UseCases;
using GestionAerolineas.src.Modules.Regions.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.Regions.UI;

public class RegionMenu
{
    private readonly CreateRegionUseCase _create;
    private readonly GetAllRegionsUseCase _getAll;
    private readonly GetRegionByIdUseCase _getById;
    private readonly GetRegionByNameUseCase _getByName;
    private readonly UpdateRegionUseCase _update;
    private readonly DeleteRegionUseCase _delete;

    private readonly GetAllCountriesUseCase _getAllCountries;

    public RegionMenu(
        CreateRegionUseCase create,
        GetAllRegionsUseCase getAll,
        GetRegionByIdUseCase getById,
        GetRegionByNameUseCase getByName,
        UpdateRegionUseCase update,
        DeleteRegionUseCase delete,
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
            "Crear region",
            "Listar regions",
            "Get region by ID",
            "Get region by name",
            "Actualizar region",
            "Eliminar region",
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

                        Console.Write("Ingrese tipo: ");
                        string type = Console.ReadLine()!;

                        Console.Write("Ingrese pais_id: ");
                        int countryId = int.Parse(Console.ReadLine()!);

                        await _create.ExecuteAsync(name, type, countryId);
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

                        Console.Write("Ingrese nuevo tipo: ");
                        string newType = Console.ReadLine()!;

                        Console.Write("Ingrese nuevo pais_id: ");
                        int newCountryId = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newName, newType, newCountryId);
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
        Console.WriteLine("PaÃ­ses disponibles:");
        var countries = await _getAllCountries.ExecuteAsync();
        foreach (var c in countries)
            Console.WriteLine($"{c.Id.Value} - {c.Name.Value} - ISO={c.IsoCode.Value}");
    }

    private async Task<Dictionary<int, string>> GetCountryDisplayMapAsync()
    {
        var countries = await _getAllCountries.ExecuteAsync();
        return countries.ToDictionary(c => c.Id.Value, c => c.Name.Value);
    }

    private static string Format(Region item, Dictionary<int, string> countryMap)
    {
        string countryDisplay = GetDisplay(countryMap, item.CountryId.Value);
        return $"{item.Id.Value} - {item.Name.Value} - tipo={item.Type.Value} - paÃ­s={countryDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}


