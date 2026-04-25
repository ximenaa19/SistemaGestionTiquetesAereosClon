// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\UI\ReservationStatusTransitionMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Application.UseCases;

namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.UI;

public class ReservationStatusTransitionMenu
{
    private readonly CreateReservationStatusTransitionUseCase _create;
    private readonly GetAllReservationStatusTransitionsUseCase _getAll;
    private readonly GetReservationStatusTransitionByIdUseCase _getById;
    private readonly GetReservationStatusTransitionByPairUseCase _getByPair;
    private readonly UpdateReservationStatusTransitionUseCase _update;
    private readonly DeleteReservationStatusTransitionUseCase _delete;

    private readonly GetAllReservationStatusesUseCase _getAllStatuses;

    public ReservationStatusTransitionMenu(
        CreateReservationStatusTransitionUseCase create,
        GetAllReservationStatusTransitionsUseCase getAll,
        GetReservationStatusTransitionByIdUseCase getById,
        GetReservationStatusTransitionByPairUseCase getByPair,
        UpdateReservationStatusTransitionUseCase update,
        DeleteReservationStatusTransitionUseCase delete,
        GetAllReservationStatusesUseCase getAllStatuses)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByPair = getByPair;
        _update = update;
        _delete = delete;
        _getAllStatuses = getAllStatuses;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear reservation status transition",
            "Listar reservation status transitions",
            "Get transition by ID",
            "Get transition by origin/destination (IDs)",
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
                        var statusMap = await GetStatusDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine($"{item.Id.Value} - origen={GetDisplay(statusMap, item.OriginStatusId.Value)} -> destino={GetDisplay(statusMap, item.DestinationStatusId.Value)}");
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

                        var statusMapById = await GetStatusDisplayMapAsync();
                        Console.WriteLine($"{result.Id.Value} - origen={GetDisplay(statusMapById, result.OriginStatusId.Value)} -> destino={GetDisplay(statusMapById, result.DestinationStatusId.Value)}");
                        break;

                    case 3:
                        Console.Write("Ingrese el ID del estado ORIGEN: ");
                        int searchOriginId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el ID del estado DESTINO: ");
                        int searchDestinationId = int.Parse(Console.ReadLine()!);

                        var resultByPair = await _getByPair.ExecuteAsync(searchOriginId, searchDestinationId);
                        if (resultByPair is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var statusMapByPair = await GetStatusDisplayMapAsync();
                        Console.WriteLine($"{resultByPair.Id.Value} - origen={GetDisplay(statusMapByPair, resultByPair.OriginStatusId.Value)} -> destino={GetDisplay(statusMapByPair, resultByPair.DestinationStatusId.Value)}");
                        break;

                    case 4:
                        Console.Write("Ingrese el ID de la transiciÃ³n: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el nuevo ID del estado ORIGEN: ");
                        int newOriginId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el nuevo ID del estado DESTINO: ");
                        int newDestinationId = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newOriginId, newDestinationId);
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

    private async Task<Dictionary<int, string>> GetStatusDisplayMapAsync()
    {
        var statuses = await _getAllStatuses.ExecuteAsync();
        return statuses.ToDictionary(s => s.Id.Value, s => s.Name.Value);
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}


