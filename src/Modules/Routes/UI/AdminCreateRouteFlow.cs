// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\UI\AdminCreateRouteFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Routes.UI;

public sealed class AdminCreateRouteFlow
{
    private readonly CreateRouteUseCase _createRoute;
    private readonly GetAllAirportsUseCase _getAllAirports;

    public AdminCreateRouteFlow(
        CreateRouteUseCase createRoute,
        GetAllAirportsUseCase getAllAirports)
    {
        _createRoute = createRoute;
        _getAllAirports = getAllAirports;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("CREACION DE ROUTE");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");
            Console.ResetColor();

            var airports = (await _getAllAirports.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.IataCode.Value})"))
                .OrderBy(x => x.id)
                .ToList();

            var origin = AdminFlowConsole.SelectById("AEROPUERTO ORIGEN", "Seleccione origin_airport_id", airports);
            if (origin is null) return;

            var destination = AdminFlowConsole.SelectById("AEROPUERTO DESTINO", "Seleccione destination_airport_id", airports);
            if (destination is null) return;

            int? distanceKm;
            while (true)
            {
                var raw = AdminFlowConsole.ReadOptionalText("Distancia KM [opcional]");
                if (raw == AdminFlowConsole.CancelToken) return;
                if (string.IsNullOrWhiteSpace(raw))
                {
                    distanceKm = null;
                    break;
                }

                if (int.TryParse(raw, out var value))
                {
                    distanceKm = value;
                    break;
                }

                AdminFlowConsole.PrintError("Debes ingresar un numero entero valido.");
            }

            int? durationMin;
            while (true)
            {
                var raw = AdminFlowConsole.ReadOptionalText("Duracion estimada (min) [opcional]");
                if (raw == AdminFlowConsole.CancelToken) return;
                if (string.IsNullOrWhiteSpace(raw))
                {
                    durationMin = null;
                    break;
                }

                if (int.TryParse(raw, out var value))
                {
                    durationMin = value;
                    break;
                }

                AdminFlowConsole.PrintError("Debes ingresar un numero entero valido.");
            }

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Origen: {origin.Value.name}",
                $"Destino: {destination.Value.name}",
                $"Distancia KM: {(distanceKm?.ToString() ?? "NULL")}",
                $"Duracion min: {(durationMin?.ToString() ?? "NULL")}"
            });

            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _createRoute.ExecuteAsync(origin.Value.id, destination.Value.id, distanceKm, durationMin);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Route creada correctamente.");
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
