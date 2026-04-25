// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\UI\SessionMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Globalization;
using GestionAerolineas.src.Modules.Sessions.Application.UseCases;
using GestionAerolineas.src.Modules.Sessions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Users.Application.UseCases;

namespace GestionAerolineas.src.Modules.Sessions.UI;

public class SessionMenu
{
    private readonly CreateSessionUseCase _create;
    private readonly GetAllSessionsUseCase _getAll;
    private readonly GetSessionByIdUseCase _getById;
    private readonly GetSessionsByUserIdUseCase _getByUserId;
    private readonly GetActiveSessionsUseCase _getActive;
    private readonly GetInactiveSessionsUseCase _getInactive;
    private readonly GetSessionsByDateRangeUseCase _getByDateRange;
    private readonly UpdateSessionUseCase _update;
    private readonly ForceEndSessionUseCase _forceEnd;
    private readonly DeleteSessionUseCase _delete;
    private readonly GetAllUsersUseCase _getAllUsers;

    public SessionMenu(
        CreateSessionUseCase create,
        GetAllSessionsUseCase getAll,
        GetSessionByIdUseCase getById,
        GetSessionsByUserIdUseCase getByUserId,
        GetActiveSessionsUseCase getActive,
        GetInactiveSessionsUseCase getInactive,
        GetSessionsByDateRangeUseCase getByDateRange,
        UpdateSessionUseCase update,
        ForceEndSessionUseCase forceEnd,
        DeleteSessionUseCase delete,
        GetAllUsersUseCase getAllUsers)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByUserId = getByUserId;
        _getActive = getActive;
        _getInactive = getInactive;
        _getByDateRange = getByDateRange;
        _update = update;
        _forceEnd = forceEnd;
        _delete = delete;
        _getAllUsers = getAllUsers;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear session",
            "Listar sessions",
            "Get session by ID",
            "Get sessions by user_id",
            "Get active sessions",
            "Get inactive sessions",
            "Get sessions by date range",
            "Actualizar session",
            "Force end active sessions of another user",
            "Eliminar session",
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
                        await PrintUsersAsync();

                        Console.Write("\nIngrese user_id: ");
                        int userId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese started_at (yyyy-MM-dd HH:mm:ss) [default=now]: ");
                        DateTime startedAt = ReadDateTimeOrNow(Console.ReadLine());

                        Console.Write("Ingrese ended_at (yyyy-MM-dd HH:mm:ss) [opcional]: ");
                        DateTime? endedAt = ReadNullableDateTime(Console.ReadLine());

                        Console.Write("Ingrese ip_address [opcional]: ");
                        string? ipAddress = Console.ReadLine();

                        Console.Write("Ingrese activa (true/false) [default=true]: ");
                        string? activeInput = Console.ReadLine();
                        bool isActive = string.IsNullOrWhiteSpace(activeInput) ? true : bool.Parse(activeInput);

                        await _create.ExecuteAsync(userId, startedAt, endedAt, ipAddress, isActive);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        await PrintListAsync(await _getAll.ExecuteAsync());
                        break;

                    case 2:
                        await PrintSessionsForSelectionAsync();

                        Console.Write("\nIngrese el ID: ");
                        int id = int.Parse(Console.ReadLine()!);
                        var item = await _getById.ExecuteAsync(id);

                        if (item is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        await PrintOneAsync(item);
                        break;

                    case 3:
                        await PrintUsersAsync();

                        Console.Write("\nIngrese user_id: ");
                        int searchUserId = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getByUserId.ExecuteAsync(searchUserId));
                        break;

                    case 4:
                        await PrintListAsync(await _getActive.ExecuteAsync());
                        break;

                    case 5:
                        await PrintListAsync(await _getInactive.ExecuteAsync());
                        break;

                    case 6:
                        Console.Write("Ingrese fecha inicial (yyyy-MM-dd HH:mm:ss): ");
                        DateTime from = ReadRequiredDateTime(Console.ReadLine());

                        Console.Write("Ingrese fecha final (yyyy-MM-dd HH:mm:ss): ");
                        DateTime to = ReadRequiredDateTime(Console.ReadLine());

                        await PrintListAsync(await _getByDateRange.ExecuteAsync(from, to));
                        break;

                    case 7:
                        await PrintSessionsForSelectionAsync();
                        await PrintUsersAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese user_id: ");
                        int updateUserId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese started_at (yyyy-MM-dd HH:mm:ss): ");
                        DateTime updateStartedAt = ReadRequiredDateTime(Console.ReadLine());

                        Console.Write("Ingrese ended_at (yyyy-MM-dd HH:mm:ss) [opcional]: ");
                        DateTime? updateEndedAt = ReadNullableDateTime(Console.ReadLine());

                        Console.Write("Ingrese ip_address [opcional]: ");
                        string? updateIp = Console.ReadLine();

                        Console.Write("Ingrese activa (true/false): ");
                        bool updateIsActive = bool.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, updateUserId, updateStartedAt, updateEndedAt, updateIp, updateIsActive);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 8:
                        await PrintUsersAsync();

                        Console.Write("\nIngrese acting_user_id (debe ser Admin): ");
                        int actingUserId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese target_user_id: ");
                        int targetUserId = int.Parse(Console.ReadLine()!);

                        int closed = await _forceEnd.ExecuteAsync(actingUserId, targetUserId);
                        Console.WriteLine($"âœ” Sesiones cerradas: {closed}");
                        break;

                    case 9:
                        await PrintSessionsForSelectionAsync();

                        Console.Write("\nIngrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("âœ” Eliminado");
                        break;

                    case 10:
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"âŒ Error: {ex.GetBaseException().Message}");
            }

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();
        }
    }

    private async Task PrintSessionsForSelectionAsync()
    {
        Console.WriteLine("Sessions disponibles (primeras 30):");
        var items = (await _getAll.ExecuteAsync()).Take(30).ToList();

        if (items.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        foreach (var item in items)
            Console.WriteLine(Format(item));
    }

    private async Task PrintListAsync(IEnumerable<Session> items)
    {
        var list = items.ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("(sin resultados)");
            return;
        }

        foreach (var item in list)
            Console.WriteLine(Format(item));
    }

    private Task PrintOneAsync(Session item)
    {
        Console.WriteLine(Format(item));
        return Task.CompletedTask;
    }

    private async Task PrintUsersAsync()
    {
        Console.WriteLine("Users disponibles:");
        var users = (await _getAllUsers.ExecuteAsync()).Take(30).ToList();

        foreach (var user in users)
            Console.WriteLine($"{user.Id.Value} - {user.Username.Value} - role_id={user.RoleId.Value}");

        if (users.Count == 30)
            Console.WriteLine("(Mostrando solo los primeros 30)");
    }

    private static string Format(Session item)
    {
        var startedAt = item.StartedAt.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        var endedAt = item.EndedAt.Value?.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) ?? "NULL";
        var ip = string.IsNullOrWhiteSpace(item.IpAddress.Value) ? "NULL" : item.IpAddress.Value;
        var state = item.IsActive.Value ? "active" : "inactive";

        return $"{item.Id.Value} - user_id={item.UserId.Value} - startedAt={startedAt} - endedAt={endedAt} - ip={ip} - {state}";
    }

    private static DateTime ReadDateTimeOrNow(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return DateTime.Now;

        return DateTime.Parse(input, CultureInfo.InvariantCulture);
    }

    private static DateTime ReadRequiredDateTime(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new Exception("La fecha es obligatoria");

        return DateTime.Parse(input, CultureInfo.InvariantCulture);
    }

    private static DateTime? ReadNullableDateTime(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        return DateTime.Parse(input, CultureInfo.InvariantCulture);
    }
}

