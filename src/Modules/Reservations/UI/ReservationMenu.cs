// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\UI\ReservationMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using UpdateReservationStatusUseCase = GestionAerolineas.src.Modules.Reservations.Application.UseCases.UpdateReservationStatusUseCase;

namespace GestionAerolineas.src.Modules.Reservations.UI;

public class ReservationMenu
{
    private const int TopCount = 10;

    private readonly CreateReservationUseCase _createReservation;
    private readonly GetAllReservationsUseCase _getAll;
    private readonly GetReservationByIdUseCase _getById;
    private readonly GetReservationByCodeUseCase _getByCode;
    private readonly GetReservationsByCustomerIdUseCase _getByCustomerId;
    private readonly GetReservationsByStatusIdUseCase _getByStatusId;
    private readonly GetReservationsByDateRangeUseCase _getByDateRange;
    private readonly GetReservationDetailsByIdUseCase _getDetailsById;
    private readonly UpdateReservationStatusUseCase _updateStatus;
    private readonly DeleteReservationUseCase _delete;

    private readonly CreateReservationFlightUseCase _createReservationFlight;
    private readonly CreateReservationPassengerUseCase _createReservationPassenger;

    private readonly GetAllCustomersUseCase _getAllCustomers;
    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllReservationStatusesUseCase _getAllReservationStatuses;
    private readonly GetAllFlightsUseCase _getAllFlights;
    private readonly GetAllAirlinesUseCase _getAllAirlines;
    private readonly GetAllRoutesUseCase _getAllRoutes;
    private readonly GetAllAirportsUseCase _getAllAirports;
    private readonly GetAllPassengersUseCase _getAllPassengers;

    public ReservationMenu(
        CreateReservationUseCase createReservation,
        GetAllReservationsUseCase getAll,
        GetReservationByIdUseCase getById,
        GetReservationByCodeUseCase getByCode,
        GetReservationsByCustomerIdUseCase getByCustomerId,
        GetReservationsByStatusIdUseCase getByStatusId,
        GetReservationsByDateRangeUseCase getByDateRange,
        GetReservationDetailsByIdUseCase getDetailsById,
        UpdateReservationStatusUseCase updateStatus,
        DeleteReservationUseCase delete,
        CreateReservationFlightUseCase createReservationFlight,
        CreateReservationPassengerUseCase createReservationPassenger,
        GetAllCustomersUseCase getAllCustomers,
        GetAllPeopleUseCase getAllPeople,
        GetAllReservationStatusesUseCase getAllReservationStatuses,
        GetAllFlightsUseCase getAllFlights,
        GetAllAirlinesUseCase getAllAirlines,
        GetAllRoutesUseCase getAllRoutes,
        GetAllAirportsUseCase getAllAirports,
        GetAllPassengersUseCase getAllPassengers)
    {
        _createReservation = createReservation;
        _getAll = getAll;
        _getById = getById;
        _getByCode = getByCode;
        _getByCustomerId = getByCustomerId;
        _getByStatusId = getByStatusId;
        _getByDateRange = getByDateRange;
        _getDetailsById = getDetailsById;
        _updateStatus = updateStatus;
        _delete = delete;
        _createReservationFlight = createReservationFlight;
        _createReservationPassenger = createReservationPassenger;
        _getAllCustomers = getAllCustomers;
        _getAllPeople = getAllPeople;
        _getAllReservationStatuses = getAllReservationStatuses;
        _getAllFlights = getAllFlights;
        _getAllAirlines = getAllAirlines;
        _getAllRoutes = getAllRoutes;
        _getAllAirports = getAllAirports;
        _getAllPassengers = getAllPassengers;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear reserva (asistente)",
            "Listar reservations",
            "Get reservation by ID",
            "Get reservation by code (PNR)",
            "Get reservations by customer_id",
            "Get reservations by status_id",
            "Get reservations by date range",
            "Get reservation details by ID",
            "Actualizar estado de reserva",
            "Eliminar reserva (hard)",
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
                        await CreateWizardAsync();
                        break;

                    case 1:
                        await ListAllAsync();
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int id = int.Parse(Console.ReadLine()!);
                        var byId = await _getById.ExecuteAsync(id);
                        await PrintOneAsync(byId);
                        break;

                    case 3:
                        Console.Write("Ingrese el codigo_reserva (PNR): ");
                        var code = Console.ReadLine()!;
                        var byCode = await _getByCode.ExecuteAsync(code);
                        await PrintOneAsync(byCode);
                        break;

                    case 4:
                        await PrintCustomersAsync();
                        Console.Write("\nIngrese customer_id (clients.id): ");
                        int customerId = int.Parse(Console.ReadLine()!);
                        await PrintManyAsync(await _getByCustomerId.ExecuteAsync(customerId));
                        break;

                    case 5:
                        await PrintReservationStatusesAsync();
                        Console.Write("\nIngrese status_id: ");
                        int statusId = int.Parse(Console.ReadLine()!);
                        await PrintManyAsync(await _getByStatusId.ExecuteAsync(statusId));
                        break;

                    case 6:
                        Console.Write("Desde (yyyy-MM-dd): ");
                        var from = DateTime.Parse(Console.ReadLine()!);
                        Console.Write("Hasta (yyyy-MM-dd): ");
                        var to = DateTime.Parse(Console.ReadLine()!);
                        await PrintManyAsync(await _getByDateRange.ExecuteAsync(from.Date, to.Date.AddDays(1).AddTicks(-1)));
                        break;

                    case 7:
                        Console.Write("Ingrese el ID: ");
                        int detailsId = int.Parse(Console.ReadLine()!);
                        await PrintDetailsAsync(await _getDetailsById.ExecuteAsync(detailsId));
                        break;

                    case 8:
                        Console.Write("Ingrese reservation_id: ");
                        int reservationId = int.Parse(Console.ReadLine()!);
                        await PrintReservationStatusesAsync();
                        Console.Write("\nIngrese new_status_id: ");
                        int newStatusId = int.Parse(Console.ReadLine()!);
                        await _updateStatus.ExecuteAsync(reservationId, newStatusId);
                        Console.WriteLine("[OK] Actualizado");
                        break;

                    case 9:
                        Console.Write("Ingrese reservation_id: ");
                        int deleteId = int.Parse(Console.ReadLine()!);
                        var deleted = await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine(deleted ? "[OK] Eliminado" : "No encontrado");
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

    private async Task CreateWizardAsync()
    {
        await PrintCustomersAsync();
        await PrintReservationStatusesAsync();

        Console.Write("\nIngrese customer_id (clients.id): ");
        int customerId = int.Parse(Console.ReadLine()!);

        Console.Write("Ingrese status_id [default=PENDIENTE si lo dejas vacio]: ");
        var statusInput = Console.ReadLine();
        int statusId = await ResolveDefaultStatusIdAsync(statusInput);

        Console.Write("Hold minutes [default=15, 0=NULL]: ");
        var minutesInput = Console.ReadLine();
        int minutes = string.IsNullOrWhiteSpace(minutesInput) ? 15 : int.Parse(minutesInput);
        DateTime? expiresAt = minutes <= 0 ? null : DateTime.Now.AddMinutes(minutes);

        var reservation = await _createReservation.ExecuteAsync(customerId, statusId, expiresAt);

        Console.WriteLine($"\n[OK] Reserva creada: id={reservation.Id.Value}, code={(reservation.Code?.Value ?? "NULL")}");

        Console.WriteLine("\nAgrega vuelos a la reserva. Deja vuelo_id vacio para terminar.");
        while (true)
        {
            await PrintFlightsAsync();
            Console.Write("\nIngrese vuelo_id: ");
            var flightInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(flightInput))
                break;

            int flightId = int.Parse(flightInput);

            Console.Write("Ingrese valor_parcial: ");
            decimal partial = decimal.Parse(Console.ReadLine()!);

            var reservationFlight = await _createReservationFlight.ExecuteAsync(reservation.Id.Value, flightId, partial);
            Console.WriteLine($"[OK] Agregado vuelo a reserva: reserva_vuelo_id={reservationFlight.Id.Value}");

            Console.WriteLine("\nAgrega pasajeros para este vuelo. Deja pasajero_id vacio para terminar.");
            while (true)
            {
                await PrintPassengersAsync();
                Console.Write("\nIngrese pasajero_id (passengers.id): ");
                var passengerInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(passengerInput))
                    break;

                int passengerId = int.Parse(passengerInput);
                await _createReservationPassenger.ExecuteAsync(reservationFlight.Id.Value, passengerId);
                Console.WriteLine("[OK] Pasajero agregado (se decrementa asientos_disponibles)");
            }
        }

        Console.WriteLine("\n[OK] Wizard finalizado. Puedes ver el detalle con 'Get reservation details by ID'.");
    }

    private async Task<int> ResolveDefaultStatusIdAsync(string? statusInput)
    {
        if (!string.IsNullOrWhiteSpace(statusInput))
            return int.Parse(statusInput);

        var statuses = (await _getAllReservationStatuses.ExecuteAsync()).ToList();
        var pending = statuses.FirstOrDefault(s => s.Name.Value.Trim().ToUpperInvariant().Contains("PEND"));
        if (pending is null)
            throw new Exception("No se encontro un estado PENDIENTE. Crea reservationstatuses primero.");

        return pending.Id.Value;
    }

    private async Task ListAllAsync()
    {
        var customerMap = await GetCustomerDisplayMapAsync();
        var statusMap = await GetStatusDisplayMapAsync();
        var list = await _getAll.ExecuteAsync();

        foreach (var item in list)
            Console.WriteLine(Format(item, customerMap, statusMap));
    }

    private async Task PrintOneAsync(Reservation? reservation)
    {
        if (reservation is null)
        {
            Console.WriteLine("No encontrado");
            return;
        }

        var customerMap = await GetCustomerDisplayMapAsync();
        var statusMap = await GetStatusDisplayMapAsync();
        Console.WriteLine(Format(reservation, customerMap, statusMap));
    }

    private async Task PrintManyAsync(IEnumerable<Reservation> reservations)
    {
        var customerMap = await GetCustomerDisplayMapAsync();
        var statusMap = await GetStatusDisplayMapAsync();

        foreach (var item in reservations)
            Console.WriteLine(Format(item, customerMap, statusMap));
    }

    private async Task PrintDetailsAsync(ReservationDetails? details)
    {
        if (details is null)
        {
            Console.WriteLine("No encontrado");
            return;
        }

        var customerMap = await GetCustomerDisplayMapAsync();
        var statusMap = await GetStatusDisplayMapAsync();
        var flightMap = await GetFlightDisplayMapAsync();
        var passengerMap = await GetPassengerDisplayMapAsync();

        Console.WriteLine("=== RESERVATION ===");
        Console.WriteLine(Format(details.Reservation, customerMap, statusMap));

        Console.WriteLine("\n=== FLIGHTS ===");
        foreach (var rf in details.ReservationFlights)
        {
            var flightDisplay = GetDisplay(flightMap, rf.FlightId.Value);
            Console.WriteLine($"{rf.Id.Value} - flight={flightDisplay} [{rf.FlightId.Value}] - partial={rf.PartialAmount.Value:0.00}");

            var pax = details.ReservationPassengers
                .Where(p => p.ReservationFlightId.Value == rf.Id.Value)
                .ToList();

            foreach (var p in pax)
            {
                var paxDisplay = GetDisplay(passengerMap, p.PassengerId.Value);
                Console.WriteLine($"  - pax={paxDisplay} [{p.PassengerId.Value}] (reserva_pasajero_id={p.Id.Value})");
            }
        }
    }

    private async Task PrintCustomersAsync()
    {
        var customers = (await _getAllCustomers.ExecuteAsync()).ToList();
        var people = await _getAllPeople.ExecuteAsync();
        var personMap = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");

        Console.WriteLine("Customers (top 10):");
        PrintTopWithFormat(customers, c => $"{c.Id.Value} - {GetDisplay(personMap, c.PersonId.Value)}");

        Console.Write("Buscar customer (texto) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = customers
            .Where(c => GetDisplay(personMap, c.PersonId.Value).ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, c => $"{c.Id.Value} - {GetDisplay(personMap, c.PersonId.Value)}");
    }

    private async Task PrintReservationStatusesAsync()
    {
        var statuses = (await _getAllReservationStatuses.ExecuteAsync()).ToList();
        Console.WriteLine("\nReservationStatuses:");
        PrintTopWithFormat(statuses, s => $"{s.Id.Value} - {s.Name.Value}");
    }

    private async Task PrintFlightsAsync()
    {
        var flights = (await _getAllFlights.ExecuteAsync()).ToList();
        var flightMap = await GetFlightDisplayMapAsync();

        Console.WriteLine("\nFlights (top 10):");
        PrintTopWithFormat(flights, f => $"{f.Id.Value} - {GetDisplay(flightMap, f.Id.Value)}");

        Console.Write("Buscar vuelo (texto) [opcional]: ");
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

    private async Task PrintPassengersAsync()
    {
        var passengers = (await _getAllPassengers.ExecuteAsync()).ToList();
        var passengerMap = await GetPassengerDisplayMapAsync();

        Console.WriteLine("\nPassengers (top 10):");
        PrintTopWithFormat(passengers, p => $"{p.Id.Value} - {GetDisplay(passengerMap, p.Id.Value)}");

        Console.Write("Buscar pasajero (texto) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = passengers
            .Where(p => GetDisplay(passengerMap, p.Id.Value).ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, p => $"{p.Id.Value} - {GetDisplay(passengerMap, p.Id.Value)}");
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

    private async Task<Dictionary<int, string>> GetCustomerDisplayMapAsync()
    {
        var customers = await _getAllCustomers.ExecuteAsync();
        var people = await _getAllPeople.ExecuteAsync();
        var personMap = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");

        return customers.ToDictionary(c => c.Id.Value, c => GetDisplay(personMap, c.PersonId.Value));
    }

    private async Task<Dictionary<int, string>> GetStatusDisplayMapAsync()
    {
        var statuses = await _getAllReservationStatuses.ExecuteAsync();
        return statuses.ToDictionary(s => s.Id.Value, s => s.Name.Value);
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

    private async Task<Dictionary<int, string>> GetPassengerDisplayMapAsync()
    {
        var passengers = await _getAllPassengers.ExecuteAsync();
        var people = await _getAllPeople.ExecuteAsync();
        var personMap = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");

        return passengers.ToDictionary(
            p => p.Id.Value,
            p => GetDisplay(personMap, p.PersonId.Value));
    }

    private static string Format(Reservation item, Dictionary<int, string> customerMap, Dictionary<int, string> statusMap)
    {
        var customer = GetDisplay(customerMap, item.CustomerId.Value);
        var status = GetDisplay(statusMap, item.StatusId.Value);
        var code = item.Code?.Value ?? "NULL";
        var expires = item.ExpiresAt.Value?.ToString("yyyy-MM-dd HH:mm") ?? "NULL";

        return $"{item.Id.Value} - code={code} - customer={customer} [{item.CustomerId.Value}] - status={status} [{item.StatusId.Value}] - total={item.TotalAmount.Value:0.00} - reservedAt={item.ReservedAt.Value:yyyy-MM-dd HH:mm} - expiresAt={expires}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? display : $"#{id}";
    }
}

