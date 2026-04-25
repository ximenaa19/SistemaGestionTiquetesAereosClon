// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStatusTransitions\UI\FlightStatusTransitionMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStates.Application.UseCases;
using GestionAerolineas.src.Modules.FlightStatusTransitions.Application.UseCases;

namespace GestionAerolineas.src.Modules.FlightStatusTransitions.UI;

public class FlightStatusTransitionMenu
{
    private readonly CreateFlightStatusTransitionUseCase _create;
    private readonly GetAllFlightStatusTransitionsUseCase _getAll;
    private readonly GetFlightStatusTransitionByIdUseCase _getById;
    private readonly GetFlightStatusTransitionByPairUseCase _getByPair;
    private readonly UpdateFlightStatusTransitionUseCase _update;
    private readonly DeleteFlightStatusTransitionUseCase _delete;

    private readonly GetAllFlightStatesUseCase _getAllFlightStates;
    private readonly GetFlightStateByNameUseCase _getFlightStateByName;

    public FlightStatusTransitionMenu(
        CreateFlightStatusTransitionUseCase create,
        GetAllFlightStatusTransitionsUseCase getAll,
        GetFlightStatusTransitionByIdUseCase getById,
        GetFlightStatusTransitionByPairUseCase getByPair,
        UpdateFlightStatusTransitionUseCase update,
        DeleteFlightStatusTransitionUseCase delete,
        GetAllFlightStatesUseCase getAllFlightStates,
        GetFlightStateByNameUseCase getFlightStateByName)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByPair = getByPair;
        _update = update;
        _delete = delete;
        _getAllFlightStates = getAllFlightStates;
        _getFlightStateByName = getFlightStateByName;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear flight status transition",
            "Listar flight status transitions",
            "Get transition by ID",
            "Get transition by origin/destination (IDs)",
            "Get transition by origin/destination (Names)",
            "Actualizar transition",
            "Eliminar transition",
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
                        Console.Write("Ingrese el ID del estado ORIGEN: ");
                        int originId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el ID del estado DESTINO: ");
                        int destinationId = int.Parse(Console.ReadLine()!);

                        await _create.ExecuteAsync(originId, destinationId);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var stateMap = await GetStateDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine($"{item.Id.Value} - origen={GetDisplay(stateMap, item.OriginStateId.Value)} -> destino={GetDisplay(stateMap, item.DestinationStateId.Value)}");
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);

                        var result = await _getById.ExecuteAsync(searchId);
                        if (result is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var stateMapById = await GetStateDisplayMapAsync();
                        Console.WriteLine($"{result.Id.Value} - origen={GetDisplay(stateMapById, result.OriginStateId.Value)} -> destino={GetDisplay(stateMapById, result.DestinationStateId.Value)}");
                        break;

                    case 3:
                        Console.WriteLine("Estados de vuelo disponibles:");
                        var states = await _getAllFlightStates.ExecuteAsync();
                        foreach (var s in states)
                            Console.WriteLine($"{s.Id.Value} - {s.Name.Value}");

                        Console.Write("\nIngrese el ID del estado ORIGEN: ");
                        int searchOriginId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el ID del estado DESTINO: ");
                        int searchDestinationId = int.Parse(Console.ReadLine()!);

                        var resultByPair = await _getByPair.ExecuteAsync(searchOriginId, searchDestinationId);
                        if (resultByPair is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var stateMapByPair = await GetStateDisplayMapAsync();
                        Console.WriteLine($"{resultByPair.Id.Value} - origen={GetDisplay(stateMapByPair, resultByPair.OriginStateId.Value)} -> destino={GetDisplay(stateMapByPair, resultByPair.DestinationStateId.Value)}");
                        break;

                    case 4:
                        Console.Write("Ingrese el nombre del estado ORIGEN: ");
                        string originName = Console.ReadLine()!;

                        Console.Write("Ingrese el nombre del estado DESTINO: ");
                        string destinationName = Console.ReadLine()!;

                        var origin = await _getFlightStateByName.ExecuteAsync(originName);
                        var destination = await _getFlightStateByName.ExecuteAsync(destinationName);

                        if (origin is null || destination is null)
                        {
                            Console.WriteLine("No encontrado (origen o destino invÃ¡lido)");
                            break;
                        }

                        var resultByPairName = await _getByPair.ExecuteAsync(origin.Id.Value, destination.Id.Value);
                        if (resultByPairName is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var stateMapByPairName = await GetStateDisplayMapAsync();
                        Console.WriteLine($"{resultByPairName.Id.Value} - origen={GetDisplay(stateMapByPairName, resultByPairName.OriginStateId.Value)} -> destino={GetDisplay(stateMapByPairName, resultByPairName.DestinationStateId.Value)}");
                        break;

                    case 5:
                        Console.Write("Ingrese el ID de la transiciÃ³n: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el nuevo ID del estado ORIGEN: ");
                        int newOriginId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el nuevo ID del estado DESTINO: ");
                        int newDestinationId = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newOriginId, newDestinationId);
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

    private async Task<Dictionary<int, string>> GetStateDisplayMapAsync()
    {
        var states = await _getAllFlightStates.ExecuteAsync();
        return states.ToDictionary(s => s.Id.Value, s => s.Name.Value);
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}


