// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\UI\AdminCreateUserFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;
using GestionAerolineas.src.Modules.Users.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Users.UI;

public sealed class AdminCreateUserFlow
{
    private readonly CreateUserUseCase _createUser;
    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllSystemRolesUseCase _getAllRoles;

    public AdminCreateUserFlow(
        CreateUserUseCase createUser,
        GetAllPeopleUseCase getAllPeople,
        GetAllSystemRolesUseCase getAllRoles)
    {
        _createUser = createUser;
        _getAllPeople = getAllPeople;
        _getAllRoles = getAllRoles;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("CREACIÓN DE USER");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");
            Console.ResetColor();

            var roles = (await _getAllRoles.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: x.Name.Value))
                .OrderBy(x => x.id).ToList();

            var role = AdminFlowConsole.SelectById("ROLES", "Seleccione rol_id", roles);
            if (role is null) return;

            var people = (await _getAllPeople.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.FirstNames.Value} {x.LastNames.Value}"))
                .OrderBy(x => x.id).ToList();

            int? personId = null;
            while (true)
            {
                var lines = new List<string> { "[0] Sin persona asociada" };
                lines.AddRange(people.Select(x => $"[{x.id}] {x.name}"));
                AdminFlowConsole.PrintMenuBox("PERSONA (OPCIONAL)", lines);
                var raw = AdminFlowConsole.ReadRaw("Seleccione person_id (0 opcional)");
                if (raw == AdminFlowConsole.CancelToken) return;
                if (!int.TryParse(raw, out var id))
                {
                    AdminFlowConsole.PrintError("Debes ingresar un número válido.");
                    continue;
                }
                if (id == 0) break;
                if (!people.Any(x => x.id == id))
                {
                    AdminFlowConsole.PrintError("El ID no existe en la lista.");
                    continue;
                }
                personId = id;
                break;
            }

            var username = AdminFlowConsole.ReadRequiredText("Username");
            if (username is null) return;

            var password = AdminFlowConsole.ReadPasswordMin8("Contraseña");
            if (password is null) return;

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Rol: {role.Value.name}",
                $"Person ID: {(personId?.ToString() ?? "NULL")}",
                $"Username: {username}"
            });

            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _createUser.ExecuteAsync(username, password, personId, role.Value.id);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ User creado correctamente.");
                Console.ResetColor();
                Console.WriteLine("Presiona una tecla para continuar...");
                Console.ReadKey();
                return;
            }
            catch (Exception ex)
            {
                AdminFlowConsole.PrintError(ex.GetBaseException().Message);
                Console.WriteLine("Presiona una tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}

