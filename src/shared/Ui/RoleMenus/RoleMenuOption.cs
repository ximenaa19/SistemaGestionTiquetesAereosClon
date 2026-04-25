// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\shared\Ui\RoleMenus\RoleMenuOption.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.shared.Ui.RoleMenus;

/// <summary>
/// Representa una opción navegable del menú:
/// etiqueta visible + acción asíncrona a ejecutar.
/// </summary>
public sealed record RoleMenuOption(
    string Label,
    Func<Task> Action
);
