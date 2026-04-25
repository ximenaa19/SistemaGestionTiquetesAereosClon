// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\shared\Ui\AdminFlowConsole.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Globalization;

namespace GestionAerolineas.src.shared.Ui;

public static class AdminFlowConsole
{
    public const string CancelToken = "000000";

    public static void PrintHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================");
        Console.WriteLine(title.PadLeft((title.Length + 40) / 2).PadRight(40));
        Console.WriteLine("========================================");
        Console.ResetColor();
    }

    public static void PrintMenuBox(string title, IReadOnlyList<string> lines)
    {
        var width = Math.Max(36, lines.DefaultIfEmpty(string.Empty).Max(x => x.Length) + 4);
        var horizontal = new string('═', width);

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"\n╔{horizontal}╗");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"║ {title.PadRight(width - 1)}║");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"╠{horizontal}╣");
        Console.ForegroundColor = ConsoleColor.Gray;
        foreach (var line in lines)
            Console.WriteLine($"║ {line.PadRight(width - 1)}║");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"╚{horizontal}╝");
        Console.ResetColor();
    }

    public static string ReadRaw(string label)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"\n{label}: ");
        Console.ResetColor();
        return (Console.ReadLine() ?? string.Empty).Trim();
    }

    public static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ {message}");
        Console.ResetColor();
    }

    public static string? ReadRequiredText(string label)
    {
        while (true)
        {
            var value = ReadRaw(label);
            if (value == CancelToken)
                return null;
            if (string.IsNullOrWhiteSpace(value))
            {
                PrintError("Este campo es obligatorio.");
                continue;
            }

            return value.Trim();
        }
    }

    public static string ReadOptionalText(string label)
    {
        return ReadRaw(label);
    }

    public static bool? ReadYesNo(string label)
    {
        while (true)
        {
            var value = ReadRaw(label);
            if (value == CancelToken)
                return null;

            var norm = value.Trim().ToUpperInvariant();
            if (norm is "S" or "SI" or "Y" or "YES")
                return true;
            if (norm is "N" or "NO")
                return false;

            PrintError("Debes ingresar S o N.");
        }
    }

    public static DateTime? ReadOptionalDate(string label)
    {
        while (true)
        {
            var value = ReadRaw(label);
            if (value == CancelToken)
                return DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                return date;

            PrintError("Formato invalido. Usa yyyy-MM-dd.");
        }
    }

    public static (int id, string name)? SelectById(string title, string prompt, List<(int id, string name)> items)
    {
        while (true)
        {
            PrintMenuBox(title, items.Select(x => $"[{x.id}] {x.name}").ToList());
            var raw = ReadRaw(prompt);
            if (raw == CancelToken)
                return null;

            if (!int.TryParse(raw, out var id))
            {
                PrintError("Debes ingresar un número válido.");
                continue;
            }

            var selected = items.FirstOrDefault(x => x.id == id);
            if (selected == default)
            {
                PrintError("El ID no existe en la lista.");
                continue;
            }

            return selected;
        }
    }

    public static int ReadConfirmChoice(IReadOnlyList<string> summaryLines)
    {
        while (true)
        {
            PrintMenuBox("RESUMEN DE DATOS", summaryLines.ToList());
            PrintMenuBox("SELECCIONE UNA OPCIÓN", new List<string>
            {
                "[1] Confirmar",
                "[2] Editar",
                "[3] Cancelar"
            });

            var raw = ReadRaw("Opción");
            if (raw == CancelToken || raw == "3") return 3;
            if (raw == "2") return 2;
            if (raw == "1") return 1;

            PrintError("Debes ingresar 1, 2 o 3.");
        }
    }

    public static string? ReadPasswordMin8(string label)
    {
        while (true)
        {
            var value = ReadHidden(label);
            if (value == null) return null;
            if (value.Length < 8)
            {
                PrintError("La contraseña debe tener mínimo 8 caracteres.");
                continue;
            }

            var confirm = ReadHidden("Confirmar contraseña");
            if (confirm == null) return null;
            if (value != confirm)
            {
                PrintError("Las contraseñas no coinciden.");
                continue;
            }

            return value;
        }
    }

    private static string? ReadHidden(string label)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"\n{label}: ");
        Console.ResetColor();

        var buffer = new List<char>();
        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                var text = new string(buffer.ToArray()).Trim();
                if (text == CancelToken)
                    return null;
                if (string.IsNullOrWhiteSpace(text))
                {
                    PrintError("Este campo es obligatorio.");
                    return string.Empty;
                }
                return text;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (buffer.Count == 0) continue;
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
    }
}

