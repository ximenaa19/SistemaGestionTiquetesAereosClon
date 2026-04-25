// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PassengerTypes\UI\PassengerTypeMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using System.Threading.Tasks;
using GestionAerolineas.src.Modules.PassengerTypes.Application.UseCases;
namespace GestionAerolineas.src.Modules.PassengerTypes.UI;

public class PassengerTypeMenu
{
    private readonly CreatePassengerTypeUseCase _create;
    private readonly GetAllPassengerTypesUseCase _getAll;
    private readonly GetPassengerTypeByIdUseCase _getById;
    private readonly GetPassengerTypeByNameUseCase _getByName;
    private readonly UpdatePassengerTypeUseCase _update;
    private readonly DeletePassengerTypeUseCase _delete;

    public PassengerTypeMenu(
        CreatePassengerTypeUseCase create,
        GetAllPassengerTypesUseCase getAll,
        GetPassengerTypeByIdUseCase getById,
        GetPassengerTypeByNameUseCase getByName,
        UpdatePassengerTypeUseCase update,
        DeletePassengerTypeUseCase delete)
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
            "Crear passenger type",
            "Listar passenger types",
            "Get passenger type by ID",
            "Get passenger type by name",
            "Actualizar passenger type",
            "Eliminar passenger type",
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

                        int? ageMin = ReadNullableInt("Ingrese edad_min (opcional): ");
                        int? ageMax = ReadNullableInt("Ingrese edad_max (opcional): ");

                        await _create.ExecuteAsync(name, ageMin, ageMax);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine($"{item.Id.Value} - {item.Name.Value} - edad_min={item.AgeMin?.ToString() ?? "null"} - edad_max={item.AgeMax?.ToString() ?? "null"}");
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);

                        var result = await _getById.ExecuteAsync(searchId);

                        Console.WriteLine(result == null
                            ? "No encontrado"
                            : $"{result.Id.Value} - {result.Name.Value} - edad_min={result.AgeMin?.ToString() ?? "null"} - edad_max={result.AgeMax?.ToString() ?? "null"}");
                        break;

                    case 3:
                        Console.Write("Ingrese el nombre: ");
                        string searchName = Console.ReadLine()!;

                        var resultByName = await _getByName.ExecuteAsync(searchName);

                        Console.WriteLine(resultByName == null
                            ? "No encontrado"
                            : $"{resultByName.Id.Value} - {resultByName.Name.Value} - edad_min={resultByName.AgeMin?.ToString() ?? "null"} - edad_max={resultByName.AgeMax?.ToString() ?? "null"}");
                        break;

                    case 4:
                        Console.Write("Ingrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el nuevo nombre: ");
                        string newName = Console.ReadLine()!;

                        int? newAgeMin = ReadNullableInt("Ingrese edad_min (opcional): ");
                        int? newAgeMax = ReadNullableInt("Ingrese edad_max (opcional): ");

                        await _update.ExecuteAsync(updateId, newName, newAgeMin, newAgeMax);
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
}


