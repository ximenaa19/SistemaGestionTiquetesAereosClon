// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\UI\FlightAssignmentMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.FlightAssignments.Application.UseCases;
using GestionAerolineas.src.Modules.FlightAssignments.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightRoles.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.Staff.Application.UseCases;
using GestionAerolineas.src.Modules.StaffRoles.Application.UseCases;

namespace GestionAerolineas.src.Modules.FlightAssignments.UI;

public class FlightAssignmentMenu
{
    private const int TopCount = 10;

    private readonly CreateFlightAssignmentUseCase _create;
    private readonly GetAllFlightAssignmentsUseCase _getAll;
    private readonly GetFlightAssignmentByIdUseCase _getById;
    private readonly GetFlightAssignmentsByFlightIdUseCase _getByFlightId;
    private readonly GetFlightAssignmentsByStaffIdUseCase _getByStaffId;
    private readonly GetFlightAssignmentsByFlightRoleIdUseCase _getByFlightRoleId;
    private readonly GetFlightAssignmentByFlightAndStaffUseCase _getByFlightAndStaff;
    private readonly UpdateFlightAssignmentUseCase _update;
    private readonly DeleteFlightAssignmentUseCase _delete;

    private readonly GetAllFlightsUseCase _getAllFlights;
    private readonly GetAllRoutesUseCase _getAllRoutes;
    private readonly GetAllAirportsUseCase _getAllAirports;
    private readonly GetAllAirlinesUseCase _getAllAirlines;
    private readonly GetAllStaffUseCase _getAllStaff;
    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllStaffRolesUseCase _getAllStaffRoles;
    private readonly GetAllFlightRolesUseCase _getAllFlightRoles;

    public FlightAssignmentMenu(
        CreateFlightAssignmentUseCase create,
        GetAllFlightAssignmentsUseCase getAll,
        GetFlightAssignmentByIdUseCase getById,
        GetFlightAssignmentsByFlightIdUseCase getByFlightId,
        GetFlightAssignmentsByStaffIdUseCase getByStaffId,
        GetFlightAssignmentsByFlightRoleIdUseCase getByFlightRoleId,
        GetFlightAssignmentByFlightAndStaffUseCase getByFlightAndStaff,
        UpdateFlightAssignmentUseCase update,
        DeleteFlightAssignmentUseCase delete,
        GetAllFlightsUseCase getAllFlights,
        GetAllRoutesUseCase getAllRoutes,
        GetAllAirportsUseCase getAllAirports,
        GetAllAirlinesUseCase getAllAirlines,
        GetAllStaffUseCase getAllStaff,
        GetAllPeopleUseCase getAllPeople,
        GetAllStaffRolesUseCase getAllStaffRoles,
        GetAllFlightRolesUseCase getAllFlightRoles)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByFlightId = getByFlightId;
        _getByStaffId = getByStaffId;
        _getByFlightRoleId = getByFlightRoleId;
        _getByFlightAndStaff = getByFlightAndStaff;
        _update = update;
        _delete = delete;
        _getAllFlights = getAllFlights;
        _getAllRoutes = getAllRoutes;
        _getAllAirports = getAllAirports;
        _getAllAirlines = getAllAirlines;
        _getAllStaff = getAllStaff;
        _getAllPeople = getAllPeople;
        _getAllStaffRoles = getAllStaffRoles;
        _getAllFlightRoles = getAllFlightRoles;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear flight assignment",
            "Listar flight assignments",
            "Get assignment by ID",
            "Get assignments by flight_id",
            "Get assignments by staff_id",
            "Get assignments by flight_role_id",
            "Get assignment by flight_id + staff_id",
            "Actualizar assignment",
            "Eliminar assignment",
            "Salir"
        });

        while (true)
        {
            int option = menu.Show();

            try
            {
                switch (option)
                {
                    case 0:
                        await PrintFlightsAsync();
                        await PrintStaffAsync();
                        await PrintFlightRolesAsync();

                        Console.Write("\nIngrese vuelo_id: ");
                        int flightId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese personal_id (staff.id): ");
                        int staffId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese rol_vuelo_id: ");
                        int flightRoleId = int.Parse(Console.ReadLine()!);

                        await _create.ExecuteAsync(flightId, staffId, flightRoleId);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        await PrintListAsync(await _getAll.ExecuteAsync());
                        break;

                    case 2:
                        await PrintAssignmentsForSelectionAsync();

                        Console.Write("\nIngrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);

                        var byId = await _getById.ExecuteAsync(searchId);
                        if (byId is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        await PrintOneAsync(byId);
                        break;

                    case 3:
                        await PrintFlightsAsync();

                        Console.Write("\nIngrese vuelo_id: ");
                        int searchFlightId = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getByFlightId.ExecuteAsync(searchFlightId));
                        break;

                    case 4:
                        await PrintStaffAsync();

                        Console.Write("\nIngrese personal_id (staff.id): ");
                        int searchStaffId = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getByStaffId.ExecuteAsync(searchStaffId));
                        break;

                    case 5:
                        await PrintFlightRolesAsync();

                        Console.Write("\nIngrese rol_vuelo_id: ");
                        int searchRoleId = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getByFlightRoleId.ExecuteAsync(searchRoleId));
                        break;

                    case 6:
                        await PrintFlightsAsync();
                        await PrintStaffAsync();

                        Console.Write("\nIngrese vuelo_id: ");
                        int keyFlightId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese personal_id (staff.id): ");
                        int keyStaffId = int.Parse(Console.ReadLine()!);

                        var byPair = await _getByFlightAndStaff.ExecuteAsync(keyFlightId, keyStaffId);
                        if (byPair is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        await PrintOneAsync(byPair);
                        break;

                    case 7:
                        await PrintAssignmentsForSelectionAsync();
                        await PrintFlightsAsync();
                        await PrintStaffAsync();
                        await PrintFlightRolesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese vuelo_id: ");
                        int newFlightId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese personal_id (staff.id): ");
                        int newStaffId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese rol_vuelo_id: ");
                        int newFlightRoleId = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newFlightId, newStaffId, newFlightRoleId);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 8:
                        await PrintAssignmentsForSelectionAsync();

                        Console.Write("\nIngrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("âœ” Eliminado");
                        break;

                    case 9:
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"âŒ Error: {ex.GetBaseException().Message}");
            }

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private async Task PrintAssignmentsForSelectionAsync()
    {
        Console.WriteLine($"FlightAssignments (top {TopCount}):");
        var list = (await _getAll.ExecuteAsync()).Take(TopCount).ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        await PrintListAsync(list);
    }

    private async Task PrintListAsync(IEnumerable<FlightAssignment> list)
    {
        var items = list.ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("(sin resultados)");
            return;
        }

        var flightMap = await GetFlightDisplayMapAsync();
        var staffMap = await GetStaffDisplayMapAsync();
        var flightRoleMap = await GetFlightRoleDisplayMapAsync();

        foreach (var item in items)
            Console.WriteLine(Format(item, flightMap, staffMap, flightRoleMap));
    }

    private async Task PrintOneAsync(FlightAssignment item)
    {
        var flightMap = await GetFlightDisplayMapAsync();
        var staffMap = await GetStaffDisplayMapAsync();
        var flightRoleMap = await GetFlightRoleDisplayMapAsync();
        Console.WriteLine(Format(item, flightMap, staffMap, flightRoleMap));
    }

    private async Task PrintFlightsAsync()
    {
        var flights = (await _getAllFlights.ExecuteAsync()).ToList();
        var flightMap = await GetFlightDisplayMapAsync();

        Console.WriteLine("Vuelos disponibles:");
        PrintTopWithFormat(flights, f => $"{f.Id.Value} - {GetDisplay(flightMap, f.Id.Value)}");

        Console.Write("Buscar vuelo (codigo/ruta) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = flights
            .Where(f => GetDisplay(flightMap, f.Id.Value).ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, f => $"{f.Id.Value} - {GetDisplay(flightMap, f.Id.Value)}");
    }

    private async Task PrintStaffAsync()
    {
        var staff = (await _getAllStaff.ExecuteAsync()).ToList();
        var staffMap = await GetStaffDisplayMapAsync();

        Console.WriteLine("\nStaff disponible:");
        PrintTopWithFormat(staff, s => $"{s.Id.Value} - {GetDisplay(staffMap, s.Id.Value)}");

        Console.Write("Buscar staff (nombre/apellido) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = staff
            .Where(s => GetDisplay(staffMap, s.Id.Value).ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, s => $"{s.Id.Value} - {GetDisplay(staffMap, s.Id.Value)}");
    }

    private async Task PrintFlightRolesAsync()
    {
        var roles = (await _getAllFlightRoles.ExecuteAsync()).ToList();
        Console.WriteLine("\nRoles de vuelo disponibles:");
        PrintTopWithFormat(roles, r => $"{r.Id.Value} - {r.Name.Value}");

        Console.Write("Buscar rol (texto) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = roles
            .Where(r => r.Name.Value.ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, r => $"{r.Id.Value} - {r.Name.Value}");
    }

    private static void PrintTopWithFormat<T>(IEnumerable<T> items, Func<T, string> formatter)
    {
        var list = items.Take(TopCount).ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        foreach (var item in list)
            Console.WriteLine(formatter(item));
    }

    private async Task<Dictionary<int, string>> GetFlightDisplayMapAsync()
    {
        var flights = await _getAllFlights.ExecuteAsync();
        var airlines = await _getAllAirlines.ExecuteAsync();
        var routes = await _getAllRoutes.ExecuteAsync();
        var airports = await _getAllAirports.ExecuteAsync();

        var airlineMap = airlines.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
        var airportMap = airports.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
        var routeMap = routes.ToDictionary(
            r => r.Id.Value,
            r =>
            {
                var origin = GetDisplay(airportMap, r.OriginAirportId.Value);
                var dest = GetDisplay(airportMap, r.DestinationAirportId.Value);
                return $"{origin} -> {dest}";
            });

        return flights.ToDictionary(
            f => f.Id.Value,
            f =>
            {
                var airline = GetDisplay(airlineMap, f.AirlineId.Value);
                var route = GetDisplay(routeMap, f.RouteId.Value);
                return $"{f.Code.Value} - {airline} - {route} - dep={f.DepartureDateTime.Value:yyyy-MM-dd HH:mm}";
            });
    }

    private async Task<Dictionary<int, string>> GetStaffDisplayMapAsync()
    {
        var staff = await _getAllStaff.ExecuteAsync();
        var people = await _getAllPeople.ExecuteAsync();
        var staffRoles = await _getAllStaffRoles.ExecuteAsync();
        var airlines = await _getAllAirlines.ExecuteAsync();
        var airports = await _getAllAirports.ExecuteAsync();

        var personMap = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
        var staffRoleMap = staffRoles.ToDictionary(r => r.Id.Value, r => r.Name.Value);
        var airlineMap = airlines.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
        var airportMap = airports.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");

        return staff.ToDictionary(
            s => s.Id.Value,
            s =>
            {
                var person = GetDisplay(personMap, s.PersonId.Value);
                var role = GetDisplay(staffRoleMap, s.RoleId.Value);
                var airline = s.AirlineId.Value.HasValue ? GetDisplay(airlineMap, s.AirlineId.Value.Value) : "NULL";
                var airport = s.AirportId.Value.HasValue ? GetDisplay(airportMap, s.AirportId.Value.Value) : "NULL";
                var active = s.IsActive.Value ? "active" : "inactive";
                return $"{person} - {role} - airline={airline} - airport={airport} - {active}";
            });
    }

    private async Task<Dictionary<int, string>> GetFlightRoleDisplayMapAsync()
    {
        var roles = await _getAllFlightRoles.ExecuteAsync();
        return roles.ToDictionary(r => r.Id.Value, r => r.Name.Value);
    }

    private static string Format(
        FlightAssignment item,
        Dictionary<int, string> flightMap,
        Dictionary<int, string> staffMap,
        Dictionary<int, string> flightRoleMap)
    {
        var flightDisplay = GetDisplay(flightMap, item.FlightId.Value);
        var staffDisplay = GetDisplay(staffMap, item.StaffId.Value);
        var roleDisplay = GetDisplay(flightRoleMap, item.FlightRoleId.Value);

        return $"{item.Id.Value} - flight={flightDisplay} [{item.FlightId.Value}] - staff={staffDisplay} [{item.StaffId.Value}] - role={roleDisplay} [{item.FlightRoleId.Value}]";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? display : $"#{id}";
    }
}


