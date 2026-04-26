// [DocHeader]
// Modulo: General
// Capa: General
// Archivo: src\Modules\Reservations\UI\CustomerCreateReservationWizard.cs
// Responsabilidad: Flujo guiado para que un cliente autenticado cree reservas sin pasar por el modulo completo de reservas.
// Flujo: Se ejecuta desde MENU CLIENTE -> opcion "Crear reserva (wizard simple)".
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.DocumentTypes.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.PassengerTypes.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Reservations.UI;

public sealed class CustomerCreateReservationWizard
{
    private readonly int _customerId;
    private readonly string _username;
    private readonly CreateReservationUseCase _createReservation;
    private readonly CreateReservationFlightUseCase _createReservationFlight;
    private readonly CreateReservationPassengerUseCase _createReservationPassenger;
    private readonly GetAllReservationStatusesUseCase _getAllReservationStatuses;
    private readonly GetAllFlightsUseCase _getAllFlights;
    private readonly GetAllAirlinesUseCase _getAllAirlines;
    private readonly GetAllRoutesUseCase _getAllRoutes;
    private readonly GetAllAirportsUseCase _getAllAirports;
    private readonly GetAllDocumentTypesUseCase _getAllDocumentTypes;
    private readonly CreatePersonUseCase _createPerson;
    private readonly GetPersonByDocumentUseCase _getPersonByDocument;
    private readonly CreatePassengerUseCase _createPassenger;
    private readonly GetPassengerByPersonIdUseCase _getPassengerByPersonId;
    private readonly GetAllPassengerTypesUseCase _getAllPassengerTypes;

    public CustomerCreateReservationWizard(
        int customerId,
        string username,
        CreateReservationUseCase createReservation,
        CreateReservationFlightUseCase createReservationFlight,
        CreateReservationPassengerUseCase createReservationPassenger,
        GetAllReservationStatusesUseCase getAllReservationStatuses,
        GetAllFlightsUseCase getAllFlights,
        GetAllAirlinesUseCase getAllAirlines,
        GetAllRoutesUseCase getAllRoutes,
        GetAllAirportsUseCase getAllAirports,
        GetAllDocumentTypesUseCase getAllDocumentTypes,
        CreatePersonUseCase createPerson,
        GetPersonByDocumentUseCase getPersonByDocument,
        CreatePassengerUseCase createPassenger,
        GetPassengerByPersonIdUseCase getPassengerByPersonId,
        GetAllPassengerTypesUseCase getAllPassengerTypes)
    {
        _customerId = customerId;
        _username = username;
        _createReservation = createReservation;
        _createReservationFlight = createReservationFlight;
        _createReservationPassenger = createReservationPassenger;
        _getAllReservationStatuses = getAllReservationStatuses;
        _getAllFlights = getAllFlights;
        _getAllAirlines = getAllAirlines;
        _getAllRoutes = getAllRoutes;
        _getAllAirports = getAllAirports;
        _getAllDocumentTypes = getAllDocumentTypes;
        _createPerson = createPerson;
        _getPersonByDocument = getPersonByDocument;
        _createPassenger = createPassenger;
        _getPassengerByPersonId = getPassengerByPersonId;
        _getAllPassengerTypes = getAllPassengerTypes;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            var draft = await CollectDraftAsync();
            if (draft is null)
            {
                Console.WriteLine("\nOperacion cancelada por el usuario.");
                Pause();
                return;
            }

            var summary = await BuildSummaryAsync(draft);
            var choice = AdminFlowConsole.ReadConfirmChoice(summary);
            if (choice == 2)
                continue;

            if (choice == 3)
            {
                Console.WriteLine("\nOperacion cancelada por el usuario.");
                Pause();
                return;
            }

            try
            {
                var result = await PersistAsync(draft);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nReserva creada correctamente. Id={result.ReservationId}, Codigo={result.ReservationCode}");
                Console.ResetColor();
                Console.WriteLine($"Vuelos asociados: {result.FlightsCount}");
                Console.WriteLine($"Pasajeros asociados: {result.PassengersCount}");
            }
            catch (Exception ex)
            {
                AdminFlowConsole.PrintError($"No se pudo completar la operacion: {ex.GetBaseException().Message}");
            }

            Pause();
            return;
        }
    }

    private async Task<WizardDraft?> CollectDraftAsync()
    {
        var flightIds = await SelectFlightsAsync();
        if (flightIds is null)
            return null;

        var passengers = await CollectPassengersAsync();
        if (passengers is null)
            return null;

        return new WizardDraft(flightIds, passengers);
    }

    private async Task<List<int>?> SelectFlightsAsync()
    {
        var flights = (await _getAllFlights.ExecuteAsync())
            .Where(f => f.AvailableSeats.Value > 0)
            .OrderBy(f => f.DepartureDateTime.Value)
            .ToList();

        if (flights.Count == 0)
            throw new Exception("No hay vuelos disponibles con asientos.");

        var flightMap = await GetFlightDisplayMapAsync();
        var selected = new List<int>();

        while (true)
        {
            PrintContext();
            AdminFlowConsole.PrintMenuBox(
                "VUELOS DISPONIBLES",
                flights.Select(f => $"[{f.Id.Value}] {GetDisplay(flightMap, f.Id.Value)}").ToList());

            if (selected.Count > 0)
                AdminFlowConsole.PrintMenuBox("VUELOS SELECCIONADOS", selected.Select(x => $"[{x}]").ToList());

            var raw = AdminFlowConsole.ReadRaw("Ingrese ID de vuelo (000000 cancela)");
            if (raw == AdminFlowConsole.CancelToken)
                return null;

            if (!int.TryParse(raw, out var flightId))
            {
                AdminFlowConsole.PrintError("El valor ingresado no es valido. Intente nuevamente.");
                continue;
            }

            var exists = flights.Any(x => x.Id.Value == flightId);
            if (!exists)
            {
                AdminFlowConsole.PrintError("El vuelo no existe.");
                continue;
            }

            if (selected.Contains(flightId))
            {
                AdminFlowConsole.PrintError("Ese vuelo ya fue agregado.");
                continue;
            }

            selected.Add(flightId);
            var addMore = AdminFlowConsole.ReadYesNo("Desea agregar otro vuelo? (s/n)");
            if (addMore is null)
                return null;
            if (!addMore.Value)
                break;
        }

        return selected;
    }

    private async Task<List<PassengerDraft>?> CollectPassengersAsync()
    {
        var list = new List<PassengerDraft>();
        var addPassenger = AdminFlowConsole.ReadYesNo("Desea agregar un nuevo pasajero? (s/n)");
        if (addPassenger is null)
            return null;

        while (addPassenger.Value)
        {
            var draft = await ReadPassengerDraftAsync();
            if (draft is null)
                return null;

            list.Add(draft);

            addPassenger = AdminFlowConsole.ReadYesNo("Desea agregar otro pasajero? (s/n)");
            if (addPassenger is null)
                return null;
        }

        return list;
    }

    private async Task<PassengerDraft?> ReadPassengerDraftAsync()
    {
        var documentTypes = (await _getAllDocumentTypes.ExecuteAsync())
            .OrderBy(x => x.Id.Value)
            .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.Code.Value})"))
            .ToList();

        var documentType = AdminFlowConsole.SelectById(
            "TIPOS DE DOCUMENTO",
            "Seleccione tipo_documento_id",
            documentTypes);
        if (documentType is null)
            return null;

        var documentNumber = AdminFlowConsole.ReadRequiredText("Numero de documento");
        if (documentNumber is null)
            return null;

        var firstNames = AdminFlowConsole.ReadRequiredText("Nombres");
        if (firstNames is null)
            return null;

        var lastNames = AdminFlowConsole.ReadRequiredText("Apellidos");
        if (lastNames is null)
            return null;

        return new PassengerDraft(
            documentType.Value.id,
            documentType.Value.name,
            documentNumber,
            firstNames,
            lastNames);
    }

    private async Task<List<string>> BuildSummaryAsync(WizardDraft draft)
    {
        var flightMap = await GetFlightDisplayMapAsync();
        var lines = new List<string>
        {
            $"Cliente principal: {_username} (customer_id={_customerId})",
            $"Cantidad de vuelos: {draft.FlightIds.Count}"
        };

        lines.AddRange(draft.FlightIds.Select((id, i) => $"Vuelo {i + 1}: {GetDisplay(flightMap, id)}"));

        if (draft.Passengers.Count == 0)
        {
            lines.Add("Pasajeros agregados: 0");
        }
        else
        {
            lines.Add($"Pasajeros agregados: {draft.Passengers.Count}");
            lines.AddRange(draft.Passengers.Select((p, i) =>
                $"Pax {i + 1}: {p.FirstNames} {p.LastNames} - {p.DocumentTypeName} {p.DocumentNumber}"));
        }

        return lines;
    }

    private async Task<PersistResult> PersistAsync(WizardDraft draft)
    {
        var statusId = await ResolvePendingStatusIdAsync();
        var expiresAt = DateTime.Now.AddMinutes(15);

        var reservation = await _createReservation.ExecuteAsync(_customerId, statusId, expiresAt);

        var reservationFlights = new List<int>();
        foreach (var flightId in draft.FlightIds)
        {
            // En autoservicio cliente no pedimos valor parcial, se registra en 0 y puede ajustarse luego por proceso comercial.
            var reservationFlight = await _createReservationFlight.ExecuteAsync(reservation.Id.Value, flightId, 0m);
            reservationFlights.Add(reservationFlight.Id.Value);
        }

        var passengerIds = await ResolvePassengerIdsAsync(draft.Passengers);
        foreach (var reservationFlightId in reservationFlights)
        {
            foreach (var passengerId in passengerIds)
                await _createReservationPassenger.ExecuteAsync(reservationFlightId, passengerId);
        }

        return new PersistResult(
            reservation.Id.Value,
            reservation.Code?.Value ?? "NULL",
            reservationFlights.Count,
            passengerIds.Count);
    }

    private async Task<List<int>> ResolvePassengerIdsAsync(IReadOnlyList<PassengerDraft> drafts)
    {
        var result = new List<int>();
        if (drafts.Count == 0)
            return result;

        var defaultPassengerTypeId = await ResolveDefaultPassengerTypeIdAsync();

        foreach (var draft in drafts)
        {
            var person = await _getPersonByDocument.ExecuteAsync(draft.DocumentTypeId, draft.DocumentNumber);
            if (person is null)
            {
                await _createPerson.ExecuteAsync(
                    draft.DocumentTypeId,
                    draft.DocumentNumber,
                    draft.FirstNames,
                    draft.LastNames,
                    null,
                    null,
                    null);

                person = await _getPersonByDocument.ExecuteAsync(draft.DocumentTypeId, draft.DocumentNumber);
                if (person is null)
                    throw new Exception("No se pudo recuperar la persona creada para un pasajero.");
            }

            var passenger = await _getPassengerByPersonId.ExecuteAsync(person.Id.Value);
            if (passenger is null)
            {
                await _createPassenger.ExecuteAsync(person.Id.Value, defaultPassengerTypeId);
                passenger = await _getPassengerByPersonId.ExecuteAsync(person.Id.Value);
                if (passenger is null)
                    throw new Exception("No se pudo recuperar el pasajero creado.");
            }

            if (!result.Contains(passenger.Id.Value))
                result.Add(passenger.Id.Value);
        }

        return result;
    }

    private async Task<int> ResolvePendingStatusIdAsync()
    {
        var statuses = (await _getAllReservationStatuses.ExecuteAsync()).ToList();
        var pending = statuses.FirstOrDefault(s => s.Name.Value.Trim().ToUpperInvariant().Contains("PEND"));
        if (pending is null)
            throw new Exception("No se encontro estado Pendiente en reservationstatuses.");

        return pending.Id.Value;
    }

    private async Task<int> ResolveDefaultPassengerTypeIdAsync()
    {
        var types = (await _getAllPassengerTypes.ExecuteAsync()).ToList();
        if (types.Count == 0)
            throw new Exception("No hay passengertypes registrados.");

        var adult = types.FirstOrDefault(x =>
            x.Name.Value.Trim().ToUpperInvariant().Contains("ADUL"));

        return adult?.Id.Value ?? types.OrderBy(x => x.Id.Value).First().Id.Value;
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
                var destination = GetDisplay(airportMap, r.DestinationAirportId.Value);
                return $"{origin} -> {destination}";
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

    private void PrintContext()
    {
        Console.Clear();
        AdminFlowConsole.PrintHeader("CREAR RESERVA (WIZARD)");
        Console.WriteLine($"Cliente: {_username} (customer_id={_customerId})");
        Console.WriteLine($"Cancela en cualquier campo con: {AdminFlowConsole.CancelToken}");
    }

    private static void Pause()
    {
        Console.WriteLine("\nPresiona una tecla para continuar...");
        Console.ReadKey();
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? display : $"#{id}";
    }

    private sealed record PassengerDraft(
        int DocumentTypeId,
        string DocumentTypeName,
        string DocumentNumber,
        string FirstNames,
        string LastNames);

    private sealed record WizardDraft(
        IReadOnlyList<int> FlightIds,
        IReadOnlyList<PassengerDraft> Passengers);

    private sealed record PersistResult(
        int ReservationId,
        string ReservationCode,
        int FlightsCount,
        int PassengersCount);
}
