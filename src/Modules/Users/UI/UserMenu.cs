// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\UI\UserMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Globalization;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;
using GestionAerolineas.src.Modules.Users.Application.UseCases;
using GestionAerolineas.src.Modules.Users.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.Users.UI;

public class UserMenu
{
    private readonly CreateUserUseCase _create;
    private readonly GetAllUsersUseCase _getAll;
    private readonly GetUserByIdUseCase _getById;
    private readonly GetUserByUsernameUseCase _getByUsername;
    private readonly GetUsersByRoleIdUseCase _getByRoleId;
    private readonly UpdateUserUseCase _update;
    private readonly SetUserActiveStatusUseCase _setActive;
    private readonly DeleteUserHardUseCase _deleteHard;

    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllSystemRolesUseCase _getAllRoles;

    public UserMenu(
        CreateUserUseCase create,
        GetAllUsersUseCase getAll,
        GetUserByIdUseCase getById,
        GetUserByUsernameUseCase getByUsername,
        GetUsersByRoleIdUseCase getByRoleId,
        UpdateUserUseCase update,
        SetUserActiveStatusUseCase setActive,
        DeleteUserHardUseCase deleteHard,
        GetAllPeopleUseCase getAllPeople,
        GetAllSystemRolesUseCase getAllRoles)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByUsername = getByUsername;
        _getByRoleId = getByRoleId;
        _update = update;
        _setActive = setActive;
        _deleteHard = deleteHard;
        _getAllPeople = getAllPeople;
        _getAllRoles = getAllRoles;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear user",
            "Listar users",
            "Get user by ID",
            "Get user by username",
            "Get users by rol_id",
            "Actualizar user",
            "Deactivate/Activate a user (soft)",
            "Eliminar user (hard)",
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
                        await PrintRolesAsync();
                        await PrintPeopleAsync();

                        Console.Write("\nIngrese username: ");
                        string username = Console.ReadLine() ?? string.Empty;

                        string password = ReadHiddenRequired("Ingrese password: ");
                        string confirm = ReadHiddenRequired("Confirmar password: ");
                        if (password != confirm)
                            throw new Exception("Las contraseÃ±as no coinciden");

                        Console.Write("Ingrese person_id [opcional]: ");
                        int? personId = ReadNullableInt(Console.ReadLine());

                        Console.Write("Ingrese rol_id: ");
                        int roleId = int.Parse(Console.ReadLine()!);

                        await _create.ExecuteAsync(username, password, personId, roleId);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        await PrintListAsync(await _getAll.ExecuteAsync());
                        break;

                    case 2:
                        await PrintUsersForSelectionAsync();

                        Console.Write("\nIngrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);

                        var byId = await _getById.ExecuteAsync(searchId);
                        if (byId is null)
                        {
                            Console.WriteLine("(sin registros)");
                            break;
                        }

                        await PrintOneAsync(byId);
                        break;

                    case 3:
                        Console.Write("Ingrese username: ");
                        string searchUsername = Console.ReadLine() ?? string.Empty;

                        var byUsername = await _getByUsername.ExecuteAsync(searchUsername);
                        if (byUsername is null)
                        {
                            Console.WriteLine("(sin registros)");
                            break;
                        }

                        await PrintOneAsync(byUsername);
                        break;

                    case 4:
                        await PrintRolesAsync();

                        Console.Write("\nIngrese rol_id: ");
                        int searchRoleId = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getByRoleId.ExecuteAsync(searchRoleId));
                        break;

                    case 5:
                        await PrintUsersForSelectionAsync();
                        await PrintRolesAsync();
                        await PrintPeopleAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese username: ");
                        string newUsername = Console.ReadLine() ?? string.Empty;

                        string? newPassword = ReadHiddenOptional("Ingrese nuevo password [opcional]: ");
                        if (!string.IsNullOrWhiteSpace(newPassword))
                        {
                            var confirmNew = ReadHiddenRequired("Confirmar nuevo password: ");
                            if (newPassword != confirmNew)
                                throw new Exception("Las contraseÃ±as no coinciden");
                        }

                        Console.Write("Ingrese person_id [opcional]: ");
                        int? newPersonId = ReadNullableInt(Console.ReadLine());

                        Console.Write("Ingrese rol_id: ");
                        int newRoleId = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newUsername, newPassword, newPersonId, newRoleId);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 6:
                        await PrintUsersForSelectionAsync();

                        Console.Write("\nIngrese el ID: ");
                        int toggleId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese activo (true/false, 1/0) [default=true]: ");
                        bool isActive = ReadBool(Console.ReadLine(), defaultValue: true);

                        string? actingUsername = null;
                        if (!isActive)
                        {
                            Console.Write("Ingrese su username actual para validar la desactivacion: ");
                            actingUsername = Console.ReadLine();
                        }

                        await _setActive.ExecuteAsync(toggleId, isActive, actingUsername);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 7:
                        await PrintUsersForSelectionAsync();

                        Console.Write("\nIngrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        Console.Write("Confirmar (y/N): ");
                        var confirmDelete = (Console.ReadLine() ?? string.Empty).Trim();
                        if (!string.Equals(confirmDelete, "y", StringComparison.OrdinalIgnoreCase))
                            break;

                        await _deleteHard.ExecuteAsync(deleteId);
                        Console.WriteLine("âœ” Eliminado");
                        break;

                    case 8:
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

    private async Task PrintUsersForSelectionAsync()
    {
        Console.WriteLine("Users (primeros 30):");
        var list = (await _getAll.ExecuteAsync()).Take(30).ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        var personMap = await GetPersonDisplayMapAsync();
        var roleMap = await GetRoleDisplayMapAsync();

        foreach (var item in list)
            Console.WriteLine(Format(item, personMap, roleMap));
    }

    private async Task PrintListAsync(IEnumerable<User> list)
    {
        var items = list.ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("(sin resultados)");
            return;
        }

        var personMap = await GetPersonDisplayMapAsync();
        var roleMap = await GetRoleDisplayMapAsync();

        foreach (var item in items)
            Console.WriteLine(Format(item, personMap, roleMap));
    }

    private async Task PrintOneAsync(User item)
    {
        var personMap = await GetPersonDisplayMapAsync();
        var roleMap = await GetRoleDisplayMapAsync();
        Console.WriteLine(Format(item, personMap, roleMap));
    }

    private async Task PrintPeopleAsync()
    {
        var people = (await _getAllPeople.ExecuteAsync()).ToList();
        Console.WriteLine("\nPeople (top 10):");
        foreach (var person in people.Take(10))
            Console.WriteLine($"{person.Id.Value} - {person.FirstNames.Value} {person.LastNames.Value} - doc={person.DocumentNumber.Value}");

        Console.Write("Buscar persona (texto) [opcional]: ");
        var search = (Console.ReadLine() ?? string.Empty).Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalized = search.ToUpperInvariant();
            var matches = people
                .Where(p => $"{p.FirstNames.Value} {p.LastNames.Value}".ToUpperInvariant().Contains(normalized))
                .Take(10)
                .ToList();

            Console.WriteLine("\nCoincidencias (top 10):");
            if (matches.Count == 0)
                Console.WriteLine("(sin registros)");
            else
                foreach (var person in matches)
                    Console.WriteLine($"{person.Id.Value} - {person.FirstNames.Value} {person.LastNames.Value} - doc={person.DocumentNumber.Value}");
        }

        Console.WriteLine("(Dejar vacio person_id permite crear un user sin persona)");
    }

    private async Task PrintRolesAsync()
    {
        var roles = (await _getAllRoles.ExecuteAsync()).ToList();

        Console.WriteLine("SystemRoles (top 10):");
        foreach (var role in roles.Take(10))
            Console.WriteLine($"{role.Id.Value} - {role.Name.Value}");

        Console.Write("Buscar rol (texto) [opcional]: ");
        var search = (Console.ReadLine() ?? string.Empty).Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalized = search.ToUpperInvariant();
            var matches = roles
                .Where(r => r.Name.Value.ToUpperInvariant().Contains(normalized))
                .Take(10)
                .ToList();

            Console.WriteLine("\nCoincidencias (top 10):");
            if (matches.Count == 0)
                Console.WriteLine("(sin registros)");
            else
                foreach (var role in matches)
                    Console.WriteLine($"{role.Id.Value} - {role.Name.Value}");
        }
    }

    private async Task<Dictionary<int, string>> GetPersonDisplayMapAsync()
    {
        var people = await _getAllPeople.ExecuteAsync();
        return people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
    }

    private async Task<Dictionary<int, string>> GetRoleDisplayMapAsync()
    {
        var roles = await _getAllRoles.ExecuteAsync();
        return roles.ToDictionary(r => r.Id.Value, r => r.Name.Value);
    }

    private static string Format(User item, Dictionary<int, string> personMap, Dictionary<int, string> roleMap)
    {
        string personDisplay = item.PersonId.Value.HasValue
            ? GetDisplay(personMap, item.PersonId.Value.Value)
            : "NULL";
        string roleDisplay = GetDisplay(roleMap, item.RoleId.Value);
        var lastAccessDisplay = item.LastAccess.Value?.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) ?? "NULL";
        var activeDisplay = item.IsActive.Value ? "active" : "inactive";

        return $"{item.Id.Value} - username={item.Username.Value} - role={roleDisplay} - person={personDisplay} - {activeDisplay} - lastAccess={lastAccessDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }

    private static int? ReadNullableInt(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        return int.Parse(input);
    }

    private static bool ReadBool(string? input, bool defaultValue)
    {
        if (string.IsNullOrWhiteSpace(input))
            return defaultValue;

        var normalized = input.Trim().ToUpperInvariant();
        return normalized switch
        {
            "1" => true,
            "0" => false,
            "TRUE" => true,
            "FALSE" => false,
            "T" => true,
            "F" => false,
            "Y" => true,
            "N" => false,
            "S" => true,
            _ => bool.Parse(input)
        };
    }

    private static string ReadHiddenRequired(string prompt)
    {
        var value = ReadHiddenLine(prompt);
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception("El valor es obligatorio");
        return value;
    }

    private static string? ReadHiddenOptional(string prompt)
    {
        var value = ReadHiddenLine(prompt);
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private static string ReadHiddenLine(string prompt)
    {
        Console.Write(prompt);

        var buffer = new List<char>();
        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (buffer.Count == 0)
                    continue;

                buffer.RemoveAt(buffer.Count - 1);
                Console.Write("\b \b");
                continue;
            }

            if (!char.IsControl(key.KeyChar))
            {
                buffer.Add(key.KeyChar);
                Console.Write("*");
            }
        }

        return new string(buffer.ToArray());
    }
}

