// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\shared\Ui\RoleMenus\CustomerRoleMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.shared.Ui.RoleMenus;

/// <summary>
/// Fachada de menú para perfil cliente.
/// Encapsula un <see cref="RoleMenu"/> con título específico de cliente.
/// </summary>
public sealed class CustomerRoleMenu
{
    private readonly RoleMenu _menu;

    public CustomerRoleMenu(IReadOnlyList<RoleMenuOption> options)
    {
        _menu = new RoleMenu("MENU CLIENTE", options);
    }

    /// <summary>
    /// Ejecuta el menú de cliente.
    /// </summary>
    public Task StartAsync() => _menu.StartAsync();
}
