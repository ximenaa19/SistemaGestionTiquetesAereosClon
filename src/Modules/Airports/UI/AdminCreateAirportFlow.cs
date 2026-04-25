// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\UI\AdminCreateAirportFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Cities.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Airports.UI;

public sealed class AdminCreateAirportFlow
{
    private readonly CreateAirportUseCase _createAirport;
    private readonly GetAllCitiesUseCase _getAllCities;

    public AdminCreateAirportFlow(
        CreateAirportUseCase createAirport,
        GetAllCitiesUseCase getAllCities)
    {
        _createAirport = createAirport;
        _getAllCities = getAllCities;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("CREACION DE AIRPORT");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");
            Console.ResetColor();

            var cities = (await _getAllCities.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} (region:{x.RegionId.Value})"))
                .OrderBy(x => x.id)
                .ToList();

            var city = AdminFlowConsole.SelectById("CIUDADES", "Seleccione city_id", cities);
            if (city is null) return;

            var name = AdminFlowConsole.ReadRequiredText("Nombre");
            if (name is null) return;

            var iataCode = AdminFlowConsole.ReadRequiredText("Codigo IATA (3 letras)");
            if (iataCode is null) return;

            var icaoCode = AdminFlowConsole.ReadOptionalText("Codigo ICAO (4 letras) [opcional]");
            if (icaoCode == AdminFlowConsole.CancelToken) return;

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Nombre: {name}",
                $"IATA: {iataCode}",
                $"ICAO: {(string.IsNullOrWhiteSpace(icaoCode) ? "NULL" : icaoCode)}",
                $"Ciudad: {city.Value.name}"
            });

            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _createAirport.ExecuteAsync(
                    name,
                    iataCode,
                    string.IsNullOrWhiteSpace(icaoCode) ? null : icaoCode,
                    city.Value.id);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Airport creado correctamente.");
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
