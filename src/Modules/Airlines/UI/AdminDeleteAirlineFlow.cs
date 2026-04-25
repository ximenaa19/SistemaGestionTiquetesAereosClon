// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\UI\AdminDeleteAirlineFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Airlines.UI;

public sealed class AdminDeleteAirlineFlow
{
    private readonly GetAllAirlinesUseCase _getAll;
    private readonly GetAirlineByIdUseCase _getById;
    private readonly DeleteAirlineUseCase _delete;

    public AdminDeleteAirlineFlow(
        GetAllAirlinesUseCase getAll,
        GetAirlineByIdUseCase getById,
        DeleteAirlineUseCase delete)
    {
        _getAll = getAll;
        _getById = getById;
        _delete = delete;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("ELIMINACION DE AIRLINE");
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");

            var hasData = await PrintAirlineOptionsAsync();
            if (!hasData)
            {
                Pause();
                return;
            }

            var id = ReadRequiredInt("ID de airline a eliminar");
            if (!id.HasValue) return;

            var existing = await _getById.ExecuteAsync(id.Value);
            if (existing is null)
            {
                AdminFlowConsole.PrintError("No existe una airline con ese ID.");
                Pause();
                continue;
            }

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Airline ID: {existing.Id.Value}",
                $"Nombre: {existing.Name.Value}",
                $"IATA: {existing.IataCode.Value}",
                $"Pais origen ID: {existing.OriginCountryId.Value}",
                "Accion: ELIMINAR registro"
            });
            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _delete.ExecuteAsync(id.Value);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Airline eliminada correctamente.");
                Console.ResetColor();
                Pause();
                return;
            }
            catch (Exception ex)
            {
                AdminFlowConsole.PrintError(ex.GetBaseException().Message);
                Pause();
            }
        }
    }

    private async Task<bool> PrintAirlineOptionsAsync()
    {
        var items = (await _getAll.ExecuteAsync())
            .Select(x => $"[{x.Id.Value}] {x.Name.Value} ({x.IataCode.Value})")
            .ToList();

        if (items.Count == 0)
        {
            AdminFlowConsole.PrintError("No hay airlines para eliminar.");
            return false;
        }

        var lines = items.Take(30).ToList();
        if (items.Count > 30)
            lines.Add($"... y {items.Count - 30} mas");

        AdminFlowConsole.PrintMenuBox("AIRLINES DISPONIBLES", lines);
        return true;
    }

    private static int? ReadRequiredInt(string label)
    {
        while (true)
        {
            var raw = AdminFlowConsole.ReadRaw(label);
            if (raw == AdminFlowConsole.CancelToken) return null;
            if (int.TryParse(raw, out var value)) return value;
            AdminFlowConsole.PrintError("Debes ingresar un numero valido.");
        }
    }

    private static void Pause()
    {
        Console.WriteLine("Presiona una tecla para continuar...");
        Console.ReadKey();
    }
}
