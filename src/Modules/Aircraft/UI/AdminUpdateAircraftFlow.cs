// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\UI\AdminUpdateAircraftFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Application.UseCases;
using GestionAerolineas.src.Modules.AircraftModels.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Aircraft.UI;

public sealed class AdminUpdateAircraftFlow
{
    private readonly GetAllAircraftUseCase _getAll;
    private readonly GetAircraftByIdUseCase _getById;
    private readonly UpdateAircraftUseCase _update;
    private readonly GetAllAircraftModelsUseCase _getAllModels;
    private readonly GetAllAirlinesUseCase _getAllAirlines;

    public AdminUpdateAircraftFlow(
        GetAllAircraftUseCase getAll,
        GetAircraftByIdUseCase getById,
        UpdateAircraftUseCase update,
        GetAllAircraftModelsUseCase getAllModels,
        GetAllAirlinesUseCase getAllAirlines)
    {
        _getAll = getAll;
        _getById = getById;
        _update = update;
        _getAllModels = getAllModels;
        _getAllAirlines = getAllAirlines;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("ACTUALIZACION DE AIRCRAFT");
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");
            await PrintAircraftOptionsAsync();

            var id = ReadRequiredInt("ID de aircraft");
            if (!id.HasValue) return;

            var existing = await _getById.ExecuteAsync(id.Value);
            if (existing is null)
            {
                AdminFlowConsole.PrintError("No existe un aircraft con ese ID.");
                Pause();
                continue;
            }

            var models = (await _getAllModels.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.ModelName.Value} (cap:{x.MaxCapacity.Value})"))
                .OrderBy(x => x.id)
                .ToList();
            var model = AdminFlowConsole.SelectById("MODELOS", "Seleccione model_id", models);
            if (model is null) return;

            var airlines = (await _getAllAirlines.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.IataCode.Value})"))
                .OrderBy(x => x.id)
                .ToList();
            var airline = AdminFlowConsole.SelectById("AEROLINEAS", "Seleccione airline_id", airlines);
            if (airline is null) return;

            var registration = AdminFlowConsole.ReadRequiredText("Matricula");
            if (registration is null) return;
            var manufactureDate = AdminFlowConsole.ReadOptionalDate("Fecha fabricacion (yyyy-MM-dd) [opcional]");
            if (manufactureDate == DateTime.MinValue) return;
            var isActive = AdminFlowConsole.ReadYesNo("Activa? (S/N)");
            if (isActive is null) return;

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Aircraft ID: {id.Value}",
                $"Modelo: {model.Value.name}",
                $"Aerolinea: {airline.Value.name}",
                $"Matricula: {registration}",
                $"Fecha fabricacion: {(manufactureDate?.ToString("yyyy-MM-dd") ?? "NULL")}",
                $"Activa: {(isActive.Value ? "Si" : "No")}"
            });
            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _update.ExecuteAsync(id.Value, model.Value.id, airline.Value.id, registration, manufactureDate, isActive.Value);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Aircraft actualizado correctamente.");
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

    private static void Pause()
    {
        Console.WriteLine("Presiona una tecla para continuar...");
        Console.ReadKey();
    }

    private async Task PrintAircraftOptionsAsync()
    {
        var items = (await _getAll.ExecuteAsync())
            .Select(x => $"[{x.Id.Value}] {x.Registration.Value} - model={x.ModelId.Value} - airline={x.AirlineId.Value}")
            .ToList();

        if (items.Count == 0)
        {
            AdminFlowConsole.PrintError("No hay aircraft para actualizar.");
            return;
        }

        var lines = items.Take(30).ToList();
        if (items.Count > 30)
            lines.Add($"... y {items.Count - 30} mas");

        AdminFlowConsole.PrintMenuBox("AIRCRAFT DISPONIBLES", lines);
    }
}
