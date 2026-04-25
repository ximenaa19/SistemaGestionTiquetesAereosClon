// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SystemRoles\UI\SystemRoleMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;

namespace GestionAerolineas.src.Modules.SystemRoles.UI;

public class SystemRoleMenu
{
    private readonly CreateSystemRoleUseCase _create;
    private readonly GetAllSystemRolesUseCase _getAll;
    private readonly GetSystemRoleByIdUseCase _getById;
    private readonly GetSystemRoleByNameUseCase _getByName;
    private readonly UpdateSystemRoleUseCase _update;
    private readonly DeleteSystemRoleUseCase _delete;

    public SystemRoleMenu(
        CreateSystemRoleUseCase create,
        GetAllSystemRolesUseCase getAll,
        GetSystemRoleByIdUseCase getById,
        GetSystemRoleByNameUseCase getByName,
        UpdateSystemRoleUseCase update,
        DeleteSystemRoleUseCase delete)
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
            "Crear system role",
            "Listar system roles",
            "Get system role by ID",
            "Get system role by name",
            "Actualizar system role",
            "Eliminar system role",
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

                        await _create.ExecuteAsync(name, description);
                        Console.WriteLine("Creado");
                        break;

                    case 1:
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine($"{item.Id.Value} - {item.Name.Value} - descripcion={item.Description.Value ?? "null"}");
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);

                        var result = await _getById.ExecuteAsync(searchId);

                        Console.WriteLine(result == null
                            ? "No encontrado"
                            : $"{result.Id.Value} - {result.Name.Value} - descripcion={result.Description.Value ?? "null"}");
                        break;

                    case 3:
                        Console.Write("Ingrese el nombre: ");
                        string searchName = Console.ReadLine()!;

                        var resultByName = await _getByName.ExecuteAsync(searchName);

                        Console.WriteLine(resultByName == null
                            ? "No encontrado"
                            : $"{resultByName.Id.Value} - {resultByName.Name.Value} - descripcion={resultByName.Description.Value ?? "null"}");
                        break;

                    case 4:
                        Console.Write("Ingrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el nuevo nombre: ");
                        string newName = Console.ReadLine()!;

                        Console.Write("Ingrese la nueva descripcion (opcional): ");
                        string? newDescription = Console.ReadLine();

                        await _update.ExecuteAsync(updateId, newName, newDescription);
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

