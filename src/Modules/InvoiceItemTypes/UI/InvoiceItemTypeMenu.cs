// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\UI\InvoiceItemTypeMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using System.Threading.Tasks;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Application.UseCases;
namespace GestionAerolineas.src.Modules.InvoiceItemTypes.UI;

public class InvoiceItemTypeMenu
{
    private readonly CreateInvoiceItemTypeUseCase _create;
    private readonly GetAllInvoiceItemTypesUseCase _getAll;
    private readonly GetInvoiceItemTypeByIdUseCase _getById;
    private readonly GetInvoiceItemTypeByNameUseCase _getByName;
    private readonly UpdateInvoiceItemTypeUseCase _update;
    private readonly DeleteInvoiceItemTypeUseCase _delete;

    public InvoiceItemTypeMenu(
        CreateInvoiceItemTypeUseCase create,
        GetAllInvoiceItemTypesUseCase getAll,
        GetInvoiceItemTypeByIdUseCase getById,
        GetInvoiceItemTypeByNameUseCase getByName,
        UpdateInvoiceItemTypeUseCase update,
        DeleteInvoiceItemTypeUseCase delete)
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
            "Crear invoice item type",
            "Listar invoice item types",
            "Get invoice item type by ID",
            "Get invoice item type by name",
            "Actualizar invoice item type",
            "Eliminar invoice item type",
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

