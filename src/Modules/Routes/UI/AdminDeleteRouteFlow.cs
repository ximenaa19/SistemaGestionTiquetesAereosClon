// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\UI\AdminDeleteRouteFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Routes.UI;

public sealed class AdminDeleteRouteFlow
{
    private readonly GetAllRoutesUseCase _getAll;
    private readonly GetRouteByIdUseCase _getById;
    private readonly DeleteRouteUseCase _delete;

    public AdminDeleteRouteFlow(
        GetAllRoutesUseCase getAll,
        GetRouteByIdUseCase getById,
        DeleteRouteUseCase delete)
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
            AdminFlowConsole.PrintHeader("ELIMINACION DE ROUTE");
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");

            var hasData = await PrintRouteOptionsAsync();
            if (!hasData)
            {
                Pause();
                return;
            }

            var id = ReadRequiredInt("ID de route a eliminar");
            if (!id.HasValue) return;

            var existing = await _getById.ExecuteAsync(id.Value);
            if (existing is null)
            {
                AdminFlowConsole.PrintError("No existe una route con ese ID.");
                Pause();
                continue;
            }

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Route ID: {existing.Id.Value}",
                $"Origen ID: {existing.OriginAirportId.Value}",
                $"Destino ID: {existing.DestinationAirportId.Value}",
                $"Distancia KM: {(existing.DistanceKm.Value?.ToString() ?? "NULL")}",
                $"Duracion min: {(existing.EstimatedDurationMin.Value?.ToString() ?? "NULL")}",
                "Accion: ELIMINAR registro"
            });
            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _delete.ExecuteAsync(id.Value);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Route eliminada correctamente.");
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

    private async Task<bool> PrintRouteOptionsAsync()
    {
        var items = (await _getAll.ExecuteAsync())
            .Select(x => $"[{x.Id.Value}] origen={x.OriginAirportId.Value} destino={x.DestinationAirportId.Value} km={(x.DistanceKm.Value?.ToString() ?? "NULL")}")
            .ToList();

        if (items.Count == 0)
        {
            AdminFlowConsole.PrintError("No hay routes para eliminar.");
            return false;
        }

        var lines = items.Take(30).ToList();
        if (items.Count > 30)
            lines.Add($"... y {items.Count - 30} mas");

        AdminFlowConsole.PrintMenuBox("ROUTES DISPONIBLES", lines);
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
