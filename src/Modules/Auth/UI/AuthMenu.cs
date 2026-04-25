// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Auth\UI\AuthMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Auth.Application.UseCases;
using GestionAerolineas.src.Modules.Auth.Application.Models;
using GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;
using GestionAerolineas.src.Modules.SystemRoles.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.Auth.UI;

/// <summary>
/// Menú principal de autenticación en consola.
/// Centraliza el flujo de registro y login para entregar un resultado de sesión
/// que luego se usa para enrutar al menú por rol.
/// </summary>
public class AuthMenu
{
    private readonly RegisterAuthUserUseCase _register;
    private readonly LoginUserUseCase _login;
    private readonly GetAllSystemRolesUseCase _getAllRolesUseCase;

    public AuthMenu(
        RegisterAuthUserUseCase register,
        LoginUserUseCase login,
        GetAllSystemRolesUseCase getAllRolesUseCase)
    {
        _register = register;
        _login = login;
        _getAllRolesUseCase = getAllRolesUseCase;
    }

    /// <summary>
    /// Muestra el menú de autenticación hasta que el usuario:
    /// 1) inicia sesión correctamente o
    /// 2) decide salir.
    /// </summary>
    /// <returns>
    /// Resultado de login cuando la autenticación es exitosa; <c>null</c> si el usuario sale.
    /// </returns>
    public async Task<AuthLoginResult?> StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Register user",
            "Login",
            "Salir"
        });

        while (true)
        {
            var option = menu.Show();

            try
            {
                switch (option)
                {
                    case 0:
                        await HandleRegisterAsync();
                        break;

                    case 1:
                        Console.Write("Ingrese nombre (username): ");
                        var loginUsername = Console.ReadLine() ?? string.Empty;
                        var loginPassword = ReadHiddenRequired("Ingrese contrasenia: ");

                        var result = await _login.ExecuteAsync(loginUsername, loginPassword);
                        Console.WriteLine($"âœ” Login exitoso - userId={result.UserId} - roleId={result.RoleId}");
                        return result;

                    case 2:
                        return null;
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

    /// <summary>
    /// Ejecuta el flujo interactivo de registro:
    /// captura datos, valida contraseña, permite elegir rol y confirma antes de persistir.
    /// </summary>
    private async Task HandleRegisterAsync()
    {
        while (true)
        {
            Console.Write("Ingrese nombre (username): ");
            var username = (Console.ReadLine() ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("El nombre es obligatorio");

            var password = ReadValidPasswordWithConfirmation();
            var selectedRole = await SelectRoleAsync();

            Console.WriteLine("\n=== Confirmar registro ===");
            Console.WriteLine($"Nombre: {username}");
            Console.WriteLine($"Rol: {selectedRole.Name.Value}");
            var decision = ReadConfirmChoice();

            if (decision == 1)
            {
                await _register.ExecuteAsync(username, password, selectedRole.Id.Value);
                Console.WriteLine("âœ” Registro completado");
                return;
            }

            if (decision == 2)
                continue;

            Console.WriteLine("Registro cancelado.");
            return;
        }
    }

    /// <summary>
    /// Lista roles disponibles en catálogo y obliga a seleccionar uno válido por id.
    /// </summary>
    private async Task<SystemRole> SelectRoleAsync()
    {
        var roles = (await _getAllRolesUseCase.ExecuteAsync()).ToList();
        if (roles.Count == 0)
            throw new Exception("No hay roles del sistema configurados.");

        Console.WriteLine("\nRoles disponibles:");
        foreach (var role in roles.OrderBy(r => r.Id.Value))
            Console.WriteLine($"{role.Id.Value} - {role.Name.Value}");

        while (true)
        {
            Console.Write("Ingrese rol_id: ");
            var raw = Console.ReadLine();
            if (!int.TryParse(raw, out var roleId))
            {
                Console.WriteLine("âŒ Debes ingresar un numero valido.");
                continue;
            }

            var selected = roles.FirstOrDefault(r => r.Id.Value == roleId);
            if (selected is null)
            {
                Console.WriteLine("âŒ El rol no existe en la lista. Intenta de nuevo.");
                continue;
            }

            return selected;
        }
    }

    /// <summary>
    /// Lee contraseña oculta y exige longitud mínima + confirmación.
    /// Repite únicamente este paso si hay error para no perder el resto del progreso.
    /// </summary>
    private static string ReadValidPasswordWithConfirmation()
    {
        while (true)
        {
            var password = ReadHiddenRequired("Ingrese contrasenia (minimo 8 caracteres): ");
            if (password.Length < 8)
            {
                Console.WriteLine("âŒ La contrasenia debe tener minimo 8 caracteres.");
                continue;
            }

            var confirm = ReadHiddenRequired("Confirmar contrasenia: ");
            if (password != confirm)
            {
                Console.WriteLine("âŒ Las contrasenias no coinciden.");
                continue;
            }

            return password;
        }
    }

    /// <summary>
    /// Lee la opción final de confirmación del registro (1 confirmar, 2 editar, 3 cancelar).
    /// </summary>
    private static int ReadConfirmChoice()
    {
        while (true)
        {
            Console.WriteLine("1. Confirmar");
            Console.WriteLine("2. Editar");
            Console.WriteLine("3. Cancelar");
            Console.Write("Seleccione opcion (1-3): ");
            var raw = Console.ReadLine();
            if (raw is "1" or "2" or "3")
                return int.Parse(raw);

            Console.WriteLine("âŒ Opcion invalida. Debes ingresar 1, 2 o 3.");
        }
    }

    /// <summary>
    /// Lee un valor oculto obligatorio (ej. contraseña) y falla si llega vacío.
    /// </summary>
    private static string ReadHiddenRequired(string prompt)
    {
        var value = ReadHiddenLine(prompt);
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception("El valor es obligatorio");
        return value;
    }

    /// <summary>
    /// Lee texto ocultando caracteres en consola con asteriscos.
    /// </summary>
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

