// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\shared\Helpers\MenuLogic.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.shared.Helpers;

public class MenuLogic
{
    public static void Menus_logic(Dictionary<string, Action> Selections)
    {
        ConsoleKeyInfo user_selection = Console.ReadKey(true);

        string key = user_selection.KeyChar.ToString();

        if (Selections.ContainsKey(key))
            Selections[key]();
        else
        {
            Console.Write("\nOpcion no valida, oprima cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}


