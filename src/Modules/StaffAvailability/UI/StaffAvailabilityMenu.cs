// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\UI\StaffAvailabilityMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.Staff.Application.UseCases;
using GestionAerolineas.src.Modules.StaffAvailability.Application.UseCases;
using GestionAerolineas.src.Modules.StaffAvailability.Domain.Aggregate;
using GestionAerolineas.src.Modules.StaffRoles.Application.UseCases;

namespace GestionAerolineas.src.Modules.StaffAvailability.UI;

public class StaffAvailabilityMenu
{
    private readonly CreateStaffAvailabilityUseCase _create;
    private readonly GetAllStaffAvailabilityUseCase _getAll;
    private readonly GetStaffAvailabilityByIdUseCase _getById;
    private readonly GetStaffAvailabilityByStaffIdUseCase _getByStaffId;
    private readonly GetStaffAvailabilityByStatusIdUseCase _getByStatusId;
    private readonly GetActiveStaffAvailabilityNowByStaffIdUseCase _getActiveNow;
    private readonly UpdateStaffAvailabilityUseCase _update;
    private readonly DeleteStaffAvailabilityUseCase _delete;

    private readonly GetAllStaffUseCase _getAllStaff;
    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllStaffRolesUseCase _getAllStaffRoles;
    private readonly GetAllAirlinesUseCase _getAllAirlines;
    private readonly GetAllAirportsUseCase _getAllAirports;
    private readonly GetAllAvailabilityStatusesUseCase _getAllAvailabilityStatuses;

    public StaffAvailabilityMenu(
        CreateStaffAvailabilityUseCase create,
        GetAllStaffAvailabilityUseCase getAll,
        GetStaffAvailabilityByIdUseCase getById,
        GetStaffAvailabilityByStaffIdUseCase getByStaffId,
        GetStaffAvailabilityByStatusIdUseCase getByStatusId,
        GetActiveStaffAvailabilityNowByStaffIdUseCase getActiveNow,
        UpdateStaffAvailabilityUseCase update,
        DeleteStaffAvailabilityUseCase delete,
        GetAllStaffUseCase getAllStaff,
        GetAllPeopleUseCase getAllPeople,
        GetAllStaffRolesUseCase getAllStaffRoles,
        GetAllAirlinesUseCase getAllAirlines,
        GetAllAirportsUseCase getAllAirports,
        GetAllAvailabilityStatusesUseCase getAllAvailabilityStatuses)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByStaffId = getByStaffId;
        _getByStatusId = getByStatusId;
        _getActiveNow = getActiveNow;
        _update = update;
        _delete = delete;
        _getAllStaff = getAllStaff;
        _getAllPeople = getAllPeople;
        _getAllStaffRoles = getAllStaffRoles;
        _getAllAirlines = getAllAirlines;
        _getAllAirports = getAllAirports;
        _getAllAvailabilityStatuses = getAllAvailabilityStatuses;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear staff availability block",
            "Listar staff availability",
            "Get availability by ID",
            "Get availability by staff_id",
            "Get availability by status_id",
            "Get active availability NOW by staff_id",
            "Actualizar availability block",
            "Eliminar availability block",
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
                        await PrintStaffAsync();
                        await PrintAvailabilityStatusesAsync();

                        Console.Write("\nIngrese personal_id (staff.id): ");
                        int staffId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese estado_disponibilidad_id: ");
                        int statusId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_inicio (yyyy-MM-dd HH:mm): ");
                        var start = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_fin (yyyy-MM-dd HH:mm): ");
                        var end = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese observacion [opcional]: ");
                        var observation = Console.ReadLine();

                        await _create.ExecuteAsync(staffId, statusId, start, end, observation);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        await PrintListAsync(await _getAll.ExecuteAsync());
                        break;

                    case 2:
                        await PrintAvailabilityForSelectionAsync();

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
                        await PrintStaffAsync();

                        Console.Write("\nIngrese personal_id: ");
                        int searchStaffId = int.Parse(Console.ReadLine()!);

                        var byStaff = await _getByStaffId.ExecuteAsync(searchStaffId);
                        await PrintListAsync(byStaff);
                        break;

                    case 4:
                        await PrintAvailabilityStatusesAsync();

                        Console.Write("\nIngrese estado_disponibilidad_id: ");
                        int searchStatusId = int.Parse(Console.ReadLine()!);

                        var byStatus = await _getByStatusId.ExecuteAsync(searchStatusId);
                        await PrintListAsync(byStatus);
                        break;

                    case 5:
                        await PrintStaffAsync();

                        Console.Write("\nIngrese personal_id: ");
                        int nowStaffId = int.Parse(Console.ReadLine()!);

                        var now = DateTime.Now;
                        var activeNow = await _getActiveNow.ExecuteAsync(nowStaffId, now);
                        if (activeNow is null)
                        {
                            Console.WriteLine("No hay bloque activo en este momento");
                            break;
                        }

                        await PrintOneAsync(activeNow);
                        break;

                    case 6:
                        await PrintAvailabilityForSelectionAsync();
                        await PrintStaffAsync();
                        await PrintAvailabilityStatusesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese personal_id (staff.id): ");
                        int newStaffId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese estado_disponibilidad_id: ");
                        int newStatusId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_inicio (yyyy-MM-dd HH:mm): ");
                        var newStart = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_fin (yyyy-MM-dd HH:mm): ");
                        var newEnd = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese observacion [opcional]: ");
                        var newObs = Console.ReadLine();

                        await _update.ExecuteAsync(updateId, newStaffId, newStatusId, newStart, newEnd, newObs);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 7:
                        await PrintAvailabilityForSelectionAsync();

                        Console.Write("\nIngrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("âœ” Eliminado");
                        break;

                    case 8:
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

    private async Task PrintAvailabilityForSelectionAsync()
    {
        Console.WriteLine("Disponibilidad (primeros 30):");
        var list = (await _getAll.ExecuteAsync()).Take(30).ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        var staffMap = await GetStaffDisplayMapAsync();
        var statusMap = await GetAvailabilityStatusDisplayMapAsync();

        foreach (var item in list)
            Console.WriteLine(Format(item, staffMap, statusMap));
    }

    private async Task PrintListAsync(IEnumerable<StaffAvailabilityBlock> list)
    {
        var items = list.ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("(sin resultados)");
            return;
        }

        var staffMap = await GetStaffDisplayMapAsync();
        var statusMap = await GetAvailabilityStatusDisplayMapAsync();

        foreach (var item in items)
            Console.WriteLine(Format(item, staffMap, statusMap));
    }

    private async Task PrintOneAsync(StaffAvailabilityBlock item)
    {
        var staffMap = await GetStaffDisplayMapAsync();
        var statusMap = await GetAvailabilityStatusDisplayMapAsync();
        Console.WriteLine(Format(item, staffMap, statusMap));
    }

    private async Task PrintStaffAsync()
    {
        Console.WriteLine("Staff disponible:");
        var staff = (await _getAllStaff.ExecuteAsync()).ToList();
        var staffMap = await GetStaffDisplayMapAsync();

        foreach (var s in staff.Take(30))
            Console.WriteLine($"{s.Id.Value} - {GetDisplay(staffMap, s.Id.Value)}");

        if (staff.Count > 30)
            Console.WriteLine("(Mostrando solo los primeros 30)");
    }

    private async Task PrintAvailabilityStatusesAsync()
    {
        Console.WriteLine("\nEstados de disponibilidad:");
        var statuses = (await _getAllAvailabilityStatuses.ExecuteAsync()).ToList();

        foreach (var st in statuses.Take(30))
            Console.WriteLine($"{st.Id.Value} - {st.Name.Value}");

        if (statuses.Count > 30)
            Console.WriteLine("(Mostrando solo los primeros 30)");
    }

    private async Task<Dictionary<int, string>> GetStaffDisplayMapAsync()
    {
        var staff = await _getAllStaff.ExecuteAsync();
        var people = await _getAllPeople.ExecuteAsync();
        var roles = await _getAllStaffRoles.ExecuteAsync();
        var airlines = await _getAllAirlines.ExecuteAsync();
        var airports = await _getAllAirports.ExecuteAsync();

        var personMap = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
        var roleMap = roles.ToDictionary(r => r.Id.Value, r => r.Name.Value);
        var airlineMap = airlines.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
        var airportMap = airports.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");

        return staff.ToDictionary(
            s => s.Id.Value,
            s =>
            {
                var personDisplay = GetDisplay(personMap, s.PersonId.Value);
                var roleDisplay = GetDisplay(roleMap, s.RoleId.Value);
                var airlineDisplay = s.AirlineId.Value.HasValue ? GetDisplay(airlineMap, s.AirlineId.Value.Value) : "NULL";
                var airportDisplay = s.AirportId.Value.HasValue ? GetDisplay(airportMap, s.AirportId.Value.Value) : "NULL";
                var activeDisplay = s.IsActive.Value ? "active" : "inactive";
                return $"{personDisplay} - {roleDisplay} - airline={airlineDisplay} - airport={airportDisplay} - {activeDisplay}";
            });
    }

    private async Task<Dictionary<int, string>> GetAvailabilityStatusDisplayMapAsync()
    {
        var statuses = await _getAllAvailabilityStatuses.ExecuteAsync();
        return statuses.ToDictionary(s => s.Id.Value, s => s.Name.Value);
    }

    private static string Format(StaffAvailabilityBlock item, Dictionary<int, string> staffMap, Dictionary<int, string> statusMap)
    {
        string staffDisplay = GetDisplay(staffMap, item.StaffId.Value);
        string statusDisplay = GetDisplay(statusMap, item.StatusId.Value);
        var obsDisplay = item.Observation.Value ?? "NULL";

        return $"{item.Id.Value} - staff={staffDisplay} [{item.StaffId.Value}] - status={statusDisplay} [{item.StatusId.Value}] - start={item.StartDateTime.Value:yyyy-MM-dd HH:mm} - end={item.EndDateTime.Value:yyyy-MM-dd HH:mm} - obs={obsDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? display : $"#{id}";
    }
}

