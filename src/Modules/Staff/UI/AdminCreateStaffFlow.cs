// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\UI\AdminCreateStaffFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.Staff.Application.UseCases;
using GestionAerolineas.src.Modules.StaffRoles.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Staff.UI;

public sealed class AdminCreateStaffFlow
{
    private readonly CreateStaffUseCase _createStaff;
    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllStaffRolesUseCase _getAllRoles;
    private readonly GetAllAirlinesUseCase _getAllAirlines;
    private readonly GetAllAirportsUseCase _getAllAirports;

    public AdminCreateStaffFlow(
        CreateStaffUseCase createStaff,
        GetAllPeopleUseCase getAllPeople,
        GetAllStaffRolesUseCase getAllRoles,
        GetAllAirlinesUseCase getAllAirlines,
        GetAllAirportsUseCase getAllAirports)
    {
        _createStaff = createStaff;
        _getAllPeople = getAllPeople;
        _getAllRoles = getAllRoles;
        _getAllAirlines = getAllAirlines;
        _getAllAirports = getAllAirports;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("CREACIÓN DE STAFF");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");
            Console.ResetColor();

            var people = (await _getAllPeople.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.FirstNames.Value} {x.LastNames.Value}"))
                .OrderBy(x => x.id).ToList();
            var person = AdminFlowConsole.SelectById("PERSONAS", "Seleccione person_id", people);
            if (person is null) return;

            var roles = (await _getAllRoles.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: x.Name.Value))
                .OrderBy(x => x.id).ToList();
            var role = AdminFlowConsole.SelectById("ROLES STAFF", "Seleccione staff_role_id", roles);
            if (role is null) return;

            var airlines = (await _getAllAirlines.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.IataCode.Value})"))
                .OrderBy(x => x.id).ToList();
            var airports = (await _getAllAirports.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.IataCode.Value})"))
                .OrderBy(x => x.id).ToList();

            int? airlineId;
            int? airportId;
            while (true)
            {
                airlineId = await SelectOptionalAsync("AEROLÍNEA (OPCIONAL)", "Seleccione airline_id", airlines);
                if (airlineId == int.MinValue) return;

                airportId = await SelectOptionalAsync("AEROPUERTO (OPCIONAL)", "Seleccione airport_id", airports);
                if (airportId == int.MinValue) return;

                if (airlineId.HasValue || airportId.HasValue)
                    break;

                AdminFlowConsole.PrintError("Debes seleccionar aerolínea o aeropuerto (al menos uno).");
            }

            DateTime hireDate;
            while (true)
            {
                var raw = AdminFlowConsole.ReadRaw("Fecha ingreso (yyyy-MM-dd)");
                if (raw == AdminFlowConsole.CancelToken) return;
                if (DateTime.TryParseExact(raw, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out hireDate))
                    break;
                AdminFlowConsole.PrintError("Formato inválido. Usa yyyy-MM-dd.");
            }

            var active = AdminFlowConsole.ReadYesNo("Activo? (S/N)");
            if (active is null) return;

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Persona: {person.Value.name}",
                $"Rol: {role.Value.name}",
                $"Airline ID: {(airlineId?.ToString() ?? "NULL")}",
                $"Airport ID: {(airportId?.ToString() ?? "NULL")}",
                $"Fecha ingreso: {hireDate:yyyy-MM-dd}",
                $"Activo: {(active.Value ? "Sí" : "No")}"
            });
            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _createStaff.ExecuteAsync(person.Value.id, role.Value.id, airlineId, airportId, hireDate, active.Value);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Staff creado correctamente.");
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

    private static Task<int?> SelectOptionalAsync(string title, string prompt, List<(int id, string name)> items)
    {
        while (true)
        {
            var lines = new List<string> { "[0] Ninguno" };
            lines.AddRange(items.Select(x => $"[{x.id}] {x.name}"));
            AdminFlowConsole.PrintMenuBox(title, lines);

            var raw = AdminFlowConsole.ReadRaw($"{prompt} (0 opcional)");
            if (raw == AdminFlowConsole.CancelToken) return Task.FromResult<int?>(int.MinValue);
            if (!int.TryParse(raw, out var id))
            {
                AdminFlowConsole.PrintError("Debes ingresar un número válido.");
                continue;
            }
            if (id == 0) return Task.FromResult<int?>(null);
            if (items.Any(x => x.id == id)) return Task.FromResult<int?>(id);
            AdminFlowConsole.PrintError("El ID no existe en la lista.");
        }
    }
}

