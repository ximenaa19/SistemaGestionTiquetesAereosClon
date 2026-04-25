// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\shared\Ui\IModuleUI.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.shared.Ui;

public interface IModuleUI
{
    string Key { get; }
    string Title { get; }
    Task RunAsync(CancellationToken cancellationToken = default);
}


