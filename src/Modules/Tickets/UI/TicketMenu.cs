// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\UI\TicketMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Globalization;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.TicketStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.Tickets.Application.UseCases;
using GestionAerolineas.src.Modules.Tickets.Domain.Aggregate;
using GestionAerolineas.src.Modules.TicketStatuses.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.Tickets.UI;

public class TicketMenu
{
    private const int TopCount = 10;

    private readonly CreateTicketUseCase _create;
    private readonly GetAllTicketsUseCase _getAll;
    private readonly GetTicketByIdUseCase _getById;
    private readonly GetTicketByCodeUseCase _getByCode;
    private readonly GetTicketByReservationPassengerIdUseCase _getByReservationPassengerId;
    private readonly GetTicketsByStatusIdUseCase _getByStatusId;
    private readonly GetTicketsByPassengerIdUseCase _getByPassengerId;
    private readonly GetTicketsByReservationCodeUseCase _getByReservationCode;
    private readonly UpdateTicketUseCase _update;
    private readonly DeleteTicketUseCase _delete;

    private readonly GetAllTicketStatusesUseCase _getAllStatuses;
    private readonly GetAllReservationPassengersUseCase _getAllReservationPassengers;
    private readonly GetAllReservationFlightsUseCase _getAllReservationFlights;
    private readonly GetAllReservationsUseCase _getAllReservations;
    private readonly GetAllFlightsUseCase _getAllFlights;
    private readonly GetAllPassengersUseCase _getAllPassengers;
    private readonly GetAllPeopleUseCase _getAllPeople;

    public TicketMenu(
        CreateTicketUseCase create,
        GetAllTicketsUseCase getAll,
        GetTicketByIdUseCase getById,
        GetTicketByCodeUseCase getByCode,
        GetTicketByReservationPassengerIdUseCase getByReservationPassengerId,
        GetTicketsByStatusIdUseCase getByStatusId,
        GetTicketsByPassengerIdUseCase getByPassengerId,
        GetTicketsByReservationCodeUseCase getByReservationCode,
        UpdateTicketUseCase update,
        DeleteTicketUseCase delete,
        GetAllTicketStatusesUseCase getAllStatuses,
        GetAllReservationPassengersUseCase getAllReservationPassengers,
        GetAllReservationFlightsUseCase getAllReservationFlights,
        GetAllReservationsUseCase getAllReservations,
        GetAllFlightsUseCase getAllFlights,
        GetAllPassengersUseCase getAllPassengers,
        GetAllPeopleUseCase getAllPeople)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByCode = getByCode;
        _getByReservationPassengerId = getByReservationPassengerId;
        _getByStatusId = getByStatusId;
        _getByPassengerId = getByPassengerId;
        _getByReservationCode = getByReservationCode;
        _update = update;
        _delete = delete;
        _getAllStatuses = getAllStatuses;
        _getAllReservationPassengers = getAllReservationPassengers;
        _getAllReservationFlights = getAllReservationFlights;
        _getAllReservations = getAllReservations;
        _getAllFlights = getAllFlights;
        _getAllPassengers = getAllPassengers;
        _getAllPeople = getAllPeople;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear ticket",
            "Listar tickets",
            "Get ticket by ID",
            "Get ticket by codigo_tiquete",
            "Get ticket by reserva_pasajero_id",
            "Get tickets by estado_tiquete_id",
            "Get tickets by passenger_id",
            "Get tickets by reservation code (PNR)",
            "Actualizar ticket",
            "Eliminar ticket",
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
                        await PrintReservationPassengersAsync();
                        await PrintStatusesAsync();

                        Console.Write("\nIngrese reserva_pasajero_id: ");
                        int rpId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_emision (yyyy-MM-dd HH:mm) [default=now]: ");
                        var issuedInput = Console.ReadLine();
                        DateTime? issuedAt = string.IsNullOrWhiteSpace(issuedInput)
                            ? null
                            : DateTime.Parse(issuedInput!, CultureInfo.InvariantCulture);

                        Console.Write("Ingrese estado_tiquete_id: ");
                        int statusId = int.Parse(Console.ReadLine()!);

                        var created = await _create.ExecuteAsync(rpId, issuedAt, statusId);
                        Console.WriteLine($"âœ” Creado: id={created.Id.Value} - code={created.Code.Value}");
                        break;

                    case 1:
                        await PrintListAsync(await _getAll.ExecuteAsync());
                        break;

                    case 2:
                        await PrintTicketsForSelectionAsync();
                        Console.Write("\nIngrese el ID: ");
                        int id = int.Parse(Console.ReadLine()!);
                        var byId = await _getById.ExecuteAsync(id);
                        if (byId is null)
                        {
                            Console.WriteLine("(sin registros)");
                            break;
                        }
                        await PrintOneAsync(byId);
                        break;

                    case 3:
                        Console.Write("Ingrese codigo_tiquete: ");
                        var code = Console.ReadLine() ?? string.Empty;
                        var byCode = await _getByCode.ExecuteAsync(code);
                        if (byCode is null)
                        {
                            Console.WriteLine("(sin registros)");
                            break;
                        }
                        await PrintOneAsync(byCode);
                        break;

                    case 4:
                        await PrintReservationPassengersAsync();
                        Console.Write("\nIngrese reserva_pasajero_id: ");
                        int searchRpId = int.Parse(Console.ReadLine()!);
                        var byRp = await _getByReservationPassengerId.ExecuteAsync(searchRpId);
                        if (byRp is null)
                        {
                            Console.WriteLine("(sin registros)");
                            break;
                        }
                        await PrintOneAsync(byRp);
                        break;

                    case 5:
                        await PrintStatusesAsync();
                        Console.Write("\nIngrese estado_tiquete_id: ");
                        int stId = int.Parse(Console.ReadLine()!);
                        await PrintListAsync(await _getByStatusId.ExecuteAsync(stId));
                        break;

                    case 6:
                        await PrintPassengersAsync();
                        Console.Write("\nIngrese passenger_id: ");
                        int passengerId = int.Parse(Console.ReadLine()!);
                        await PrintListAsync(await _getByPassengerId.ExecuteAsync(passengerId));
                        break;

                    case 7:
                        Console.Write("Ingrese el codigo_reserva (PNR): ");
                        var pnr = Console.ReadLine() ?? string.Empty;
                        await PrintListAsync(await _getByReservationCode.ExecuteAsync(pnr));
                        break;

                    case 8:
                        await PrintTicketsForSelectionAsync();
                        await PrintReservationPassengersAsync();
                        await PrintStatusesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese reserva_pasajero_id: ");
                        int updateRpId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese codigo_tiquete: ");
                        var updateCode = Console.ReadLine() ?? string.Empty;

                        Console.Write("Ingrese fecha_emision (yyyy-MM-dd HH:mm): ");
                        var updateIssuedAt = DateTime.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

                        Console.Write("Ingrese estado_tiquete_id: ");
                        int updateStatusId = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, updateRpId, updateCode, updateIssuedAt, updateStatusId);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 9:
                        await PrintTicketsForSelectionAsync();
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
        }
    }

    private async Task PrintTicketsForSelectionAsync()
    {
        Console.WriteLine("Tickets (primeros 30):");
        var list = (await _getAll.ExecuteAsync()).Take(30).ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        var statusMap = await GetStatusMapAsync();
        var rpMap = await GetReservationPassengerMapAsync();

        foreach (var item in list)
            Console.WriteLine(Format(item, statusMap, rpMap));
    }

    private async Task PrintListAsync(IEnumerable<Ticket> list)
    {
        var items = list.ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("(sin resultados)");
            return;
        }

        var statusMap = await GetStatusMapAsync();
        var rpMap = await GetReservationPassengerMapAsync();

        foreach (var item in items)
            Console.WriteLine(Format(item, statusMap, rpMap));
    }

    private async Task PrintOneAsync(Ticket item)
    {
        var statusMap = await GetStatusMapAsync();
        var rpMap = await GetReservationPassengerMapAsync();
        Console.WriteLine(Format(item, statusMap, rpMap));
    }

    private async Task PrintStatusesAsync()
    {
        var statuses = (await _getAllStatuses.ExecuteAsync()).ToList();
        Console.WriteLine("TicketStatuses:");
        foreach (var s in statuses.Take(30))
            Console.WriteLine($"{s.Id.Value} - {s.Name.Value}");

        if (statuses.Count > 30)
            Console.WriteLine("(Mostrando solo los primeros 30)");
    }

    private async Task PrintPassengersAsync()
    {
        var passengers = (await _getAllPassengers.ExecuteAsync()).ToList();
        var people = (await _getAllPeople.ExecuteAsync()).ToList();

        var personMap = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
        Console.WriteLine("Passengers (top 10):");

        foreach (var p in passengers.Take(TopCount))
        {
            var name = personMap.TryGetValue(p.PersonId.Value, out var n) ? n : $"#{p.PersonId.Value}";
            Console.WriteLine($"{p.Id.Value} - {name}");
        }

        Console.Write("Buscar pasajero (texto) [opcional]: ");
        var search = (Console.ReadLine() ?? string.Empty).Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalized = search.ToUpperInvariant();
            var matches = passengers
                .Select(p =>
                {
                    var name = personMap.TryGetValue(p.PersonId.Value, out var n) ? n : string.Empty;
                    return new { p, key = name.ToUpperInvariant() };
                })
                .Where(x => x.key.Contains(normalized))
                .Select(x => x.p)
                .Take(TopCount)
                .ToList();

            Console.WriteLine("\nCoincidencias (top 10):");
            if (matches.Count == 0)
            {
                Console.WriteLine("(sin registros)");
            }
            else
            {
                foreach (var p in matches)
                {
                    var name = personMap.TryGetValue(p.PersonId.Value, out var n) ? n : $"#{p.PersonId.Value}";
                    Console.WriteLine($"{p.Id.Value} - {name}");
                }
            }
        }
    }

    private async Task PrintReservationPassengersAsync()
    {
        var map = await GetReservationPassengerMapAsync();
        var items = map
            .OrderByDescending(kv => kv.Key)
            .Take(TopCount)
            .ToList();

        Console.WriteLine("ReservationPassengers (top 10):");
        if (items.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        foreach (var kv in items)
            Console.WriteLine($"{kv.Key} - {kv.Value}");

        Console.Write("Buscar reserva/pasajero/vuelo (texto) [opcional]: ");
        var search = (Console.ReadLine() ?? string.Empty).Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalized = search.ToUpperInvariant();
            var matches = map
                .Where(kv => kv.Value.ToUpperInvariant().Contains(normalized))
                .OrderByDescending(kv => kv.Key)
                .Take(TopCount)
                .ToList();

            Console.WriteLine("\nCoincidencias (top 10):");
            if (matches.Count == 0)
                Console.WriteLine("(sin registros)");
            else
                foreach (var kv in matches)
                    Console.WriteLine($"{kv.Key} - {kv.Value}");
        }
    }

    private async Task<Dictionary<int, string>> GetStatusMapAsync()
    {
        var statuses = await _getAllStatuses.ExecuteAsync();
        return statuses.ToDictionary(s => s.Id.Value, s => s.Name.Value);
    }

    private async Task<Dictionary<int, string>> GetReservationPassengerMapAsync()
    {
        var reservationPassengers = (await _getAllReservationPassengers.ExecuteAsync()).ToList();
        var reservationFlights = (await _getAllReservationFlights.ExecuteAsync()).ToList();
        var reservations = (await _getAllReservations.ExecuteAsync()).ToList();
        var flights = (await _getAllFlights.ExecuteAsync()).ToList();
        var passengers = (await _getAllPassengers.ExecuteAsync()).ToList();
        var people = (await _getAllPeople.ExecuteAsync()).ToList();

        var rfMap = reservationFlights.ToDictionary(rf => rf.Id.Value, rf => rf);
        var reservationMap = reservations.ToDictionary(r => r.Id.Value, r => r);
        var flightMap = flights.ToDictionary(f => f.Id.Value, f => f);
        var passengerMap = passengers.ToDictionary(p => p.Id.Value, p => p);
        var personNameMap = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");

        var result = new Dictionary<int, string>();
        foreach (var rp in reservationPassengers)
        {
            var passengerName = passengerMap.TryGetValue(rp.PassengerId.Value, out var passenger) &&
                                personNameMap.TryGetValue(passenger.PersonId.Value, out var name)
                ? name
                : $"#{rp.PassengerId.Value}";

            var pnr = "NULL";
            var flightCode = "NULL";

            if (rfMap.TryGetValue(rp.ReservationFlightId.Value, out var rf))
            {
                if (reservationMap.TryGetValue(rf.ReservationId.Value, out var reservation))
                    pnr = reservation.Code?.Value ?? "NULL";

                if (flightMap.TryGetValue(rf.FlightId.Value, out var flight))
                    flightCode = flight.Code.Value;
            }

            result[rp.Id.Value] = $"pax={passengerName} [{rp.PassengerId.Value}] - PNR={pnr} - flight={flightCode} - reserva_vuelo_id={rp.ReservationFlightId.Value}";
        }

        return result;
    }

    private static string Format(Ticket t, Dictionary<int, string> statusMap, Dictionary<int, string> rpMap)
    {
        var status = statusMap.TryGetValue(t.StatusId.Value, out var st) ? $"{st} [{t.StatusId.Value}]" : $"#{t.StatusId.Value}";
        var rp = rpMap.TryGetValue(t.ReservationPassengerId.Value, out var rpDisp) ? $"{rpDisp} [{t.ReservationPassengerId.Value}]" : $"#{t.ReservationPassengerId.Value}";
        var issuedAt = t.IssuedAt.Value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

        return $"{t.Id.Value} - code={t.Code.Value} - {status} - issuedAt={issuedAt} - rp={rp}";
    }
}


