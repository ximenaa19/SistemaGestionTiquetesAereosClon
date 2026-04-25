// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\UI\StaffMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.Staff.Application.UseCases;
using GestionAerolineas.src.Modules.Staff.Domain.Aggregate;
using GestionAerolineas.src.Modules.StaffRoles.Application.UseCases;

namespace GestionAerolineas.src.Modules.Staff.UI;

public class StaffMenu
{
    private readonly CreateStaffUseCase _create;
    private readonly GetAllStaffUseCase _getAll;
    private readonly GetStaffByIdUseCase _getById;
    private readonly GetStaffByPersonIdUseCase _getByPersonId;
    private readonly GetStaffByRoleIdUseCase _getByRoleId;
    private readonly SearchStaffByPersonNameOrLastNameUseCase _searchByName;
    private readonly GetActiveStaffUseCase _getActive;
    private readonly GetInactiveStaffUseCase _getInactive;
    private readonly UpdateStaffUseCase _update;
    private readonly DeleteStaffUseCase _delete;

    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllStaffRolesUseCase _getAllStaffRoles;
    private readonly GetAllAirlinesUseCase _getAllAirlines;
    private readonly GetAllAirportsUseCase _getAllAirports;

    public StaffMenu(
        CreateStaffUseCase create,
        GetAllStaffUseCase getAll,
        GetStaffByIdUseCase getById,
        GetStaffByPersonIdUseCase getByPersonId,
        GetStaffByRoleIdUseCase getByRoleId,
        SearchStaffByPersonNameOrLastNameUseCase searchByName,
        GetActiveStaffUseCase getActive,
        GetInactiveStaffUseCase getInactive,
        UpdateStaffUseCase update,
        DeleteStaffUseCase delete,
        GetAllPeopleUseCase getAllPeople,
        GetAllStaffRolesUseCase getAllStaffRoles,
        GetAllAirlinesUseCase getAllAirlines,
        GetAllAirportsUseCase getAllAirports)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByPersonId = getByPersonId;
        _getByRoleId = getByRoleId;
        _searchByName = searchByName;
        _getActive = getActive;
        _getInactive = getInactive;
        _update = update;
        _delete = delete;
        _getAllPeople = getAllPeople;
        _getAllStaffRoles = getAllStaffRoles;
        _getAllAirlines = getAllAirlines;
        _getAllAirports = getAllAirports;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear staff member",
            "Listar staff",
            "Get staff by ID",
            "Get staff by person_id",
            "Get staff by role_id",
            "Buscar staff por nombre/apellido",
            "Get active staff",
            "Get inactive staff",
            "Actualizar staff member",
            "Eliminar staff member",
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
                        await PrintPeopleAsync();
                        await PrintStaffRolesAsync();
                        await PrintAirlinesAsync();
                        await PrintAirportsAsync();

                        Console.Write("\nIngrese persona_id: ");
                        int personId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese cargo_id: ");
                        int roleId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aerolinea_id [opcional]: ");
                        int? airlineId = ReadNullableInt(Console.ReadLine());

                        Console.Write("Ingrese aeropuerto_id [opcional]: ");
                        int? airportId = ReadNullableInt(Console.ReadLine());

                        Console.Write("Ingrese fecha_ingreso (yyyy-MM-dd): ");
                        var hireDate = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese activo (true/false) [default=true]: ");
                        var activeInput = Console.ReadLine();
                        bool isActive = string.IsNullOrWhiteSpace(activeInput) ? true : bool.Parse(activeInput);

                        await _create.ExecuteAsync(personId, roleId, airlineId, airportId, hireDate, isActive);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        await PrintListAsync(await _getAll.ExecuteAsync());
                        break;

                    case 2:
                        await PrintStaffForSelectionAsync();

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
                        await PrintPeopleAsync();

                        Console.Write("\nIngrese persona_id: ");
                        int searchPersonId = int.Parse(Console.ReadLine()!);

                        var byPerson = await _getByPersonId.ExecuteAsync(searchPersonId);
                        if (byPerson is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        await PrintOneAsync(byPerson);
                        break;

                    case 4:
                        await PrintStaffRolesAsync();

                        Console.Write("\nIngrese cargo_id: ");
                        int searchRoleId = int.Parse(Console.ReadLine()!);

                        var byRole = await _getByRoleId.ExecuteAsync(searchRoleId);
                        await PrintListAsync(byRole);
                        break;

                    case 5:
                        Console.Write("Ingrese texto (nombre o apellido): ");
                        string searchText = Console.ReadLine() ?? string.Empty;

                        var byName = await _searchByName.ExecuteAsync(searchText);
                        await PrintListAsync(byName);
                        break;

                    case 6:
                        await PrintListAsync(await _getActive.ExecuteAsync());
                        break;

                    case 7:
                        await PrintListAsync(await _getInactive.ExecuteAsync());
                        break;

                    case 8:
                        await PrintStaffForSelectionAsync();
                        await PrintPeopleAsync();
                        await PrintStaffRolesAsync();
                        await PrintAirlinesAsync();
                        await PrintAirportsAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese persona_id: ");
                        int newPersonId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese cargo_id: ");
                        int newRoleId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese aerolinea_id [opcional]: ");
                        int? newAirlineId = ReadNullableInt(Console.ReadLine());

                        Console.Write("Ingrese aeropuerto_id [opcional]: ");
                        int? newAirportId = ReadNullableInt(Console.ReadLine());

                        Console.Write("Ingrese fecha_ingreso (yyyy-MM-dd): ");
                        var newHireDate = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese activo (true/false): ");
                        bool newIsActive = bool.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newPersonId, newRoleId, newAirlineId, newAirportId, newHireDate, newIsActive);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 9:
                        await PrintStaffForSelectionAsync();

                        Console.Write("\nIngrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("âœ” Eliminado");
                        break;

                    case 10:
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

    private async Task PrintStaffForSelectionAsync()
    {
        Console.WriteLine("Staff disponible (primeros 30):");
        var list = (await _getAll.ExecuteAsync()).Take(30).ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        var personMap = await GetPersonDisplayMapAsync();
        var roleMap = await GetRoleDisplayMapAsync();
        var airlineMap = await GetAirlineDisplayMapAsync();
        var airportMap = await GetAirportDisplayMapAsync();

        foreach (var item in list)
            Console.WriteLine(Format(item, personMap, roleMap, airlineMap, airportMap));
    }

    private async Task PrintListAsync(IEnumerable<StaffMember> list)
    {
        var items = list.ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("(sin resultados)");
            return;
        }

        var personMap = await GetPersonDisplayMapAsync();
        var roleMap = await GetRoleDisplayMapAsync();
        var airlineMap = await GetAirlineDisplayMapAsync();
        var airportMap = await GetAirportDisplayMapAsync();

        foreach (var item in items)
            Console.WriteLine(Format(item, personMap, roleMap, airlineMap, airportMap));
    }

    private async Task PrintOneAsync(StaffMember item)
    {
        var personMap = await GetPersonDisplayMapAsync();
        var roleMap = await GetRoleDisplayMapAsync();
        var airlineMap = await GetAirlineDisplayMapAsync();
        var airportMap = await GetAirportDisplayMapAsync();
        Console.WriteLine(Format(item, personMap, roleMap, airlineMap, airportMap));
    }

    private async Task PrintPeopleAsync()
    {
        Console.WriteLine("People disponibles:");
        var people = (await _getAllPeople.ExecuteAsync()).ToList();

        foreach (var p in people.Take(30))
            Console.WriteLine($"{p.Id.Value} - {p.FirstNames.Value} {p.LastNames.Value} - doc={p.DocumentTypeId.Value}/{p.DocumentNumber.Value}");

        if (people.Count > 30)
            Console.WriteLine("(Mostrando solo las primeras 30)");
    }

    private async Task PrintStaffRolesAsync()
    {
        Console.WriteLine("\nCargos disponibles:");
        var roles = (await _getAllStaffRoles.ExecuteAsync()).ToList();

        foreach (var r in roles.Take(30))
            Console.WriteLine($"{r.Id.Value} - {r.Name.Value}");

        if (roles.Count > 30)
            Console.WriteLine("(Mostrando solo los primeros 30)");
    }

    private async Task PrintAirlinesAsync()
    {
        Console.WriteLine("\nAerolineas disponibles:");
        var airlines = (await _getAllAirlines.ExecuteAsync()).ToList();

        foreach (var a in airlines.Take(30))
            Console.WriteLine($"{a.Id.Value} - {a.Name.Value} - iata={a.IataCode.Value}");

        if (airlines.Count > 30)
            Console.WriteLine("(Mostrando solo las primeras 30)");
    }

    private async Task PrintAirportsAsync()
    {
        Console.WriteLine("\nAeropuertos disponibles:");
        var airports = (await _getAllAirports.ExecuteAsync()).ToList();

        foreach (var a in airports.Take(30))
            Console.WriteLine($"{a.Id.Value} - {a.Name.Value} - iata={a.IataCode.Value} - city_id={a.CityId.Value}");

        if (airports.Count > 30)
            Console.WriteLine("(Mostrando solo los primeros 30)");
    }

    private async Task<Dictionary<int, string>> GetPersonDisplayMapAsync()
    {
        var people = await _getAllPeople.ExecuteAsync();
        return people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
    }

    private async Task<Dictionary<int, string>> GetRoleDisplayMapAsync()
    {
        var roles = await _getAllStaffRoles.ExecuteAsync();
        return roles.ToDictionary(r => r.Id.Value, r => r.Name.Value);
    }

    private async Task<Dictionary<int, string>> GetAirlineDisplayMapAsync()
    {
        var airlines = await _getAllAirlines.ExecuteAsync();
        return airlines.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
    }

    private async Task<Dictionary<int, string>> GetAirportDisplayMapAsync()
    {
        var airports = await _getAllAirports.ExecuteAsync();
        return airports.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
    }

    private static string Format(
        StaffMember item,
        Dictionary<int, string> personMap,
        Dictionary<int, string> roleMap,
        Dictionary<int, string> airlineMap,
        Dictionary<int, string> airportMap)
    {
        string personDisplay = GetDisplay(personMap, item.PersonId.Value);
        string roleDisplay = GetDisplay(roleMap, item.RoleId.Value);
        string airlineDisplay = item.AirlineId.Value.HasValue ? GetDisplay(airlineMap, item.AirlineId.Value.Value) : "NULL";
        string airportDisplay = item.AirportId.Value.HasValue ? GetDisplay(airportMap, item.AirportId.Value.Value) : "NULL";
        var activeDisplay = item.IsActive.Value ? "active" : "inactive";

        return $"{item.Id.Value} - person={personDisplay} - role={roleDisplay} - airline={airlineDisplay} - airport={airportDisplay} - hire={item.HireDate.Value:yyyy-MM-dd} - {activeDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }

    private static int? ReadNullableInt(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        return int.Parse(input);
    }
}


