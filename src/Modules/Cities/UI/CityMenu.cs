// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Cities\UI\CityMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Cities.Application.UseCases;
using GestionAerolineas.src.Modules.Cities.Domain.Aggregate;
using GestionAerolineas.src.Modules.Regions.Application.UseCases;

namespace GestionAerolineas.src.Modules.Cities.UI;

public class CityMenu
{
    private readonly CreateCityUseCase _create;
    private readonly GetAllCitiesUseCase _getAll;
    private readonly GetCityByIdUseCase _getById;
    private readonly GetCityByNameUseCase _getByName;
    private readonly UpdateCityUseCase _update;
    private readonly DeleteCityUseCase _delete;

    private readonly GetAllRegionsUseCase _getAllRegions;

    public CityMenu(
        CreateCityUseCase create,
        GetAllCitiesUseCase getAll,
        GetCityByIdUseCase getById,
        GetCityByNameUseCase getByName,
        UpdateCityUseCase update,
        DeleteCityUseCase delete,
        GetAllRegionsUseCase getAllRegions)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByName = getByName;
        _update = update;
        _delete = delete;
        _getAllRegions = getAllRegions;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear city",
            "Listar cities",
            "Get city by ID",
            "Get city by name",
            "Actualizar city",
            "Eliminar city",
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
                        await PrintRegionsAsync();

                        Console.Write("\nIngrese nombre: ");
                        string name = Console.ReadLine()!;

                        Console.Write("Ingrese region_id: ");
                        int regionId = int.Parse(Console.ReadLine()!);

                        await _create.ExecuteAsync(name, regionId);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var regionMap = await GetRegionDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, regionMap));
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

                        var regionMapById = await GetRegionDisplayMapAsync();
                        Console.WriteLine(Format(byId, regionMapById));
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

                        var regionMapByName = await GetRegionDisplayMapAsync();
                        Console.WriteLine(Format(byName, regionMapByName));
                        break;

                    case 4:
                        await PrintRegionsAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo nombre: ");
                        string newName = Console.ReadLine()!;

                        Console.Write("Ingrese nuevo region_id: ");
                        int newRegionId = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newName, newRegionId);
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

    private async Task PrintRegionsAsync()
    {
        Console.WriteLine("Regiones disponibles:");
        var regions = await _getAllRegions.ExecuteAsync();
        foreach (var region in regions)
            Console.WriteLine($"{region.Id.Value} - {region.Name.Value} - tipo={region.Type.Value}");
    }

    private async Task<Dictionary<int, string>> GetRegionDisplayMapAsync()
    {
        var regions = await _getAllRegions.ExecuteAsync();
        return regions.ToDictionary(r => r.Id.Value, r => r.Name.Value);
    }

    private static string Format(City item, Dictionary<int, string> regionMap)
    {
        string regionDisplay = GetDisplay(regionMap, item.RegionId.Value);
        return $"{item.Id.Value} - {item.Name.Value} - region={regionDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}

