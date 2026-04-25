// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\shared\Ui\RoleMenus\StaffRoleMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.shared.Ui.RoleMenus;

/// <summary>
/// Fachada de menú para perfil staff/agente.
/// Encapsula un <see cref="RoleMenu"/> con título específico de operación.
/// </summary>
public sealed class StaffRoleMenu
{
    private readonly RoleMenu _menu;

    public StaffRoleMenu(IReadOnlyList<RoleMenuOption> options)
    {
        _menu = new RoleMenu("MENU STAFF", options);
    }

    /// <summary>
    /// Ejecuta el menú de staff.
    /// </summary>
    public Task StartAsync() => _menu.StartAsync();
}
