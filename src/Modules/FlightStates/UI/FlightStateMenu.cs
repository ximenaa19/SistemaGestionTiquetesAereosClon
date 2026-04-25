// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStates\UI\FlightStateMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using System.Threading.Tasks;
using GestionAerolineas.src.Modules.FlightStates.Application.UseCases;
namespace GestionAerolineas.src.Modules.FlightStates.UI;

public class FlightStateMenu
{
    private readonly CreateFlightStateUseCase _create;
    private readonly GetAllFlightStatesUseCase _getAll;
    private readonly GetFlightStateByIdUseCase _getById;
    private readonly GetFlightStateByNameUseCase _getByName;
    private readonly UpdateFlightStateUseCase _update;
    private readonly DeleteFlightStateUseCase _delete;

    public FlightStateMenu(
        CreateFlightStateUseCase create,
        GetAllFlightStatesUseCase getAll,
        GetFlightStateByIdUseCase getById,
        GetFlightStateByNameUseCase getByName,
        UpdateFlightStateUseCase update,
        DeleteFlightStateUseCase delete)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByName = getByName;
        _update = update;
        _delete = delete;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear flight state",
            "Listar flight states",
            "Get flight state by ID",
            "Get flight state by name",
            "Actualizar flight state",
            "Eliminar flight state",
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
                        Console.Write("Ingrese el nombre: ");
                        string name = Console.ReadLine()!;

                        await _create.ExecuteAsync(name);
                        Console.WriteLine("Creado");
                        break;

                    case 1:
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine($"{item.Id.Value} - {item.Name.Value}");
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);

                        var result = await _getById.ExecuteAsync(searchId);

                        Console.WriteLine(result == null
                            ? "No encontrado"
                            : $"{result.Id.Value} - {result.Name.Value}");
                        break;

                    case 3:
                        Console.Write("Ingrese el nombre: ");
                        string searchName = Console.ReadLine()!;

                        var resultByName = await _getByName.ExecuteAsync(searchName);

                        Console.WriteLine(resultByName == null
                            ? "No encontrado"
                            : $"{resultByName.Id.Value} - {resultByName.Name.Value}");
                        break;

                    case 4:
                        Console.Write("Ingrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el nuevo nombre: ");
                        string newName = Console.ReadLine()!;

                        await _update.ExecuteAsync(updateId, newName);
                        Console.WriteLine("Actualizado");
                        break;

                    case 5:
                        Console.Write("Ingrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("Eliminado");
                        break;

                    case 6:
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetBaseException().Message}");
            }

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}

