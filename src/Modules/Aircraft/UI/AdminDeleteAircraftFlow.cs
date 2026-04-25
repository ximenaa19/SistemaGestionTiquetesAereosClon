// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\UI\AdminDeleteAircraftFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Aircraft.UI;

public sealed class AdminDeleteAircraftFlow
{
    private readonly GetAllAircraftUseCase _getAll;
    private readonly GetAircraftByIdUseCase _getById;
    private readonly DeleteAircraftUseCase _delete;

    public AdminDeleteAircraftFlow(
        GetAllAircraftUseCase getAll,
        GetAircraftByIdUseCase getById,
        DeleteAircraftUseCase delete)
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
            AdminFlowConsole.PrintHeader("ELIMINACION DE AIRCRAFT");
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");

            var hasData = await PrintAircraftOptionsAsync();
            if (!hasData)
            {
                Pause();
                return;
            }

            var id = ReadRequiredInt("ID de aircraft a eliminar");
            if (!id.HasValue) return;

            var existing = await _getById.ExecuteAsync(id.Value);
            if (existing is null)
            {
                AdminFlowConsole.PrintError("No existe un aircraft con ese ID.");
                Pause();
                continue;
            }

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Aircraft ID: {existing.Id.Value}",
                $"Matricula: {existing.Registration.Value}",
                $"Modelo ID: {existing.ModelId.Value}",
                $"Airline ID: {existing.AirlineId.Value}",
                "Accion: ELIMINAR registro"
            });
            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _delete.ExecuteAsync(id.Value);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Aircraft eliminado correctamente.");
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

    private async Task<bool> PrintAircraftOptionsAsync()
    {
        var items = (await _getAll.ExecuteAsync())
            .Select(x => $"[{x.Id.Value}] {x.Registration.Value} - model={x.ModelId.Value} - airline={x.AirlineId.Value}")
            .ToList();

        if (items.Count == 0)
        {
            AdminFlowConsole.PrintError("No hay aircraft para eliminar.");
            return false;
        }

        var lines = items.Take(30).ToList();
        if (items.Count > 30)
            lines.Add($"... y {items.Count - 30} mas");

        AdminFlowConsole.PrintMenuBox("AIRCRAFT DISPONIBLES", lines);
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
