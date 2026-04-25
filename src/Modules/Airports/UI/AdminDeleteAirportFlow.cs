// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\UI\AdminDeleteAirportFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Airports.UI;

public sealed class AdminDeleteAirportFlow
{
    private readonly GetAllAirportsUseCase _getAll;
    private readonly GetAirportByIdUseCase _getById;
    private readonly DeleteAirportUseCase _delete;

    public AdminDeleteAirportFlow(
        GetAllAirportsUseCase getAll,
        GetAirportByIdUseCase getById,
        DeleteAirportUseCase delete)
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
            AdminFlowConsole.PrintHeader("ELIMINACION DE AIRPORT");
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");

            var hasData = await PrintAirportOptionsAsync();
            if (!hasData)
            {
                Pause();
                return;
            }

            var id = ReadRequiredInt("ID de airport a eliminar");
            if (!id.HasValue) return;

            var existing = await _getById.ExecuteAsync(id.Value);
            if (existing is null)
            {
                AdminFlowConsole.PrintError("No existe un airport con ese ID.");
                Pause();
                continue;
            }

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Airport ID: {existing.Id.Value}",
                $"Nombre: {existing.Name.Value}",
                $"IATA: {existing.IataCode.Value}",
                $"ICAO: {(existing.IcaoCode?.Value ?? "NULL")}",
                $"Ciudad ID: {existing.CityId.Value}",
                "Accion: ELIMINAR registro"
            });
            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _delete.ExecuteAsync(id.Value);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Airport eliminado correctamente.");
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

    private async Task<bool> PrintAirportOptionsAsync()
    {
        var items = (await _getAll.ExecuteAsync())
            .Select(x => $"[{x.Id.Value}] {x.Name.Value} ({x.IataCode.Value})")
            .ToList();

        if (items.Count == 0)
        {
            AdminFlowConsole.PrintError("No hay airports para eliminar.");
            return false;
        }

        var lines = items.Take(30).ToList();
        if (items.Count > 30)
            lines.Add($"... y {items.Count - 30} mas");

        AdminFlowConsole.PrintMenuBox("AIRPORTS DISPONIBLES", lines);
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
