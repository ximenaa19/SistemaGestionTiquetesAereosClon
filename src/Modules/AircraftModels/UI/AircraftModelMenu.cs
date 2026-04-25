// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\UI\AircraftModelMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Globalization;
using GestionAerolineas.src.Modules.AircraftManufacturers.Application.UseCases;
using GestionAerolineas.src.Modules.AircraftModels.Application.UseCases;
using GestionAerolineas.src.Modules.AircraftModels.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.AircraftModels.UI;

public class AircraftModelMenu
{
    private readonly CreateAircraftModelUseCase _create;
    private readonly GetAllAircraftModelsUseCase _getAll;
    private readonly GetAircraftModelByIdUseCase _getById;
    private readonly GetAircraftModelByNameUseCase _getByName;
    private readonly UpdateAircraftModelUseCase _update;
    private readonly DeleteAircraftModelUseCase _delete;

    private readonly GetAllAircraftManufacturersUseCase _getAllManufacturers;

    public AircraftModelMenu(
        CreateAircraftModelUseCase create,
        GetAllAircraftModelsUseCase getAll,
        GetAircraftModelByIdUseCase getById,
        GetAircraftModelByNameUseCase getByName,
        UpdateAircraftModelUseCase update,
        DeleteAircraftModelUseCase delete,
        GetAllAircraftManufacturersUseCase getAllManufacturers)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByName = getByName;
        _update = update;
        _delete = delete;
        _getAllManufacturers = getAllManufacturers;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear aircraft model",
            "Listar aircraft models",
            "Get aircraft model by ID",
            "Get aircraft model by name",
            "Actualizar aircraft model",
            "Eliminar aircraft model",
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
                        await PrintManufacturersAsync();

                        Console.Write("\nIngrese fabricante_id: ");
                        int manufacturerId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nombre_modelo: ");
                        string modelName = Console.ReadLine()!;

                        Console.Write("Ingrese capacidad_maxima: ");
                        int maxCapacity = int.Parse(Console.ReadLine()!);

                        decimal? mtow = ReadNullableDecimal("Ingrese peso_max_despegue_kg (opcional): ");
                        decimal? fuel = ReadNullableDecimal("Ingrese consumo_combustible_kg_h (opcional): ");
                        int? speed = ReadNullableInt("Ingrese velocidad_crucero_kmh (opcional): ");
                        int? altitude = ReadNullableInt("Ingrese altitud_crucero_ft (opcional): ");

                        await _create.ExecuteAsync(manufacturerId, modelName, maxCapacity, mtow, fuel, speed, altitude);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var manufacturerMap = await GetManufacturerDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, manufacturerMap));
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

                        var manufacturerMapById = await GetManufacturerDisplayMapAsync();
                        Console.WriteLine(Format(byId, manufacturerMapById));
                        break;

                    case 3:
                        Console.Write("Ingrese el nombre_modelo: ");
                        string searchName = Console.ReadLine()!;

                        var byName = await _getByName.ExecuteAsync(searchName);
                        if (byName is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var manufacturerMapByName = await GetManufacturerDisplayMapAsync();
                        Console.WriteLine(Format(byName, manufacturerMapByName));
                        break;

                    case 4:
                        await PrintManufacturersAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo fabricante_id: ");
                        int newManufacturerId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo nombre_modelo: ");
                        string newModelName = Console.ReadLine()!;

                        Console.Write("Ingrese nueva capacidad_maxima: ");
                        int newMaxCapacity = int.Parse(Console.ReadLine()!);

                        decimal? newMtow = ReadNullableDecimal("Ingrese nuevo peso_max_despegue_kg (opcional): ");
                        decimal? newFuel = ReadNullableDecimal("Ingrese nuevo consumo_combustible_kg_h (opcional): ");
                        int? newSpeed = ReadNullableInt("Ingrese nueva velocidad_crucero_kmh (opcional): ");
                        int? newAltitude = ReadNullableInt("Ingrese nueva altitud_crucero_ft (opcional): ");

                        await _update.ExecuteAsync(updateId, newManufacturerId, newModelName, newMaxCapacity, newMtow, newFuel, newSpeed, newAltitude);
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

    private async Task PrintManufacturersAsync()
    {
        Console.WriteLine("Fabricantes disponibles:");
        var manufacturers = await _getAllManufacturers.ExecuteAsync();
        foreach (var m in manufacturers)
            Console.WriteLine($"{m.Id.Value} - {m.Name.Value}");
    }

    private async Task<Dictionary<int, string>> GetManufacturerDisplayMapAsync()
    {
        var manufacturers = await _getAllManufacturers.ExecuteAsync();
        return manufacturers.ToDictionary(m => m.Id.Value, m => m.Name.Value);
    }

    private static string Format(AircraftModel item, Dictionary<int, string> manufacturerMap)
    {
        string manufacturerDisplay = GetDisplay(manufacturerMap, item.ManufacturerId.Value);

        return $"{item.Id.Value} - {item.ModelName.Value} - fabricante={manufacturerDisplay} - capacidad={item.MaxCapacity.Value}" +
               $" - mtow_kg={(item.MaxTakeoffWeightKg?.ToString() ?? "null")}" +
               $" - fuel_kg_h={(item.FuelConsumptionKgPerHour?.ToString() ?? "null")}" +
               $" - speed_kmh={(item.CruiseSpeedKmh?.ToString() ?? "null")}" +
               $" - altitude_ft={(item.CruiseAltitudeFt?.ToString() ?? "null")}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }

    private static int? ReadNullableInt(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(input))
            return null;

        if (!int.TryParse(input, out var value))
            throw new Exception("Valor invÃ¡lido");

        return value;
    }

    private static decimal? ReadNullableDecimal(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(input))
            return null;

        if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out var current))
            return current;

        if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var invariant))
            return invariant;

        throw new Exception("Valor invÃ¡lido");
    }
}

