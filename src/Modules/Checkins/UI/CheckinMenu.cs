// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\UI\CheckinMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Globalization;
using GestionAerolineas.src.Modules.CheckinStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.Checkins.Application.UseCases;
using GestionAerolineas.src.Modules.Checkins.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightSeats.Application.UseCases;
using GestionAerolineas.src.Modules.FlightSeats.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;
using GestionAerolineas.src.Modules.Staff.Application.UseCases;
using GestionAerolineas.src.Modules.Tickets.Application.UseCases;

namespace GestionAerolineas.src.Modules.Checkins.UI;

public class CheckinMenu
{
    private const int TopCount = 10;

    private readonly CreateCheckinUseCase _create;
    private readonly GetAllCheckinsUseCase _getAll;
    private readonly GetCheckinByIdUseCase _getById;
    private readonly GetCheckinByTicketIdUseCase _getByTicketId;
    private readonly GetCheckinsByPassengerIdUseCase _getByPassengerId;
    private readonly GetCheckinsByFlightIdUseCase _getByFlightId;
    private readonly GetCheckinsByStatusIdUseCase _getByStatusId;
    private readonly GetCheckinsByCheckedAtRangeUseCase _getByCheckedAtRange;
    private readonly UpdateCheckinUseCase _update;
    private readonly DeleteCheckinUseCase _delete;

    private readonly GetAllTicketsUseCase _getAllTickets;
    private readonly GetAllCheckinStatusesUseCase _getAllStatuses;
    private readonly GetAllStaffUseCase _getAllStaff;
    private readonly GetAvailableSeatsByFlightIdUseCase _getAvailableSeatsByFlightId;
    private readonly GetAllFlightSeatsUseCase _getAllFlightSeats;
    private readonly GetAllFlightsUseCase _getAllFlights;
    private readonly GetAllReservationPassengersUseCase _getAllReservationPassengers;
    private readonly GetAllReservationFlightsUseCase _getAllReservationFlights;
    private readonly GetAllPassengersUseCase _getAllPassengers;
    private readonly GetAllPeopleUseCase _getAllPeople;

    public CheckinMenu(
        CreateCheckinUseCase create,
        GetAllCheckinsUseCase getAll,
        GetCheckinByIdUseCase getById,
        GetCheckinByTicketIdUseCase getByTicketId,
        GetCheckinsByPassengerIdUseCase getByPassengerId,
        GetCheckinsByFlightIdUseCase getByFlightId,
        GetCheckinsByStatusIdUseCase getByStatusId,
        GetCheckinsByCheckedAtRangeUseCase getByCheckedAtRange,
        UpdateCheckinUseCase update,
        DeleteCheckinUseCase delete,
        GetAllTicketsUseCase getAllTickets,
        GetAllCheckinStatusesUseCase getAllStatuses,
        GetAllStaffUseCase getAllStaff,
        GetAvailableSeatsByFlightIdUseCase getAvailableSeatsByFlightId,
        GetAllFlightSeatsUseCase getAllFlightSeats,
        GetAllFlightsUseCase getAllFlights,
        GetAllReservationPassengersUseCase getAllReservationPassengers,
        GetAllReservationFlightsUseCase getAllReservationFlights,
        GetAllPassengersUseCase getAllPassengers,
        GetAllPeopleUseCase getAllPeople)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByTicketId = getByTicketId;
        _getByPassengerId = getByPassengerId;
        _getByFlightId = getByFlightId;
        _getByStatusId = getByStatusId;
        _getByCheckedAtRange = getByCheckedAtRange;
        _update = update;
        _delete = delete;
        _getAllTickets = getAllTickets;
        _getAllStatuses = getAllStatuses;
        _getAllStaff = getAllStaff;
        _getAvailableSeatsByFlightId = getAvailableSeatsByFlightId;
        _getAllFlightSeats = getAllFlightSeats;
        _getAllFlights = getAllFlights;
        _getAllReservationPassengers = getAllReservationPassengers;
        _getAllReservationFlights = getAllReservationFlights;
        _getAllPassengers = getAllPassengers;
        _getAllPeople = getAllPeople;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear check-in",
            "Listar check-ins",
            "Consultar check-in por ID",
            "Consultar check-in por tiquete_id",
            "Consultar check-ins por passenger_id",
            "Consultar check-ins por vuelo_id",
            "Consultar check-ins por estado_checkin_id",
            "Consultar check-ins por rango fecha_checkin",
            "Actualizar check-in",
            "Eliminar check-in",
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
                        var ticketMap = await GetTicketMapAsync();
                        var availableTicketIds = await GetTicketIdsWithoutCheckinAsync();
                        if (availableTicketIds.Count == 0)
                            throw new Exception("No hay tickets sin check-in. Primero crea/emite un ticket para una reserva con pasajero.");

                        await PrintTicketsAsync(ticketMap);
                        var defaultTicketId = availableTicketIds.OrderByDescending(x => x).First();

                        var staffIds = await PrintAirportStaffAsync();
                        if (staffIds.Count == 0)
                            throw new Exception("No hay staff activo con aeropuerto asignado para hacer check-in.");
                        var defaultStaffId = staffIds.First();

                        await PrintStatusesAsync();

                        Console.Write($"\nIngrese tiquete_id [ENTER={defaultTicketId}]: ");
                        int ticketId = ReadIntOrDefault(Console.ReadLine(), "tiquete_id", defaultTicketId);
                        if (!availableTicketIds.Contains(ticketId))
                            throw new Exception("El tiquete no existe o ya tiene check-in.");

                        Console.Write($"Ingrese personal_id [ENTER={defaultStaffId}]: ");
                        int staffId = ReadIntOrDefault(Console.ReadLine(), "personal_id", defaultStaffId);

                        var flightId = await TryResolveFlightIdFromTicketAsync(ticketId);
                        if (flightId is null)
                            throw new Exception("No se pudo resolver el vuelo del ticket (revisa reserva_pasajero/reserva_vuelo)");

                        var availableSeats = await PrintAvailableSeatsForFlightAsync(flightId.Value);
                        if (availableSeats.Count == 0)
                            throw new Exception("No hay asientos disponibles para el vuelo del ticket.");
                        var defaultSeat = availableSeats.First();

                        Console.Write($"Ingrese asiento_vuelo_id o codigo_asiento [ENTER={defaultSeat.Code.Value}]: ");
                        int seatId = await ResolveSeatIdForFlightAsync(flightId.Value, Console.ReadLine(), defaultSeat.Id.Value);

                        Console.Write("Ingrese fecha_checkin (yyyy-MM-dd HH:mm) [default=now]: ");
                        DateTime? checkedAt = ReadOptionalDateTime(Console.ReadLine());

                        Console.Write("Ingrese estado_checkin_id [default=Realizado]: ");
                        int statusId = await ResolveDefaultStatusIdAsync(Console.ReadLine());

                        Console.Write("Equipaje bodega? (0/1, s/n) [default=0]: ");
                        bool hasBag = ReadBool(Console.ReadLine(), defaultValue: false);

                        decimal? weight = ReadBaggageWeight(hasBag);

                        var created = await _create.ExecuteAsync(
                            ticketId,
                            staffId,
                            seatId,
                            checkedAt,
                            statusId,
                            hasBag,
                            weight);

                        Console.WriteLine($"[OK] Check-in creado: id={created.Id.Value}, boardingPass={created.BoardingPassNumber.Value}");
                        break;

                    case 1:
                        await PrintListAsync(await _getAll.ExecuteAsync());
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int id = int.Parse(Console.ReadLine()!);

                        var byId = await _getById.ExecuteAsync(id);
                        if (byId is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        await PrintListAsync(new[] { byId });
                        break;

                    case 3:
                        var map = await GetTicketMapAsync();
                        await PrintTicketsAsync(map);

                        Console.Write("\nIngrese tiquete_id: ");
                        int tId = int.Parse(Console.ReadLine()!);

                        var byTicket = await _getByTicketId.ExecuteAsync(tId);
                        if (byTicket is null)
                        {
                            Console.WriteLine("(sin registros)");
                            break;
                        }

                        await PrintListAsync(new[] { byTicket });
                        break;

                    case 4:
                        await PrintPassengersAsync();
                        Console.Write("\nIngrese passenger_id: ");
                        int passengerId = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getByPassengerId.ExecuteAsync(passengerId));
                        break;

                    case 5:
                        await PrintFlightsAsync();
                        Console.Write("\nIngrese vuelo_id: ");
                        int flightIdInput = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getByFlightId.ExecuteAsync(flightIdInput));
                        break;

                    case 6:
                        await PrintStatusesAsync();
                        Console.Write("\nIngrese estado_checkin_id: ");
                        int statusIdInput = int.Parse(Console.ReadLine()!);

                        await PrintListAsync(await _getByStatusId.ExecuteAsync(statusIdInput));
                        break;

                    case 7:
                        Console.Write("Desde (yyyy-MM-dd): ");
                        var from = DateTime.ParseExact(Console.ReadLine()!, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        Console.Write("Hasta (yyyy-MM-dd): ");
                        var to = DateTime.ParseExact(Console.ReadLine()!, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                            .AddDays(1)
                            .AddTicks(-1);


                        await PrintListAsync(await _getByCheckedAtRange.ExecuteAsync(from, to));
                        break;

                    case 8:
                        Console.WriteLine("Check-ins existentes:");
                        await PrintListAsync(await _getAll.ExecuteAsync());

                        var ticketMap2 = await GetTicketMapAsync();
                        await PrintTicketsAsync(ticketMap2);
                        var updateStaffIds = await PrintAirportStaffAsync();
                        if (updateStaffIds.Count == 0)
                            throw new Exception("No hay staff activo con aeropuerto asignado para hacer check-in.");

                        await PrintStatusesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updId = ReadInt(Console.ReadLine(), "id");
                        var current = await _getById.ExecuteAsync(updId);
                        if (current is null)
                            throw new Exception("El check-in no existe");

                        Console.Write($"Ingrese tiquete_id [ENTER={current.TicketId.Value}]: ");
                        int updTicketId = ReadIntOrDefault(Console.ReadLine(), "tiquete_id", current.TicketId.Value);

                        Console.Write($"Ingrese personal_id [ENTER={current.StaffId.Value}]: ");
                        int updStaffId = ReadIntOrDefault(Console.ReadLine(), "personal_id", current.StaffId.Value);

                        var updFlightId = await TryResolveFlightIdFromTicketAsync(updTicketId);
                        if (updFlightId is null)
                            throw new Exception("No se pudo resolver el vuelo del ticket");

                        var seatMapForUpdate = await GetSeatMapAsync();
                        if (seatMapForUpdate.TryGetValue(current.FlightSeatId.Value, out var currentSeat))
                            Console.WriteLine($"Asiento actual: {current.FlightSeatId.Value} - {currentSeat}");
                        await PrintAvailableSeatsForFlightAsync(updFlightId.Value);
                        Console.Write($"Ingrese asiento_vuelo_id o codigo_asiento [ENTER={current.FlightSeatId.Value}]: ");
                        int updSeatId = await ResolveSeatIdForFlightAsync(updFlightId.Value, Console.ReadLine(), current.FlightSeatId.Value);

                        Console.Write($"Ingrese fecha_checkin (yyyy-MM-dd HH:mm) [ENTER={current.CheckedAt.Value:yyyy-MM-dd HH:mm}]: ");
                        var updCheckedAt = ReadOptionalDateTime(Console.ReadLine()) ?? current.CheckedAt.Value;

                        Console.Write("Ingrese estado_checkin_id [default=Realizado]: ");
                        int updStatusId = await ResolveDefaultStatusIdAsync(Console.ReadLine());

                        Console.Write("Numero tarjeta embarque (ENTER para mantener): ");
                        var bp = Console.ReadLine();

                        Console.Write("Equipaje bodega? (0/1, s/n) [default=0]: ");
                        bool updHasBag = ReadBool(Console.ReadLine(), defaultValue: false);

                        decimal? updWeight = ReadBaggageWeight(updHasBag);

                        await _update.ExecuteAsync(
                            updId,
                            updTicketId,
                            updStaffId,
                            updSeatId,
                            updCheckedAt,
                            updStatusId,
                            bp,
                            updHasBag,
                            updWeight);

                        Console.WriteLine("[OK] Actualizado");
                        break;

                    case 9:
                        Console.WriteLine("Check-ins existentes:");
                        await PrintListAsync(await _getAll.ExecuteAsync());

                        Console.Write("Ingrese el ID: ");
                        int delId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(delId);
                        Console.WriteLine("[OK] Eliminado");
                        break;

                    case 10:
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.GetBaseException().Message}");
            }

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private async Task PrintListAsync(IEnumerable<Checkin> list)
    {
        var statuses = await GetStatusMapAsync();
        var ticketMap = await GetTicketMapAsync();
        var staffMap = await GetStaffMapAsync();
        var seatMap = await GetSeatMapAsync();

        var items = list.ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        foreach (var item in items)
            Console.WriteLine(Format(item, statuses, ticketMap, staffMap, seatMap));
    }

    private async Task PrintStatusesAsync()
    {
        var statuses = await _getAllStatuses.ExecuteAsync();
        Console.WriteLine("CheckinStatuses:");
        foreach (var s in statuses)
            Console.WriteLine($"{s.Id.Value} - {s.Name.Value}");
    }

    private async Task<List<int>> PrintAirportStaffAsync()
    {
        var staff = (await _getAllStaff.ExecuteAsync())
            .Where(s => s.IsActive.Value && s.AirportId.Value is not null)
            .OrderBy(s => s.Id.Value)
            .ToList();

        var peopleMap = await GetPeopleNameMapForStaffAsync(staff.Select(s => s.PersonId.Value).ToList());

        Console.WriteLine("Staff (airport, active) (top 10):");
        if (staff.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return new List<int>();
        }

        foreach (var s in staff.Take(TopCount))
        {
            var name = peopleMap.TryGetValue(s.PersonId.Value, out var n) ? n : $"#{s.PersonId.Value}";
            Console.WriteLine($"{s.Id.Value} - {name} - airport_id={s.AirportId.Value}");
        }

        Console.Write("Buscar personal (texto) [opcional]: ");
        var search = (Console.ReadLine() ?? string.Empty).Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalized = search.ToUpperInvariant();
            var matches = staff
                .Select(s =>
                {
                    var name = peopleMap.TryGetValue(s.PersonId.Value, out var n) ? n : string.Empty;
                    return new { s, key = $"{name} {s.Id.Value}".ToUpperInvariant() };
                })
                .Where(x => x.key.Contains(normalized))
                .Select(x => x.s)
                .Take(TopCount)
                .ToList();

            Console.WriteLine("\nCoincidencias (top 10):");
            if (matches.Count == 0)
                Console.WriteLine("(sin registros)");
            else
                foreach (var s in matches)
                {
                    var name = peopleMap.TryGetValue(s.PersonId.Value, out var n) ? n : $"#{s.PersonId.Value}";
                    Console.WriteLine($"{s.Id.Value} - {name} - airport_id={s.AirportId.Value}");
                }
        }

        return staff.Select(s => s.Id.Value).ToList();
    }

    private async Task PrintPassengersAsync()
    {
        var passengers = (await _getAllPassengers.ExecuteAsync()).ToList();
        var people = (await _getAllPeople.ExecuteAsync()).ToList();
        var nameByPersonId = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");

        Console.WriteLine("Passengers (top 10):");
        foreach (var p in passengers.Take(TopCount))
        {
            var name = nameByPersonId.TryGetValue(p.PersonId.Value, out var n) ? n : $"#{p.PersonId.Value}";
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
                    var name = nameByPersonId.TryGetValue(p.PersonId.Value, out var n) ? n : string.Empty;
                    return new { p, key = $"{name} {p.Id.Value}".ToUpperInvariant() };
                })
                .Where(x => x.key.Contains(normalized))
                .Select(x => x.p)
                .Take(TopCount)
                .ToList();

            Console.WriteLine("\nCoincidencias (top 10):");
            if (matches.Count == 0)
                Console.WriteLine("(sin registros)");
            else
                foreach (var p in matches)
                {
                    var name = nameByPersonId.TryGetValue(p.PersonId.Value, out var n) ? n : $"#{p.PersonId.Value}";
                    Console.WriteLine($"{p.Id.Value} - {name}");
                }
        }
    }

    private async Task PrintFlightsAsync()
    {
        var flights = (await _getAllFlights.ExecuteAsync())
            .OrderByDescending(f => f.Id.Value)
            .Take(TopCount)
            .ToList();

        Console.WriteLine("Flights (top 10):");
        foreach (var f in flights)
            Console.WriteLine($"{f.Id.Value} - {f.Code.Value} - dep={f.DepartureDateTime.Value:yyyy-MM-dd HH:mm}");
    }

    private async Task<List<FlightSeat>> PrintAvailableSeatsForFlightAsync(int flightId)
    {
        var seats = (await _getAvailableSeatsByFlightId.ExecuteAsync(flightId))
            .OrderBy(s => s.Code.Value)
            .ToList();

        Console.WriteLine($"FlightSeats disponibles (top 10) para vuelo_id={flightId}:");
        if (seats.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return new List<FlightSeat>();
        }

        foreach (var s in seats.Take(TopCount))
            Console.WriteLine($"{s.Id.Value} - {s.Code.Value} - occupied={s.IsOccupied.Value}");

        return seats;
    }

    private async Task PrintTicketsAsync(Dictionary<int, string> ticketMap)
    {
        var checkins = (await _getAll.ExecuteAsync()).ToList();
        var usedTicketIds = checkins.Select(c => c.TicketId.Value).ToHashSet();

        var items = ticketMap
            .Where(kv => !usedTicketIds.Contains(kv.Key))
            .OrderByDescending(kv => kv.Key)
            .Take(TopCount)
            .ToList();

        Console.WriteLine("Tickets sin check-in (top 10):");
        if (items.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        foreach (var kv in items)
            Console.WriteLine($"{kv.Key} - {kv.Value}");

        Console.Write("Buscar ticket (texto) [opcional]: ");
        var search = (Console.ReadLine() ?? string.Empty).Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalized = search.ToUpperInvariant();
            var matches = ticketMap
                .Where(kv => !usedTicketIds.Contains(kv.Key))
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

    private async Task<List<int>> GetTicketIdsWithoutCheckinAsync()
    {
        var tickets = await _getAllTickets.ExecuteAsync();
        var checkins = await _getAll.ExecuteAsync();
        var usedTicketIds = checkins.Select(c => c.TicketId.Value).ToHashSet();

        return tickets
            .Where(t => !usedTicketIds.Contains(t.Id.Value))
            .OrderByDescending(t => t.Id.Value)
            .Select(t => t.Id.Value)
            .ToList();
    }

    private async Task<int> ResolveSeatIdForFlightAsync(int flightId, string? input, int? defaultSeatId = null)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            if (defaultSeatId.HasValue)
                return defaultSeatId.Value;

            throw new Exception("Debes ingresar asiento_vuelo_id o codigo_asiento");
        }

        var trimmed = input.Trim();
        if (int.TryParse(trimmed, out var seatId))
        {
            if (defaultSeatId == seatId)
                return seatId;

            var availableSeats = await _getAvailableSeatsByFlightId.ExecuteAsync(flightId);
            if (availableSeats.Any(s => s.Id.Value == seatId))
                return seatId;

            throw new Exception($"No hay asiento disponible con id '{seatId}' para el vuelo {flightId}");
        }

        var normalized = trimmed.ToUpperInvariant();
        var seats = await _getAvailableSeatsByFlightId.ExecuteAsync(flightId);
        var seat = seats.FirstOrDefault(s => s.Code.Value.Trim().ToUpperInvariant() == normalized);
        if (seat is null)
        {
            if (defaultSeatId.HasValue)
            {
                var currentSeat = (await _getAllFlightSeats.ExecuteAsync()).FirstOrDefault(s =>
                    s.Id.Value == defaultSeatId.Value &&
                    s.FlightId.Value == flightId &&
                    s.Code.Value.Trim().Equals(trimmed, StringComparison.OrdinalIgnoreCase));

                if (currentSeat is not null)
                    return currentSeat.Id.Value;
            }

            throw new Exception($"No hay asiento disponible con codigo '{trimmed}' para el vuelo {flightId}");
        }

        return seat.Id.Value;
    }

    private async Task<int> ResolveDefaultStatusIdAsync(string? input)
    {
        if (!string.IsNullOrWhiteSpace(input))
            return ReadInt(input, "estado_checkin_id");

        var statuses = (await _getAllStatuses.ExecuteAsync()).ToList();
        var realized = statuses.FirstOrDefault(s =>
            s.Name.Value.Trim().Equals("Realizado", StringComparison.OrdinalIgnoreCase));

        if (realized is not null)
            return realized.Id.Value;

        var first = statuses.OrderBy(s => s.Id.Value).FirstOrDefault();
        if (first is null)
            throw new Exception("No hay estados de check-in configurados.");

        return first.Id.Value;
    }

    private async Task<Dictionary<int, string>> GetStatusMapAsync()
    {
        var statuses = await _getAllStatuses.ExecuteAsync();
        return statuses.ToDictionary(s => s.Id.Value, s => s.Name.Value);
    }

    private async Task<int?> TryResolveFlightIdFromTicketAsync(int ticketId)
    {
        var tickets = (await _getAllTickets.ExecuteAsync()).ToList();
        var reservationPassengers = (await _getAllReservationPassengers.ExecuteAsync()).ToList();
        var reservationFlights = (await _getAllReservationFlights.ExecuteAsync()).ToList();

        var ticket = tickets.FirstOrDefault(t => t.Id.Value == ticketId);
        if (ticket is null)
            return null;

        var rp = reservationPassengers.FirstOrDefault(rp => rp.Id.Value == ticket.ReservationPassengerId.Value);
        if (rp is null)
            return null;

        var rf = reservationFlights.FirstOrDefault(rf => rf.Id.Value == rp.ReservationFlightId.Value);
        return rf?.FlightId.Value;
    }

    private async Task<Dictionary<int, string>> GetTicketMapAsync()
    {
        var tickets = (await _getAllTickets.ExecuteAsync()).ToList();
        var reservationPassengers = (await _getAllReservationPassengers.ExecuteAsync()).ToList();
        var reservationFlights = (await _getAllReservationFlights.ExecuteAsync()).ToList();
        var flights = (await _getAllFlights.ExecuteAsync()).ToList();
        var passengers = (await _getAllPassengers.ExecuteAsync()).ToList();
        var people = (await _getAllPeople.ExecuteAsync()).ToList();

        var rpById = reservationPassengers.ToDictionary(rp => rp.Id.Value, rp => rp);
        var rfById = reservationFlights.ToDictionary(rf => rf.Id.Value, rf => rf);
        var flightById = flights.ToDictionary(f => f.Id.Value, f => f);
        var passengerById = passengers.ToDictionary(p => p.Id.Value, p => p);
        var personNameById = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");

        var map = new Dictionary<int, string>();
        foreach (var t in tickets)
        {
            var pax = "NULL";
            var flightCode = "NULL";

            if (rpById.TryGetValue(t.ReservationPassengerId.Value, out var rp) &&
                passengerById.TryGetValue(rp.PassengerId.Value, out var passenger) &&
                personNameById.TryGetValue(passenger.PersonId.Value, out var name))
                pax = $"{name} [{rp.PassengerId.Value}]";

            if (rpById.TryGetValue(t.ReservationPassengerId.Value, out var rp2) &&
                rfById.TryGetValue(rp2.ReservationFlightId.Value, out var rf) &&
                flightById.TryGetValue(rf.FlightId.Value, out var f))
                flightCode = $"{f.Code.Value} [{rf.FlightId.Value}]";

            map[t.Id.Value] = $"code={t.Code.Value} - pax={pax} - flight={flightCode} - reserva_pasajero_id={t.ReservationPassengerId.Value}";
        }

        return map;
    }

    private async Task<Dictionary<int, string>> GetStaffMapAsync()
    {
        var staff = (await _getAllStaff.ExecuteAsync()).ToList();
        var people = (await _getAllPeople.ExecuteAsync()).ToList();
        var personNameById = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");

        var result = new Dictionary<int, string>();
        foreach (var s in staff)
        {
            var name = personNameById.TryGetValue(s.PersonId.Value, out var n) ? n : $"#{s.PersonId.Value}";
            result[s.Id.Value] = $"{name} - active={(s.IsActive.Value ? 1 : 0)} - airport_id={s.AirportId.Value?.ToString() ?? "NULL"}";
        }

        return result;
    }

    private async Task<Dictionary<int, string>> GetSeatMapAsync()
    {
        var seats = (await _getAllFlightSeats.ExecuteAsync()).ToList();
        var flights = (await _getAllFlights.ExecuteAsync()).ToList();

        var flightCodeById = flights.ToDictionary(f => f.Id.Value, f => f.Code.Value);

        return seats.ToDictionary(
            s => s.Id.Value,
            s =>
            {
                var flightCode = flightCodeById.TryGetValue(s.FlightId.Value, out var c) ? c : $"#{s.FlightId.Value}";
                return $"{flightCode} - {s.Code.Value} - occupied={(s.IsOccupied.Value ? 1 : 0)}";
            });
    }

    private async Task<Dictionary<int, string>> GetPeopleNameMapForStaffAsync(List<int> personIds)
    {
        var people = (await _getAllPeople.ExecuteAsync())
            .Where(p => personIds.Contains(p.Id.Value))
            .ToList();

        return people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
    }

    private static string Format(
        Checkin c,
        Dictionary<int, string> statusMap,
        Dictionary<int, string> ticketMap,
        Dictionary<int, string> staffMap,
        Dictionary<int, string> seatMap)
    {
        var status = statusMap.TryGetValue(c.StatusId.Value, out var st) ? $"{st} [{c.StatusId.Value}]" : $"#{c.StatusId.Value}";
        var ticket = ticketMap.TryGetValue(c.TicketId.Value, out var tk) ? $"{tk} [{c.TicketId.Value}]" : $"#{c.TicketId.Value}";
        var staff = staffMap.TryGetValue(c.StaffId.Value, out var sf) ? $"{sf} [{c.StaffId.Value}]" : $"#{c.StaffId.Value}";
        var seat = seatMap.TryGetValue(c.FlightSeatId.Value, out var s) ? $"{s} [{c.FlightSeatId.Value}]" : $"#{c.FlightSeatId.Value}";
        var dt = c.CheckedAt.Value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

        return $"{c.Id.Value} - ticket={ticket} - staff={staff} - seat={seat} - checkedAt={dt} - status={status} - boardingPass={c.BoardingPassNumber.Value} - bag={(c.HasHoldBaggage.Value ? 1 : 0)} - kg={c.BaggageWeightKg.Value:0.00}";
    }

    private static int ReadInt(string? input, string fieldName)
    {
        if (!int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            throw new Exception($"{fieldName} debe ser un numero entero valido");

        return value;
    }

    private static int ReadIntOrDefault(string? input, string fieldName, int defaultValue)
    {
        if (string.IsNullOrWhiteSpace(input))
            return defaultValue;

        return ReadInt(input, fieldName);
    }

    private static DateTime? ReadOptionalDateTime(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        return ReadRequiredDateTime(input, "fecha_checkin");
    }

    private static DateTime ReadRequiredDateTime(string? input, string fieldName)
    {
        var formats = new[] { "yyyy-MM-dd HH:mm", "yyyy-MM-ddTHH:mm", "yyyy-MM-dd" };
        if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var value))
            return value;

        if (DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
            return value;

        throw new Exception($"{fieldName} debe tener formato yyyy-MM-dd HH:mm");
    }

    private static bool ReadBool(string? input, bool defaultValue)
    {
        if (string.IsNullOrWhiteSpace(input))
            return defaultValue;

        var normalized = input.Trim().ToUpperInvariant();
        return normalized switch
        {
            "1" or "S" or "SI" or "Y" or "YES" or "TRUE" => true,
            "0" or "N" or "NO" or "FALSE" => false,
            _ => throw new Exception("El valor booleano debe ser 1/0, s/n o true/false")
        };
    }

    private static decimal? ReadBaggageWeight(bool hasBag)
    {
        if (!hasBag)
            return 0m;

        Console.Write("Peso equipaje (kg): ");
        if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out var weight))
            throw new Exception("peso_equipaje_kg debe ser un numero valido");

        return weight;
    }
}

