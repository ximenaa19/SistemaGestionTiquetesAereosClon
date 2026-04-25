// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\UI\ReservationFlightMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;

namespace GestionAerolineas.src.Modules.ReservationFlights.UI;

public class ReservationFlightMenu
{
    private const int TopCount = 10;

    private readonly CreateReservationFlightUseCase _create;
    private readonly GetAllReservationFlightsUseCase _getAll;
    private readonly GetReservationFlightByIdUseCase _getById;
    private readonly GetReservationFlightsByReservationIdUseCase _getByReservationId;
    private readonly GetReservationFlightsByFlightIdUseCase _getByFlightId;
    private readonly GetReservationFlightByReservationAndFlightUseCase _getByPair;
    private readonly GetReservationFlightsByReservationCodeUseCase _getByReservationCode;
    private readonly UpdateReservationFlightUseCase _update;
    private readonly DeleteReservationFlightUseCase _delete;

    private readonly GetAllReservationsUseCase _getAllReservations;
    private readonly GetAllCustomersUseCase _getAllCustomers;
    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllReservationStatusesUseCase _getAllReservationStatuses;

    private readonly GetAllFlightsUseCase _getAllFlights;
    private readonly GetAllAirlinesUseCase _getAllAirlines;
    private readonly GetAllRoutesUseCase _getAllRoutes;
    private readonly GetAllAirportsUseCase _getAllAirports;

    public ReservationFlightMenu(
        CreateReservationFlightUseCase create,
        GetAllReservationFlightsUseCase getAll,
        GetReservationFlightByIdUseCase getById,
        GetReservationFlightsByReservationIdUseCase getByReservationId,
        GetReservationFlightsByFlightIdUseCase getByFlightId,
        GetReservationFlightByReservationAndFlightUseCase getByPair,
        GetReservationFlightsByReservationCodeUseCase getByReservationCode,
        UpdateReservationFlightUseCase update,
        DeleteReservationFlightUseCase delete,
        GetAllReservationsUseCase getAllReservations,
        GetAllCustomersUseCase getAllCustomers,
        GetAllPeopleUseCase getAllPeople,
        GetAllReservationStatusesUseCase getAllReservationStatuses,
        GetAllFlightsUseCase getAllFlights,
        GetAllAirlinesUseCase getAllAirlines,
        GetAllRoutesUseCase getAllRoutes,
        GetAllAirportsUseCase getAllAirports)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByReservationId = getByReservationId;
        _getByFlightId = getByFlightId;
        _getByPair = getByPair;
        _getByReservationCode = getByReservationCode;
        _update = update;
        _delete = delete;
        _getAllReservations = getAllReservations;
        _getAllCustomers = getAllCustomers;
        _getAllPeople = getAllPeople;
        _getAllReservationStatuses = getAllReservationStatuses;
        _getAllFlights = getAllFlights;
        _getAllAirlines = getAllAirlines;
        _getAllRoutes = getAllRoutes;
        _getAllAirports = getAllAirports;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear reservation flight",
            "Listar reservation flights",
            "Get reservation flight by ID",
            "Get reservation flights by reservation_id",
            "Get reservation flights by flight_id",
            "Get reservation flight by reservation_id + flight_id",
            "Get reservation flights by reservation code (PNR)",
            "Actualizar reservation flight",
            "Eliminar reservation flight",
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
                        await PrintReservationsAsync();
                        await PrintFlightsAsync();

                        Console.Write("\nIngrese reserva_id: ");
                        int reservationId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese vuelo_id: ");
                        int flightId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese valor_parcial: ");
                        decimal partial = decimal.Parse(Console.ReadLine()!);

                        await _create.ExecuteAsync(reservationId, flightId, partial);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        await PrintManyAsync(await _getAll.ExecuteAsync());
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int id = int.Parse(Console.ReadLine()!);
                        await PrintOneAsync(await _getById.ExecuteAsync(id));
                        break;

                    case 3:
                        await PrintReservationsAsync();
                        Console.Write("\nIngrese reserva_id: ");
                        int byReservationId = int.Parse(Console.ReadLine()!);
                        await PrintManyAsync(await _getByReservationId.ExecuteAsync(byReservationId));
                        break;

                    case 4:
                        await PrintFlightsAsync();
                        Console.Write("\nIngrese vuelo_id: ");
                        int byFlightId = int.Parse(Console.ReadLine()!);
                        await PrintManyAsync(await _getByFlightId.ExecuteAsync(byFlightId));
                        break;

                    case 5:
                        await PrintReservationsAsync();
                        await PrintFlightsAsync();

                        Console.Write("\nIngrese reserva_id: ");
                        int pairReservationId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese vuelo_id: ");
                        int pairFlightId = int.Parse(Console.ReadLine()!);

                        await PrintOneAsync(await _getByPair.ExecuteAsync(pairReservationId, pairFlightId));
                        break;

                    case 6:
                        Console.Write("Ingrese el codigo_reserva (PNR): ");
                        var code = Console.ReadLine()!;
                        await PrintManyAsync(await _getByReservationCode.ExecuteAsync(code));
                        break;

                    case 7:
                        await PrintReservationsAsync();
                        await PrintFlightsAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese reserva_id: ");
                        int newReservationId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese vuelo_id: ");
                        int newFlightId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese valor_parcial: ");
                        decimal newPartial = decimal.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newReservationId, newFlightId, newPartial);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 8:
                        Console.Write("Ingrese el ID: ");
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

    private async Task PrintReservationsAsync()
    {
        var reservations = (await _getAllReservations.ExecuteAsync()).ToList();
        var reservationMap = await GetReservationDisplayMapAsync();

        Console.WriteLine("Reservations (top 10):");
        PrintTopWithFormat(reservations, r => $"{r.Id.Value} - {GetDisplay(reservationMap, r.Id.Value)}");

        Console.Write("Buscar reserva (texto) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = reservations
            .Where(r => GetDisplay(reservationMap, r.Id.Value).ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, r => $"{r.Id.Value} - {GetDisplay(reservationMap, r.Id.Value)}");
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

    private async Task PrintOneAsync(ReservationFlight? item)
    {
        if (item is null)
        {
            Console.WriteLine("No encontrado");
            return;
        }

        var reservationMap = await GetReservationDisplayMapAsync();
        var flightMap = await GetFlightDisplayMapAsync();
        Console.WriteLine(Format(item, reservationMap, flightMap));
    }

    private async Task PrintManyAsync(IEnumerable<ReservationFlight> items)
    {
        var reservationMap = await GetReservationDisplayMapAsync();
        var flightMap = await GetFlightDisplayMapAsync();

        foreach (var item in items)
            Console.WriteLine(Format(item, reservationMap, flightMap));
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

    private async Task<Dictionary<int, string>> GetReservationDisplayMapAsync()
    {
        var reservations = await _getAllReservations.ExecuteAsync();
        var customers = await _getAllCustomers.ExecuteAsync();
        var people = await _getAllPeople.ExecuteAsync();
        var statuses = await _getAllReservationStatuses.ExecuteAsync();

        var personMap = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
        var customerMap = customers.ToDictionary(c => c.Id.Value, c => GetDisplay(personMap, c.PersonId.Value));
        var statusMap = statuses.ToDictionary(s => s.Id.Value, s => s.Name.Value);

        return reservations.ToDictionary(
            r => r.Id.Value,
            r =>
            {
                var code = r.Code?.Value ?? "NULL";
                var customer = GetDisplay(customerMap, r.CustomerId.Value);
                var status = GetDisplay(statusMap, r.StatusId.Value);
                return $"{code} - {customer} - status={status} - total={r.TotalAmount.Value:0.00}";
            });
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

    private static string Format(
        ReservationFlight item,
        Dictionary<int, string> reservationMap,
        Dictionary<int, string> flightMap)
    {
        var reservationDisplay = GetDisplay(reservationMap, item.ReservationId.Value);
        var flightDisplay = GetDisplay(flightMap, item.FlightId.Value);

        return $"{item.Id.Value} - reservation={reservationDisplay} [{item.ReservationId.Value}] - flight={flightDisplay} [{item.FlightId.Value}] - partial={item.PartialAmount.Value:0.00}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? display : $"#{id}";
    }
}


