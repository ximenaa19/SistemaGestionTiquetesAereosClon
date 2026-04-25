// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\UI\AdminCreateAirlineFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Countries.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Airlines.UI;

public sealed class AdminCreateAirlineFlow
{
    private readonly CreateAirlineUseCase _createAirline;
    private readonly GetAllCountriesUseCase _getAllCountries;

    public AdminCreateAirlineFlow(
        CreateAirlineUseCase createAirline,
        GetAllCountriesUseCase getAllCountries)
    {
        _createAirline = createAirline;
        _getAllCountries = getAllCountries;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("CREACION DE AIRLINE");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");
            Console.ResetColor();

            var countries = (await _getAllCountries.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.IsoCode.Value})"))
                .OrderBy(x => x.id)
                .ToList();

            var selectedCountry = AdminFlowConsole.SelectById("PAIS ORIGEN", "Seleccione origin_country_id", countries);
            if (selectedCountry is null) return;

            var name = AdminFlowConsole.ReadRequiredText("Nombre");
            if (name is null) return;

            var iataCode = AdminFlowConsole.ReadRequiredText("Codigo IATA (2 letras)");
            if (iataCode is null) return;

            var isActive = AdminFlowConsole.ReadYesNo("Activa? (S/N)");
            if (isActive is null) return;

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Nombre: {name}",
                $"IATA: {iataCode}",
                $"Pais origen: {selectedCountry.Value.name}",
                $"Activa: {(isActive.Value ? "Si" : "No")}"
            });

            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _createAirline.ExecuteAsync(name, iataCode, selectedCountry.Value.id, isActive.Value);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Airline creada correctamente.");
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
