// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\UI\AdminUpdateAirportFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Cities.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Airports.UI;

public sealed class AdminUpdateAirportFlow
{
    private readonly GetAllAirportsUseCase _getAll;
    private readonly GetAirportByIdUseCase _getById;
    private readonly UpdateAirportUseCase _update;
    private readonly GetAllCitiesUseCase _getAllCities;

    public AdminUpdateAirportFlow(
        GetAllAirportsUseCase getAll,
        GetAirportByIdUseCase getById,
        UpdateAirportUseCase update,
        GetAllCitiesUseCase getAllCities)
    {
        _getAll = getAll;
        _getById = getById;
        _update = update;
        _getAllCities = getAllCities;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("ACTUALIZACION DE AIRPORT");
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");
            await PrintAirportOptionsAsync();

            var id = ReadRequiredInt("ID de airport");
            if (!id.HasValue) return;

            var existing = await _getById.ExecuteAsync(id.Value);
            if (existing is null)
            {
                AdminFlowConsole.PrintError("No existe un airport con ese ID.");
                Pause();
                continue;
            }

            var cities = (await _getAllCities.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} (region:{x.RegionId.Value})"))
                .OrderBy(x => x.id)
                .ToList();
            var city = AdminFlowConsole.SelectById("CIUDADES", "Seleccione ciudad_id", cities);
            if (city is null) return;

            var name = AdminFlowConsole.ReadRequiredText("Nombre");
            if (name is null) return;
            var iata = AdminFlowConsole.ReadRequiredText("Codigo IATA (3 letras)");
            if (iata is null) return;
            var icao = AdminFlowConsole.ReadOptionalText("Codigo ICAO (4 letras) [opcional]");
            if (icao == AdminFlowConsole.CancelToken) return;

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Airport ID: {id.Value}",
                $"Nombre: {name}",
                $"IATA: {iata}",
                $"ICAO: {(string.IsNullOrWhiteSpace(icao) ? "NULL" : icao)}",
                $"Ciudad: {city.Value.name}"
            });
            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _update.ExecuteAsync(id.Value, name, iata, string.IsNullOrWhiteSpace(icao) ? null : icao, city.Value.id);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Airport actualizado correctamente.");
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

    private async Task PrintAirportOptionsAsync()
    {
        var items = (await _getAll.ExecuteAsync())
            .Select(x => $"[{x.Id.Value}] {x.Name.Value} ({x.IataCode.Value})")
            .ToList();

        if (items.Count == 0)
        {
            AdminFlowConsole.PrintError("No hay airports para actualizar.");
            return;
        }

        var lines = items.Take(30).ToList();
        if (items.Count > 30)
            lines.Add($"... y {items.Count - 30} mas");

        AdminFlowConsole.PrintMenuBox("AIRPORTS DISPONIBLES", lines);
    }
}
