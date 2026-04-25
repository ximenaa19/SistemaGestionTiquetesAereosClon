// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\UI\CabinConfigurationMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Application.UseCases;
using GestionAerolineas.src.Modules.Aircraft.Domain.Aggregate;
using GestionAerolineas.src.Modules.CabinConfiguration.Application.UseCases;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Aggregate;
using GestionAerolineas.src.Modules.CabinTypes.Application.UseCases;
using GestionAerolineas.src.Modules.CabinTypes.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.CabinConfiguration.UI;

public class CabinConfigurationMenu
{
    private readonly CreateCabinConfigurationUseCase _create;
    private readonly GetAllCabinConfigurationsUseCase _getAll;
    private readonly GetCabinConfigurationByIdUseCase _getById;
    private readonly GetCabinConfigurationsByAircraftIdUseCase _getByAircraftId;
    private readonly GetCabinConfigurationByAircraftAndCabinTypeUseCase _getByAircraftAndCabinType;
    private readonly UpdateCabinConfigurationUseCase _update;
    private readonly DeleteCabinConfigurationUseCase _delete;

    private readonly GetAllAircraftUseCase _getAllAircraft;
    private readonly GetAllCabinTypeUseCase _getAllCabinTypes;

    public CabinConfigurationMenu(
        CreateCabinConfigurationUseCase create,
        GetAllCabinConfigurationsUseCase getAll,
        GetCabinConfigurationByIdUseCase getById,
        GetCabinConfigurationsByAircraftIdUseCase getByAircraftId,
        GetCabinConfigurationByAircraftAndCabinTypeUseCase getByAircraftAndCabinType,
        UpdateCabinConfigurationUseCase update,
        DeleteCabinConfigurationUseCase delete,
        GetAllAircraftUseCase getAllAircraft,
        GetAllCabinTypeUseCase getAllCabinTypes)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByAircraftId = getByAircraftId;
        _getByAircraftAndCabinType = getByAircraftAndCabinType;
        _update = update;
        _delete = delete;
        _getAllAircraft = getAllAircraft;
        _getAllCabinTypes = getAllCabinTypes;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear cabin configuration",
            "Listar cabin configurations",
            "Get cabin configuration by ID",
            "Listar configuraciones de cabina por ID de aeronave",
            "Get cabin configuration by aircraft+cabin type",
            "Actualizar cabin configuration",
            "Eliminar cabin configuration",
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
                        await PrintAircraftAsync();
                        await PrintCabinTypesAsync();

                        Console.Write("\nIngrese aeronave_id: ");
                        int aircraftId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese tipo_cabina_id: ");
                        int cabinTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fila_inicio: ");
                        int startRow = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fila_fin: ");
                        int endRow = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese asientos_por_fila: ");
                        int seatsPerRow = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese letras_asientos (ej: ABCDEF): ");
                        string seatLetters = Console.ReadLine()!;

                        await _create.ExecuteAsync(aircraftId, cabinTypeId, startRow, endRow, seatsPerRow, seatLetters);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var aircraftMap = await GetAircraftDisplayMapAsync();
                        var cabinTypeMap = await GetCabinTypeDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, aircraftMap, cabinTypeMap));
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

                        var aircraftMapById = await GetAircraftDisplayMapAsync();
                        var cabinTypeMapById = await GetCabinTypeDisplayMapAsync();
                        Console.WriteLine(Format(byId, aircraftMapById, cabinTypeMapById));
                        break;

                    case 3:
                        await PrintAircraftAsync();

                        Console.Write("\nIngrese aeronave_id: ");
                        int searchAircraftId = int.Parse(Console.ReadLine()!);

                        var byAircraft = await _getByAircraftId.ExecuteAsync(searchAircraftId);
                        var byAircraftList = byAircraft.ToList();
                        if (byAircraftList.Count == 0)
                        {
                            Console.WriteLine("No hay registros");
                            break;
                        }

                        var aircraftMapByAircraft = await GetAircraftDisplayMapAsync();
                        var cabinTypeMapByAircraft = await GetCabinTypeDisplayMapAsync();
                        foreach (var item in byAircraftList)
                            Console.WriteLine(Format(item, aircraftMapByAircraft, cabinTypeMapByAircraft));
                        break;

                    case 4:
                        await PrintAircraftAsync();
                        await PrintCabinTypesAsync();

                        Console.Write("\nIngrese aeronave_id: ");
                        int pairAircraftId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese tipo_cabina_id: ");
                        int pairCabinTypeId = int.Parse(Console.ReadLine()!);

                        var byPair = await _getByAircraftAndCabinType.ExecuteAsync(pairAircraftId, pairCabinTypeId);
                        if (byPair is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var aircraftMapByPair = await GetAircraftDisplayMapAsync();
                        var cabinTypeMapByPair = await GetCabinTypeDisplayMapAsync();
                        Console.WriteLine(Format(byPair, aircraftMapByPair, cabinTypeMapByPair));
                        break;

                    case 5:
                        await PrintAircraftAsync();
                        await PrintCabinTypesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aeronave_id: ");
                        int newAircraftId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese tipo_cabina_id: ");
                        int newCabinTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fila_inicio: ");
                        int newStartRow = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fila_fin: ");
                        int newEndRow = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese asientos_por_fila: ");
                        int newSeatsPerRow = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese letras_asientos (ej: ABCDEF): ");
                        string newSeatLetters = Console.ReadLine()!;

                        await _update.ExecuteAsync(updateId, newAircraftId, newCabinTypeId, newStartRow, newEndRow, newSeatsPerRow, newSeatLetters);
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

    private async Task PrintAircraftAsync()
    {
        Console.WriteLine("Aeronaves disponibles:");
        var aircraft = (await _getAllAircraft.ExecuteAsync()).ToList();

        foreach (var item in aircraft.Take(30))
            Console.WriteLine($"{item.Id.Value} - reg={item.Registration.Value} - model_id={item.ModelId.Value} - airline_id={item.AirlineId.Value}");

        if (aircraft.Count > 30)
            Console.WriteLine("(Mostrando solo las primeras 30)");
    }

    private async Task PrintCabinTypesAsync()
    {
        Console.WriteLine("\nTipos de cabina disponibles:");
        var cabinTypes = (await _getAllCabinTypes.ExecuteAsync()).ToList();

        foreach (var item in cabinTypes.Take(30))
            Console.WriteLine($"{item.Id.Value} - {item.Name.Value}");

        if (cabinTypes.Count > 30)
            Console.WriteLine("(Mostrando solo los primeros 30)");
    }

    private async Task<Dictionary<int, string>> GetAircraftDisplayMapAsync()
    {
        var aircraft = await _getAllAircraft.ExecuteAsync();
        return aircraft.ToDictionary(a => a.Id.Value, a => a.Registration.Value);
    }

    private async Task<Dictionary<int, string>> GetCabinTypeDisplayMapAsync()
    {
        var cabinTypes = await _getAllCabinTypes.ExecuteAsync();
        return cabinTypes.ToDictionary(c => c.Id.Value, c => c.Name.Value);
    }

    private static string Format(
        CabinConfigurationAggregate item,
        Dictionary<int, string> aircraftMap,
        Dictionary<int, string> cabinTypeMap)
    {
        string aircraftDisplay = GetDisplay(aircraftMap, item.AircraftId.Value);
        string cabinTypeDisplay = GetDisplay(cabinTypeMap, item.CabinTypeId.Value);

        return $"{item.Id.Value} - aircraft={aircraftDisplay} [{item.AircraftId.Value}] - cabinType={cabinTypeDisplay} [{item.CabinTypeId.Value}] - rows={item.StartRow.Value}-{item.EndRow.Value} - seatsPerRow={item.SeatsPerRow.Value} - letters={item.SeatLetters.Value}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? display : $"#{id}";
    }
}

