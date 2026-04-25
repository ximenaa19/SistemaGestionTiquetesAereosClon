// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\UI\ConsoleMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.UI;

public class ConsoleMenu
{
    private readonly string[] _options;

    public ConsoleMenu(string[] options)
    {
        _options = options;
    }

    public int Show()
    {
        Console.WriteLine("Seleccione una opción:");

        for (int i = 0; i < _options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {_options[i]}");
        }

        Console.Write("Opción: ");
        var input = Console.ReadLine();

        if (int.TryParse(input, out int selected) && selected >= 1 && selected <= _options.Length)
            return selected - 1;

        Console.WriteLine("Opción inválida.");
        return Show();
    }
}
