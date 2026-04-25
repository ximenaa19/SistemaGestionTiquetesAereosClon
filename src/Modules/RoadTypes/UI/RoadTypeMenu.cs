// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RoadTypes\UI\RoadTypeMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using System.Threading.Tasks;
using GestionAerolineas.src.Modules.RoadTypes.Application.UseCases;

namespace GestionAerolineas.src.Modules.RoadTypes.UI;

public class RoadTypeMenu
{
    private readonly CreateRoadTypeUseCase _create;
    private readonly GetAllRoadTypesUseCase _getAll;
    private readonly GetRoadTypeByIdUseCase _getById;
    private readonly GetRoadTypeByNameUseCase _getByName;
    private readonly UpdateRoadTypeUseCase _update;
    private readonly DeleteRoadTypeUseCase _delete;

    public RoadTypeMenu(
        CreateRoadTypeUseCase create,
        GetAllRoadTypesUseCase getAll,
        GetRoadTypeByIdUseCase getById,
        GetRoadTypeByNameUseCase getByName,
        UpdateRoadTypeUseCase update,
        DeleteRoadTypeUseCase delete)
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
            "Crear road type",
            "Listar road types",
            "Get road type by ID",
            "Get road type by name",
            "Actualizar road type",
            "Eliminar road type",
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
                        Console.Write("Ingrese el ID: ");
                        int id = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el nombre: ");
                        string name = Console.ReadLine()!;

                        await _create.ExecuteAsync(id, name);
                        Console.WriteLine("âœ” Creado");
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
                Console.WriteLine($"âŒ Error: {ex.Message}");
            }

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
