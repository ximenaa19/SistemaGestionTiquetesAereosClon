// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\UI\ReservationPassengerMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;

namespace GestionAerolineas.src.Modules.ReservationPassengers.UI;

public class ReservationPassengerMenu
{
    private const int TopCount = 10;

    private readonly CreateReservationPassengerUseCase _create;
    private readonly GetAllReservationPassengersUseCase _getAll;
    private readonly GetReservationPassengerByIdUseCase _getById;
    private readonly GetReservationPassengersByReservationFlightIdUseCase _getByReservationFlightId;
    private readonly GetReservationPassengersByPassengerIdUseCase _getByPassengerId;
    private readonly GetReservationPassengerByReservationFlightAndPassengerUseCase _getByPair;
    private readonly GetReservationPassengersByReservationCodeUseCase _getByReservationCode;
    private readonly UpdateReservationPassengerUseCase _update;
    private readonly DeleteReservationPassengerUseCase _delete;

    private readonly GetAllReservationFlightsUseCase _getAllReservationFlights;
    private readonly GetAllReservationsUseCase _getAllReservations;
    private readonly GetAllCustomersUseCase _getAllCustomers;
    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllReservationStatusesUseCase _getAllReservationStatuses;

    private readonly GetAllFlightsUseCase _getAllFlights;
    private readonly GetAllAirlinesUseCase _getAllAirlines;
    private readonly GetAllRoutesUseCase _getAllRoutes;
    private readonly GetAllAirportsUseCase _getAllAirports;
    private readonly GetAllPassengersUseCase _getAllPassengers;

    public ReservationPassengerMenu(
        CreateReservationPassengerUseCase create,
        GetAllReservationPassengersUseCase getAll,
        GetReservationPassengerByIdUseCase getById,
        GetReservationPassengersByReservationFlightIdUseCase getByReservationFlightId,
        GetReservationPassengersByPassengerIdUseCase getByPassengerId,
        GetReservationPassengerByReservationFlightAndPassengerUseCase getByPair,
        GetReservationPassengersByReservationCodeUseCase getByReservationCode,
        UpdateReservationPassengerUseCase update,
        DeleteReservationPassengerUseCase delete,
        GetAllReservationFlightsUseCase getAllReservationFlights,
        GetAllReservationsUseCase getAllReservations,
        GetAllCustomersUseCase getAllCustomers,
        GetAllPeopleUseCase getAllPeople,
        GetAllReservationStatusesUseCase getAllReservationStatuses,
        GetAllFlightsUseCase getAllFlights,
        GetAllAirlinesUseCase getAllAirlines,
        GetAllRoutesUseCase getAllRoutes,
        GetAllAirportsUseCase getAllAirports,
        GetAllPassengersUseCase getAllPassengers)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByReservationFlightId = getByReservationFlightId;
        _getByPassengerId = getByPassengerId;
        _getByPair = getByPair;
        _getByReservationCode = getByReservationCode;
        _update = update;
        _delete = delete;
        _getAllReservationFlights = getAllReservationFlights;
        _getAllReservations = getAllReservations;
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
            "Crear reservation passenger",
            "Listar reservation passengers",
            "Get reservation passenger by ID",
            "Get reservation passengers by reserva_vuelo_id",
            "Get reservation passengers by passenger_id",
            "Get reservation passenger by reserva_vuelo_id + passenger_id",
            "Get reservation passengers by reservation code (PNR)",
            "Actualizar reservation passenger",
            "Eliminar reservation passenger",
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
                        await PrintReservationFlightsAsync();
                        await PrintPassengersAsync();

                        Console.Write("\nIngrese reserva_vuelo_id: ");
                        int reservationFlightId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese pasajero_id: ");
                        int passengerId = int.Parse(Console.ReadLine()!);

                        await _create.ExecuteAsync(reservationFlightId, passengerId);
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
                        await PrintReservationFlightsAsync();
                        Console.Write("\nIngrese reserva_vuelo_id: ");
                        int byReservationFlightId = int.Parse(Console.ReadLine()!);
                        await PrintManyAsync(await _getByReservationFlightId.ExecuteAsync(byReservationFlightId));
                        break;

                    case 4:
                        await PrintPassengersAsync();
                        Console.Write("\nIngrese pasajero_id: ");
                        int byPassengerId = int.Parse(Console.ReadLine()!);
                        await PrintManyAsync(await _getByPassengerId.ExecuteAsync(byPassengerId));
                        break;

                    case 5:
                        await PrintReservationFlightsAsync();
                        await PrintPassengersAsync();

                        Console.Write("\nIngrese reserva_vuelo_id: ");
                        int pairReservationFlightId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese pasajero_id: ");
                        int pairPassengerId = int.Parse(Console.ReadLine()!);

                        await PrintOneAsync(await _getByPair.ExecuteAsync(pairReservationFlightId, pairPassengerId));
                        break;

                    case 6:
                        Console.Write("Ingrese el codigo_reserva (PNR): ");
                        var code = Console.ReadLine()!;
                        await PrintManyAsync(await _getByReservationCode.ExecuteAsync(code));
                        break;

                    case 7:
                        await PrintReservationFlightsAsync();
                        await PrintPassengersAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo reserva_vuelo_id: ");
                        int newReservationFlightId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo pasajero_id: ");
                        int newPassengerId = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newReservationFlightId, newPassengerId);
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

    private async Task PrintReservationFlightsAsync()
    {
        var reservationFlights = (await _getAllReservationFlights.ExecuteAsync()).ToList();
        var reservationFlightMap = await GetReservationFlightDisplayMapAsync();

        Console.WriteLine("ReservationFlights (top 10):");
        PrintTopWithFormat(reservationFlights, rf => $"{rf.Id.Value} - {GetDisplay(reservationFlightMap, rf.Id.Value)}");

        Console.Write("Buscar reserva_vuelo (texto) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = reservationFlights
            .Where(rf => GetDisplay(reservationFlightMap, rf.Id.Value).ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, rf => $"{rf.Id.Value} - {GetDisplay(reservationFlightMap, rf.Id.Value)}");
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

    private async Task PrintOneAsync(ReservationPassenger? item)
    {
        if (item is null)
        {
            Console.WriteLine("No encontrado");
            return;
        }

        var reservationFlightMap = await GetReservationFlightDisplayMapAsync();
        var passengerMap = await GetPassengerDisplayMapAsync();
        Console.WriteLine(Format(item, reservationFlightMap, passengerMap));
    }

    private async Task PrintManyAsync(IEnumerable<ReservationPassenger> items)
    {
        var reservationFlightMap = await GetReservationFlightDisplayMapAsync();
        var passengerMap = await GetPassengerDisplayMapAsync();

        foreach (var item in items)
            Console.WriteLine(Format(item, reservationFlightMap, passengerMap));
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

    private async Task<Dictionary<int, string>> GetReservationFlightDisplayMapAsync()
    {
        var reservations = await _getAllReservations.ExecuteAsync();
        var reservationFlights = await _getAllReservationFlights.ExecuteAsync();
        var customers = await _getAllCustomers.ExecuteAsync();
        var people = await _getAllPeople.ExecuteAsync();
        var statuses = await _getAllReservationStatuses.ExecuteAsync();

        var flights = await _getAllFlights.ExecuteAsync();
        var airlines = await _getAllAirlines.ExecuteAsync();
        var routes = await _getAllRoutes.ExecuteAsync();
        var airports = await _getAllAirports.ExecuteAsync();

        var personMap = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
        var customerMap = customers.ToDictionary(c => c.Id.Value, c => GetDisplay(personMap, c.PersonId.Value));
        var statusMap = statuses.ToDictionary(s => s.Id.Value, s => s.Name.Value);

        var reservationMap = reservations.ToDictionary(
            r => r.Id.Value,
            r =>
            {
                var code = r.Code?.Value ?? "NULL";
                var customer = GetDisplay(customerMap, r.CustomerId.Value);
                var status = GetDisplay(statusMap, r.StatusId.Value);
                return $"{code} - {customer} - status={status} - total={r.TotalAmount.Value:0.00}";
            });

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

        var flightMap = flights.ToDictionary(
            f => f.Id.Value,
            f =>
            {
                var airline = GetDisplay(airlineMap, f.AirlineId.Value);
                var route = GetDisplay(routeMap, f.RouteId.Value);
                return $"{f.Code.Value} - {airline} - {route} - dep={f.DepartureDateTime.Value:yyyy-MM-dd HH:mm} - avail={f.AvailableSeats.Value}";
            });

        return reservationFlights.ToDictionary(
            rf => rf.Id.Value,
            rf =>
            {
                var reservation = GetDisplay(reservationMap, rf.ReservationId.Value);
                var flight = GetDisplay(flightMap, rf.FlightId.Value);
                return $"{reservation} | {flight} | partial={rf.PartialAmount.Value:0.00}";
            });
    }

    private async Task<Dictionary<int, string>> GetPassengerDisplayMapAsync()
    {
        var passengers = await _getAllPassengers.ExecuteAsync();
        var people = await _getAllPeople.ExecuteAsync();
        var personMap = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");

        return passengers.ToDictionary(p => p.Id.Value, p => GetDisplay(personMap, p.PersonId.Value));
    }

    private static string Format(
        ReservationPassenger item,
        Dictionary<int, string> reservationFlightMap,
        Dictionary<int, string> passengerMap)
    {
        var rfDisplay = GetDisplay(reservationFlightMap, item.ReservationFlightId.Value);
        var paxDisplay = GetDisplay(passengerMap, item.PassengerId.Value);

        return $"{item.Id.Value} - reserva_vuelo={rfDisplay} [{item.ReservationFlightId.Value}] - pasajero={paxDisplay} [{item.PassengerId.Value}]";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? display : $"#{id}";
    }
}


