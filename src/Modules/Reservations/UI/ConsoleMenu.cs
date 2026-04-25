// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\UI\ConsoleMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;

namespace GestionAerolineas.src.Modules.Reservations.UI;

public class ConsoleMenu
{
    private int _selectedIndex = 0;
    private readonly string[] _options;

    public ConsoleMenu(string[] options)
    {
        _options = options;
    }

    public int Show()
    {
        ConsoleKey key;

        do
        {
            Console.Clear();

            Console.WriteLine("=== MENU ===\n");

            for (int i = 0; i < _options.Length; i++)
            {
                if (i == _selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"> {_options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {_options[i]}");
                }
            }

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    _selectedIndex--;
                    if (_selectedIndex < 0)
                        _selectedIndex = _options.Length - 1;
                    break;

                case ConsoleKey.DownArrow:
                    _selectedIndex++;
                    if (_selectedIndex >= _options.Length)
                        _selectedIndex = 0;
                    break;
            }
        } while (key != ConsoleKey.Enter);

        return _selectedIndex;
    }
}

