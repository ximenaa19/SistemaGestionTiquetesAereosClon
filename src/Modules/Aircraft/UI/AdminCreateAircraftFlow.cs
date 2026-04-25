// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\UI\AdminCreateAircraftFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Application.UseCases;
using GestionAerolineas.src.Modules.AircraftModels.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Aircraft.UI;

public sealed class AdminCreateAircraftFlow
{
    private readonly CreateAircraftUseCase _createAircraft;
    private readonly GetAllAircraftModelsUseCase _getAllModels;
    private readonly GetAllAirlinesUseCase _getAllAirlines;

    public AdminCreateAircraftFlow(
        CreateAircraftUseCase createAircraft,
        GetAllAircraftModelsUseCase getAllModels,
        GetAllAirlinesUseCase getAllAirlines)
    {
        _createAircraft = createAircraft;
        _getAllModels = getAllModels;
        _getAllAirlines = getAllAirlines;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("CREACION DE AIRCRAFT");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");
            Console.ResetColor();

            var models = (await _getAllModels.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.ModelName.Value} (cap:{x.MaxCapacity.Value})"))
                .OrderBy(x => x.id)
                .ToList();
            var selectedModel = AdminFlowConsole.SelectById("MODELOS", "Seleccione model_id", models);
            if (selectedModel is null) return;

            var airlines = (await _getAllAirlines.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.IataCode.Value})"))
                .OrderBy(x => x.id)
                .ToList();
            var selectedAirline = AdminFlowConsole.SelectById("AEROLINEAS", "Seleccione airline_id", airlines);
            if (selectedAirline is null) return;

            var registration = AdminFlowConsole.ReadRequiredText("Matricula");
            if (registration is null) return;

            var manufactureDate = AdminFlowConsole.ReadOptionalDate("Fecha fabricacion (yyyy-MM-dd) [opcional]");
            if (manufactureDate == DateTime.MinValue) return;

            var isActive = AdminFlowConsole.ReadYesNo("Activa? (S/N)");
            if (isActive is null) return;

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Modelo: {selectedModel.Value.name}",
                $"Aerolinea: {selectedAirline.Value.name}",
                $"Matricula: {registration}",
                $"Fecha fabricacion: {(manufactureDate?.ToString("yyyy-MM-dd") ?? "NULL")}",
                $"Activa: {(isActive.Value ? "Si" : "No")}"
            });

            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _createAircraft.ExecuteAsync(
                    selectedModel.Value.id,
                    selectedAirline.Value.id,
                    registration,
                    manufactureDate,
                    isActive.Value);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Aircraft creado correctamente.");
                Console.ResetColor();
                Console.WriteLine("Presiona una tecla para continuar...");
                Console.ReadKey();
                return;
            }
            catch (Exception ex)
            {
                AdminFlowConsole.PrintError(ex.GetBaseException().Message);
                Console.WriteLine("Presiona una tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}
