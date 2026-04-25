// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\UI\AircraftMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Application.UseCases;
using GestionAerolineas.src.Modules.Aircraft.Domain.Aggregate;
using GestionAerolineas.src.Modules.AircraftModels.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;

namespace GestionAerolineas.src.Modules.Aircraft.UI;

public class AircraftMenu
{
    private readonly CreateAircraftUseCase _create;
    private readonly GetAllAircraftUseCase _getAll;
    private readonly GetAircraftByIdUseCase _getById;
    private readonly GetAircraftByRegistrationUseCase _getByRegistration;
    private readonly UpdateAircraftUseCase _update;
    private readonly DeleteAircraftUseCase _delete;

    private readonly GetAllAircraftModelsUseCase _getAllModels;
    private readonly GetAllAirlinesUseCase _getAllAirlines;

    public AircraftMenu(
        CreateAircraftUseCase create,
        GetAllAircraftUseCase getAll,
        GetAircraftByIdUseCase getById,
        GetAircraftByRegistrationUseCase getByRegistration,
        UpdateAircraftUseCase update,
        DeleteAircraftUseCase delete,
        GetAllAircraftModelsUseCase getAllModels,
        GetAllAirlinesUseCase getAllAirlines)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByRegistration = getByRegistration;
        _update = update;
        _delete = delete;
        _getAllModels = getAllModels;
        _getAllAirlines = getAllAirlines;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear aircraft",
            "Listar aircraft",
            "Get aircraft by ID",
            "Get aircraft by registration",
            "Actualizar aircraft",
            "Eliminar aircraft",
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
                        await PrintModelsAsync();
                        await PrintAirlinesAsync();

                        Console.Write("\nIngrese modelo_id: ");
                        int modelId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aerolinea_id: ");
                        int airlineId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese matricula: ");
                        string registration = Console.ReadLine()!;

                        Console.Write("Ingrese fecha_fabricacion (yyyy-MM-dd) [opcional]: ");
                        var manufactureDateInput = Console.ReadLine();
                        DateTime? manufactureDate = string.IsNullOrWhiteSpace(manufactureDateInput)
                            ? null
                            : DateTime.Parse(manufactureDateInput!);

                        Console.Write("Ingrese activa (true/false) [default=true]: ");
                        var activeInput = Console.ReadLine();
                        bool isActive = string.IsNullOrWhiteSpace(activeInput) ? true : bool.Parse(activeInput);

                        await _create.ExecuteAsync(modelId, airlineId, registration, manufactureDate, isActive);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var modelMap = await GetModelDisplayMapAsync();
                        var airlineMap = await GetAirlineDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, modelMap, airlineMap));
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

                        var modelMapById = await GetModelDisplayMapAsync();
                        var airlineMapById = await GetAirlineDisplayMapAsync();
                        Console.WriteLine(Format(byId, modelMapById, airlineMapById));
                        break;

                    case 3:
                        Console.Write("Ingrese matricula: ");
                        string searchReg = Console.ReadLine()!;

                        var byReg = await _getByRegistration.ExecuteAsync(searchReg);
                        if (byReg is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var modelMapByReg = await GetModelDisplayMapAsync();
                        var airlineMapByReg = await GetAirlineDisplayMapAsync();
                        Console.WriteLine(Format(byReg, modelMapByReg, airlineMapByReg));
                        break;

                    case 4:
                        await PrintModelsAsync();
                        await PrintAirlinesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese modelo_id: ");
                        int newModelId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aerolinea_id: ");
                        int newAirlineId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese matricula: ");
                        string newRegistration = Console.ReadLine()!;

                        Console.Write("Ingrese fecha_fabricacion (yyyy-MM-dd) [opcional]: ");
                        var newManufactureDateInput = Console.ReadLine();
                        DateTime? newManufactureDate = string.IsNullOrWhiteSpace(newManufactureDateInput)
                            ? null
                            : DateTime.Parse(newManufactureDateInput!);

                        Console.Write("Ingrese activa (true/false): ");
                        bool newIsActive = bool.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newModelId, newAirlineId, newRegistration, newManufactureDate, newIsActive);
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

    private async Task PrintModelsAsync()
    {
        Console.WriteLine("Modelos disponibles:");
        var models = (await _getAllModels.ExecuteAsync()).ToList();
        foreach (var model in models.Take(30))
            Console.WriteLine($"{model.Id.Value} - {model.ModelName.Value} - max={model.MaxCapacity.Value} - manufacturer_id={model.ManufacturerId.Value}");

        if (models.Count > 30)
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

    private async Task<Dictionary<int, string>> GetModelDisplayMapAsync()
    {
        var models = await _getAllModels.ExecuteAsync();
        return models.ToDictionary(m => m.Id.Value, m => m.ModelName.Value);
    }

    private async Task<Dictionary<int, string>> GetAirlineDisplayMapAsync()
    {
        var airlines = await _getAllAirlines.ExecuteAsync();
        return airlines.ToDictionary(a => a.Id.Value, a => a.Name.Value);
    }

    private static string Format(AircraftAggregate item, Dictionary<int, string> modelMap, Dictionary<int, string> airlineMap)
    {
        string modelDisplay = GetDisplay(modelMap, item.ModelId.Value);
        string airlineDisplay = GetDisplay(airlineMap, item.AirlineId.Value);
        var manufactureDateDisplay = item.ManufactureDate.Value?.ToString("yyyy-MM-dd") ?? "NULL";
        var activeDisplay = item.IsActive.Value ? "active" : "inactive";

        return $"{item.Id.Value} - model={modelDisplay} - airline={airlineDisplay} - reg={item.Registration.Value} - manufactured={manufactureDateDisplay} - {activeDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}

