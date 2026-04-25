// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\UI\AdminUpdateRouteFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Routes.UI;

public sealed class AdminUpdateRouteFlow
{
    private readonly GetAllRoutesUseCase _getAll;
    private readonly GetRouteByIdUseCase _getById;
    private readonly UpdateRouteUseCase _update;
    private readonly GetAllAirportsUseCase _getAllAirports;

    public AdminUpdateRouteFlow(
        GetAllRoutesUseCase getAll,
        GetRouteByIdUseCase getById,
        UpdateRouteUseCase update,
        GetAllAirportsUseCase getAllAirports)
    {
        _getAll = getAll;
        _getById = getById;
        _update = update;
        _getAllAirports = getAllAirports;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("ACTUALIZACION DE ROUTE");
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");
            await PrintRouteOptionsAsync();

            var id = ReadRequiredInt("ID de route");
            if (!id.HasValue) return;

            var existing = await _getById.ExecuteAsync(id.Value);
            if (existing is null)
            {
                AdminFlowConsole.PrintError("No existe una route con ese ID.");
                Pause();
                continue;
            }

            var airports = (await _getAllAirports.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.IataCode.Value})"))
                .OrderBy(x => x.id)
                .ToList();
            var origin = AdminFlowConsole.SelectById("AEROPUERTO ORIGEN", "Seleccione origen_id", airports);
            if (origin is null) return;
            var destination = AdminFlowConsole.SelectById("AEROPUERTO DESTINO", "Seleccione destino_id", airports);
            if (destination is null) return;

            var distanceKm = ReadOptionalInt("Distancia KM [opcional]");
            if (distanceKm.isCancelled) return;
            var durationMin = ReadOptionalInt("Duracion estimada min [opcional]");
            if (durationMin.isCancelled) return;

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Route ID: {id.Value}",
                $"Origen: {origin.Value.name}",
                $"Destino: {destination.Value.name}",
                $"Distancia KM: {(distanceKm.value?.ToString() ?? "NULL")}",
                $"Duracion min: {(durationMin.value?.ToString() ?? "NULL")}"
            });
            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _update.ExecuteAsync(id.Value, origin.Value.id, destination.Value.id, distanceKm.value, durationMin.value);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Route actualizada correctamente.");
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

    private static (int? value, bool isCancelled) ReadOptionalInt(string label)
    {
        while (true)
        {
            var raw = AdminFlowConsole.ReadOptionalText(label);
            if (raw == AdminFlowConsole.CancelToken) return (null, true);
            if (string.IsNullOrWhiteSpace(raw)) return (null, false);
            if (int.TryParse(raw, out var value)) return (value, false);
            AdminFlowConsole.PrintError("Debes ingresar un numero entero valido.");
        }
    }

    private static void Pause()
    {
        Console.WriteLine("Presiona una tecla para continuar...");
        Console.ReadKey();
    }

    private async Task PrintRouteOptionsAsync()
    {
        var items = (await _getAll.ExecuteAsync())
            .Select(x => $"[{x.Id.Value}] origen={x.OriginAirportId.Value} destino={x.DestinationAirportId.Value} km={(x.DistanceKm.Value?.ToString() ?? "NULL")}")
            .ToList();

        if (items.Count == 0)
        {
            AdminFlowConsole.PrintError("No hay routes para actualizar.");
            return;
        }

        var lines = items.Take(30).ToList();
        if (items.Count > 30)
            lines.Add($"... y {items.Count - 30} mas");

        AdminFlowConsole.PrintMenuBox("ROUTES DISPONIBLES", lines);
    }
}
