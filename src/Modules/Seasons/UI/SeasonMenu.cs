// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\UI\SeasonMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Seasons.Application.UseCases;

namespace GestionAerolineas.src.Modules.Seasons.UI;

public class SeasonMenu
{
    private readonly CreateSeasonUseCase _create;
    private readonly GetAllSeasonsUseCase _getAll;
    private readonly GetSeasonByIdUseCase _getById;
    private readonly GetSeasonByNameUseCase _getByName;
    private readonly UpdateSeasonUseCase _update;
    private readonly DeleteSeasonUseCase _delete;

    public SeasonMenu(
        CreateSeasonUseCase create,
        GetAllSeasonsUseCase getAll,
        GetSeasonByIdUseCase getById,
        GetSeasonByNameUseCase getByName,
        UpdateSeasonUseCase update,
        DeleteSeasonUseCase delete)
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
            "Crear season",
            "Listar seasons",
            "Get season by ID",
            "Get season by name",
            "Actualizar season",
            "Eliminar season",
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

                        Console.Write("Ingrese la descripcion (opcional): ");
                        string? description = Console.ReadLine();

                        decimal priceFactor = ReadDecimal("Ingrese el precio_factor: ");

                        await _create.ExecuteAsync(name, description, priceFactor);
                        Console.WriteLine("Creado");
                        break;

                    case 1:
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine($"{item.Id.Value} - {item.Name.Value} - descripcion={item.Description.Value ?? "null"} - precio_factor={item.PriceFactor.Value:0.0000}");
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);

                        var result = await _getById.ExecuteAsync(searchId);

                        Console.WriteLine(result == null
                            ? "No encontrado"
                            : $"{result.Id.Value} - {result.Name.Value} - descripcion={result.Description.Value ?? "null"} - precio_factor={result.PriceFactor.Value:0.0000}");
                        break;

                    case 3:
                        Console.Write("Ingrese el nombre: ");
                        string searchName = Console.ReadLine()!;

                        var resultByName = await _getByName.ExecuteAsync(searchName);

                        Console.WriteLine(resultByName == null
                            ? "No encontrado"
                            : $"{resultByName.Id.Value} - {resultByName.Name.Value} - descripcion={resultByName.Description.Value ?? "null"} - precio_factor={resultByName.PriceFactor.Value:0.0000}");
                        break;

                    case 4:
                        Console.Write("Ingrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el nuevo nombre: ");
                        string newName = Console.ReadLine()!;

                        Console.Write("Ingrese la nueva descripcion (opcional): ");
                        string? newDescription = Console.ReadLine();

                        decimal newPriceFactor = ReadDecimal("Ingrese el nuevo precio_factor: ");

                        await _update.ExecuteAsync(updateId, newName, newDescription, newPriceFactor);
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

    private static decimal ReadDecimal(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();

        if (!decimal.TryParse(input, out var value))
            throw new Exception("Valor decimal invalido");

        return value;
    }
}

