// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\shared\Ui\RoleMenus\RoleMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.shared.Ui.RoleMenus;

/// <summary>
/// Componente base de menú navegable por flechas para consola.
/// Se reutiliza en los distintos menús por rol y submenús del sistema.
/// </summary>
public sealed class RoleMenu
{
    private readonly string _title;
    private readonly IReadOnlyList<RoleMenuOption> _options;
    private readonly string _exitLabel;

    public RoleMenu(string title, IReadOnlyList<RoleMenuOption> options, string exitLabel = "Salir")
    {
        _title = title;
        _options = options;
        _exitLabel = exitLabel;
    }

    /// <summary>
    /// Inicia el bucle de interacción del menú hasta que el usuario salga o pulse Escape.
    /// </summary>
    public async Task StartAsync()
    {
        var selected = 0;
        var totalOptions = _options.Count + 1;

        while (true)
        {
            Render(selected);

            var key = Console.ReadKey(intercept: true).Key;
            if (key == ConsoleKey.UpArrow)
            {
                selected = (selected - 1 + totalOptions) % totalOptions;
                continue;
            }

            if (key == ConsoleKey.DownArrow)
            {
                selected = (selected + 1) % totalOptions;
                continue;
            }

            if (key == ConsoleKey.Escape)
                return;

            if (key != ConsoleKey.Enter)
                continue;

            if (selected == _options.Count)
                return;

            try
            {
                await _options[selected].Action();
            }
            catch (Exception ex)
            {
                Console.ResetColor();
                Console.WriteLine($"Error: {ex.GetBaseException().Message}");
                Console.WriteLine("Presiona una tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    /// <summary>
    /// Dibuja título, opciones y ayuda visual según la posición seleccionada.
    /// </summary>
    private void Render(int selected)
    {
        Console.Clear();
        DrawTitle();

        for (var index = 0; index < _options.Count; index++)
            DrawOption(index, _options[index].Label, selected == index);

        DrawOption(_options.Count, _exitLabel, selected == _options.Count);

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Usa ↑ ↓ para moverte, Enter para seleccionar, Esc para volver.");
        Console.ResetColor();
    }

    /// <summary>
    /// Dibuja el encabezado decorado del menú actual.
    /// </summary>
    private void DrawTitle()
    {
        var line = new string('═', Math.Max(32, _title.Length + 8));
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"╔{line}╗");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"║   {_title.ToUpperInvariant()}   ║");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"╚{line}╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    /// <summary>
    /// Dibuja una opción individual, resaltando la opción activa.
    /// </summary>
    private static void DrawOption(int index, string label, bool selected)
    {
        if (selected)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("➤ ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{index + 1}. {label}");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("  ");
        Console.WriteLine($"{index + 1}. {label}");
        Console.ResetColor();
    }
}
