// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Addresses\UI\AddressMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Application.UseCases;
using GestionAerolineas.src.Modules.Addresses.Domain.Aggregate;
using GestionAerolineas.src.Modules.Cities.Application.UseCases;
using GestionAerolineas.src.Modules.Regions.Application.UseCases;
using GestionAerolineas.src.Modules.RoadTypes.Application.UseCases;

namespace GestionAerolineas.src.Modules.Addresses.UI;

public class AddressMenu
{
    private readonly CreateAddressUseCase _create;
    private readonly GetAllAddressesUseCase _getAll;
    private readonly GetAddressByIdUseCase _getById;
    private readonly UpdateAddressUseCase _update;
    private readonly DeleteAddressUseCase _delete;

    private readonly GetAllRoadTypesUseCase _getAllRoadTypes;
    private readonly GetAllCitiesUseCase _getAllCities;
    private readonly GetAllRegionsUseCase _getAllRegions;

    public AddressMenu(
        CreateAddressUseCase create,
        GetAllAddressesUseCase getAll,
        GetAddressByIdUseCase getById,
        UpdateAddressUseCase update,
        DeleteAddressUseCase delete,
        GetAllRoadTypesUseCase getAllRoadTypes,
        GetAllCitiesUseCase getAllCities,
        GetAllRegionsUseCase getAllRegions)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _update = update;
        _delete = delete;
        _getAllRoadTypes = getAllRoadTypes;
        _getAllCities = getAllCities;
        _getAllRegions = getAllRegions;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear address",
            "Listar addresses",
            "Get address by ID",
            "Actualizar address",
            "Eliminar address",
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
                        await PrintRoadTypesAsync();
                        await PrintCitiesAsync();

                        Console.Write("\nIngrese tipo_via_id: ");
                        int roadTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nombre_via: ");
                        string roadName = Console.ReadLine()!;

                        Console.Write("Ingrese numero (opcional): ");
                        string? number = Console.ReadLine();

                        Console.Write("Ingrese complemento (opcional): ");
                        string? complement = Console.ReadLine();

                        Console.Write("Ingrese ciudad_id: ");
                        int cityId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese codigo_postal (opcional): ");
                        string? postal = Console.ReadLine();

                        await _create.ExecuteAsync(roadTypeId, roadName, number, complement, cityId, postal);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var roadTypeMap = await GetRoadTypeDisplayMapAsync();
                        var cityMap = await GetCityDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, roadTypeMap, cityMap));
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

                        var roadTypeMapById = await GetRoadTypeDisplayMapAsync();
                        var cityMapById = await GetCityDisplayMapAsync();
                        Console.WriteLine(Format(byId, roadTypeMapById, cityMapById));
                        break;

                    case 3:
                        await PrintRoadTypesAsync();
                        await PrintCitiesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo tipo_via_id: ");
                        int newRoadTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo nombre_via: ");
                        string newRoadName = Console.ReadLine()!;

                        Console.Write("Ingrese nuevo numero (opcional): ");
                        string? newNumber = Console.ReadLine();

                        Console.Write("Ingrese nuevo complemento (opcional): ");
                        string? newComplement = Console.ReadLine();

                        Console.Write("Ingrese nueva ciudad_id: ");
                        int newCityId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo codigo_postal (opcional): ");
                        string? newPostal = Console.ReadLine();

                        await _update.ExecuteAsync(updateId, newRoadTypeId, newRoadName, newNumber, newComplement, newCityId, newPostal);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 4:
                        Console.Write("Ingrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("âœ” Eliminado");
                        break;

                    case 5:
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

    private async Task PrintRoadTypesAsync()
    {
        Console.WriteLine("Tipos de vÃ­a disponibles:");
        var roadTypes = await _getAllRoadTypes.ExecuteAsync();
        foreach (var rt in roadTypes)
            Console.WriteLine($"{rt.Id.Value} - {rt.Name.Value}");
        Console.WriteLine();
    }

    private async Task PrintCitiesAsync()
    {
        Console.WriteLine("Ciudades disponibles:");
        var regionMap = await GetRegionDisplayMapAsync();
        var cities = await _getAllCities.ExecuteAsync();
        foreach (var c in cities)
            Console.WriteLine($"{c.Id.Value} - {c.Name.Value} - region={GetDisplay(regionMap, c.RegionId.Value)}");
        Console.WriteLine();
    }

    private async Task<Dictionary<int, string>> GetRoadTypeDisplayMapAsync()
    {
        var roadTypes = await _getAllRoadTypes.ExecuteAsync();
        return roadTypes.ToDictionary(rt => rt.Id.Value, rt => rt.Name.Value);
    }

    private async Task<Dictionary<int, string>> GetRegionDisplayMapAsync()
    {
        var regions = await _getAllRegions.ExecuteAsync();
        return regions.ToDictionary(r => r.Id.Value, r => r.Name.Value);
    }

    private async Task<Dictionary<int, string>> GetCityDisplayMapAsync()
    {
        var regionMap = await GetRegionDisplayMapAsync();
        var cities = await _getAllCities.ExecuteAsync();
        return cities.ToDictionary(
            c => c.Id.Value,
            c => $"{c.Name.Value} ({GetDisplay(regionMap, c.RegionId.Value)})");
    }

    private static string Format(Address item, Dictionary<int, string> roadTypeMap, Dictionary<int, string> cityMap)
    {
        string roadTypeDisplay = GetDisplay(roadTypeMap, item.RoadTypeId.Value);
        string cityDisplay = GetDisplay(cityMap, item.CityId.Value);

        string number = string.IsNullOrWhiteSpace(item.Number.Value) ? "" : $" #{item.Number.Value}";
        string complement = string.IsNullOrWhiteSpace(item.Complement.Value) ? "" : $" - {item.Complement.Value}";
        string postal = string.IsNullOrWhiteSpace(item.PostalCode.Value) ? "null" : item.PostalCode.Value!;

        return $"{item.Id.Value} - {roadTypeDisplay} {item.RoadName.Value}{number}{complement} - ciudad={cityDisplay} - postal={postal}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}


