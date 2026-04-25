// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\UI\AdminUpdateAirlineFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Countries.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Airlines.UI;

public sealed class AdminUpdateAirlineFlow
{
    private readonly GetAllAirlinesUseCase _getAll;
    private readonly GetAirlineByIdUseCase _getById;
    private readonly UpdateAirlineUseCase _update;
    private readonly GetAllCountriesUseCase _getAllCountries;

    public AdminUpdateAirlineFlow(
        GetAllAirlinesUseCase getAll,
        GetAirlineByIdUseCase getById,
        UpdateAirlineUseCase update,
        GetAllCountriesUseCase getAllCountries)
    {
        _getAll = getAll;
        _getById = getById;
        _update = update;
        _getAllCountries = getAllCountries;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("ACTUALIZACION DE AIRLINE");
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");
            await PrintAirlineOptionsAsync();

            var id = ReadRequiredInt("ID de airline");
            if (!id.HasValue) return;

            var existing = await _getById.ExecuteAsync(id.Value);
            if (existing is null)
            {
                AdminFlowConsole.PrintError("No existe una airline con ese ID.");
                Pause();
                continue;
            }

            var countries = (await _getAllCountries.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.IsoCode.Value})"))
                .OrderBy(x => x.id)
                .ToList();
            var country = AdminFlowConsole.SelectById("PAIS ORIGEN", "Seleccione pais_origen_id", countries);
            if (country is null) return;

            var name = AdminFlowConsole.ReadRequiredText("Nombre");
            if (name is null) return;
            var iataCode = AdminFlowConsole.ReadRequiredText("Codigo IATA (2 letras)");
            if (iataCode is null) return;
            var isActive = AdminFlowConsole.ReadYesNo("Activa? (S/N)");
            if (isActive is null) return;

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Airline ID: {id.Value}",
                $"Nombre: {name}",
                $"IATA: {iataCode}",
                $"Pais origen: {country.Value.name}",
                $"Activa: {(isActive.Value ? "Si" : "No")}"
            });
            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _update.ExecuteAsync(id.Value, name, iataCode, country.Value.id, isActive.Value);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Airline actualizada correctamente.");
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

    private async Task PrintAirlineOptionsAsync()
    {
        var items = (await _getAll.ExecuteAsync())
            .Select(x => $"[{x.Id.Value}] {x.Name.Value} ({x.IataCode.Value})")
            .ToList();

        if (items.Count == 0)
        {
            AdminFlowConsole.PrintError("No hay airlines para actualizar.");
            return;
        }

        var lines = items.Take(30).ToList();
        if (items.Count > 30)
            lines.Add($"... y {items.Count - 30} mas");

        AdminFlowConsole.PrintMenuBox("AIRLINES DISPONIBLES", lines);
    }
}
