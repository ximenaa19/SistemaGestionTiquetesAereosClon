// [DocHeader]
// Modulo: Reservations
// Capa: UI
// Archivo: src\Modules\Reservations\UI\CustomerRescheduleReservationFlow.cs
// Responsabilidad: Flujo de consola para reprogramar reservas propias de cliente y gestionar lista de espera.
using GestionAerolineas.src.Modules.FlightStates.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Reservations.UI;

public sealed class CustomerRescheduleReservationFlow
{
    private readonly int _customerId;
    private readonly string _username;
    private readonly GetReservationsByCustomerIdUseCase _getReservationsByCustomerId;
    private readonly GetReservationDetailsByIdUseCase _getReservationDetailsById;
    private readonly GetAllReservationStatusesUseCase _getAllReservationStatuses;
    private readonly GetAllFlightsUseCase _getAllFlights;
    private readonly GetFlightByIdUseCase _getFlightById;
    private readonly GetAllFlightStatesUseCase _getAllFlightStates;
    private readonly RescheduleReservationUseCase _rescheduleReservation;
    private readonly AddReservationToWaitlistUseCase _addReservationToWaitlist;

    public CustomerRescheduleReservationFlow(
        int customerId,
        string username,
        GetReservationsByCustomerIdUseCase getReservationsByCustomerId,
        GetReservationDetailsByIdUseCase getReservationDetailsById,
        GetAllReservationStatusesUseCase getAllReservationStatuses,
        GetAllFlightsUseCase getAllFlights,
        GetFlightByIdUseCase getFlightById,
        GetAllFlightStatesUseCase getAllFlightStates,
        RescheduleReservationUseCase rescheduleReservation,
        AddReservationToWaitlistUseCase addReservationToWaitlist)
    {
        _customerId = customerId;
        _username = username;
        _getReservationsByCustomerId = getReservationsByCustomerId;
        _getReservationDetailsById = getReservationDetailsById;
        _getAllReservationStatuses = getAllReservationStatuses;
        _getAllFlights = getAllFlights;
        _getFlightById = getFlightById;
        _getAllFlightStates = getAllFlightStates;
        _rescheduleReservation = rescheduleReservation;
        _addReservationToWaitlist = addReservationToWaitlist;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            PrintHeader();

            var confirmed = await GetConfirmedReservationsAsync();
            if (confirmed.Count == 0)
            {
                Console.WriteLine("No tienes reservas confirmadas para reprogramar.");
                Pause();
                return;
            }

            AdminFlowConsole.PrintMenuBox(
                "RESERVAS CONFIRMADAS",
                confirmed.Select(x => $"[{x.Id}] Reserva {x.Code} - Estado={x.StatusName}").ToList());

            var reservationId = ReadIntRequired("Ingrese reservation_id");
            if (!reservationId.HasValue)
                return;

            var selected = confirmed.FirstOrDefault(x => x.Id == reservationId.Value);
            if (selected is null)
            {
                AdminFlowConsole.PrintError("La reserva no existe o no pertenece al cliente activo.");
                continue;
            }

            var details = await _getReservationDetailsById.ExecuteAsync(selected.Id);
            if (details is null || details.ReservationFlights.Count == 0)
            {
                AdminFlowConsole.PrintError("La reserva no tiene vuelos para reprogramar.");
                continue;
            }

            var reservationFlightId = await SelectReservationFlightIdAsync(details);
            if (!reservationFlightId.HasValue)
                return;

            var currentRf = details.ReservationFlights.First(x => x.Id.Value == reservationFlightId.Value);
            var currentFlight = await _getFlightById.ExecuteAsync(currentRf.FlightId.Value);
            if (currentFlight is null)
            {
                AdminFlowConsole.PrintError("No se encontro el vuelo actual de la reserva.");
                continue;
            }

            var compatibles = await GetCompatibleFlightsAsync(currentFlight.Id.Value, currentFlight.RouteId.Value);
            if (compatibles.Count == 0)
            {
                Console.WriteLine("No hay vuelos disponibles");
                Pause();
                return;
            }

            AdminFlowConsole.PrintMenuBox(
                "VUELOS COMPATIBLES",
                compatibles.Select(f => $"[{f.Id}] {f.Display} | cupo={f.AvailableSeats}").ToList());

            var newFlightId = ReadIntRequired("Ingrese nuevo vuelo_id");
            if (!newFlightId.HasValue)
                return;

            var newFlight = compatibles.FirstOrDefault(x => x.Id == newFlightId.Value);
            if (newFlight is null)
            {
                AdminFlowConsole.PrintError("El vuelo no existe en la lista compatible.");
                continue;
            }

            if (newFlight.Id == currentFlight.Id.Value)
            {
                AdminFlowConsole.PrintError("El nuevo vuelo debe ser diferente al vuelo actual.");
                continue;
            }

            var summaryLines = new List<string>
            {
                $"Cliente: {_username} (customer_id={_customerId})",
                $"Reserva: {selected.Id} - {selected.Code}",
                $"Vuelo actual: {currentFlight.Code.Value} [{currentFlight.Id.Value}]",
                $"Nuevo vuelo: {newFlight.Display}",
                $"Motivo: Reprogramacion solicitada por cliente"
            };
            var decision = AdminFlowConsole.ReadConfirmChoice(summaryLines);
            if (decision == 2)
                continue;
            if (decision == 3)
                return;

            try
            {
                if (newFlight.AvailableSeats > 0)
                {
                    await _rescheduleReservation.ExecuteAsync(
                        _customerId,
                        selected.Id,
                        currentRf.Id.Value,
                        newFlight.Id,
                        "Reprogramacion manual por cliente");
                    Console.WriteLine("Reserva reprogramada correctamente.");
                }
                else
                {
                    var joinWaitlist = AdminFlowConsole.ReadYesNo("El vuelo no tiene cupo. Desea entrar a lista de espera? (s/n)");
                    if (joinWaitlist is null)
                        return;

                    if (joinWaitlist.Value)
                    {
                        await _addReservationToWaitlist.AddAsync(
                            selected.Id,
                            currentRf.Id.Value,
                            newFlight.Id,
                            "Sin cupo al reprogramar");
                        Console.WriteLine("La reserva fue agregada a la lista de espera.");
                    }
                    else
                    {
                        Console.WriteLine("No se realizaron cambios.");
                    }
                }
            }
            catch (Exception ex)
            {
                AdminFlowConsole.PrintError(ex.GetBaseException().Message);
            }

            Pause();
            return;
        }
    }

    private async Task<int?> SelectReservationFlightIdAsync(
        GestionAerolineas.src.Modules.Reservations.Domain.Aggregate.ReservationDetails details)
    {
        if (details.ReservationFlights.Count == 1)
            return details.ReservationFlights[0].Id.Value;

        var lines = details.ReservationFlights
            .Select(x => $"[{x.Id.Value}] reservation_flight_id - flight_id={x.FlightId.Value}")
            .ToList();
        AdminFlowConsole.PrintMenuBox("VUELOS ACTUALES DE LA RESERVA", lines);

        return ReadIntRequired("Ingrese reservation_flight_id a reprogramar");
    }

    private async Task<List<ReservationView>> GetConfirmedReservationsAsync()
    {
        var statusMap = (await _getAllReservationStatuses.ExecuteAsync())
            .ToDictionary(x => x.Id.Value, x => x.Name.Value);

        var all = (await _getReservationsByCustomerId.ExecuteAsync(_customerId)).ToList();
        var list = new List<ReservationView>();
        foreach (var item in all)
        {
            var name = statusMap.TryGetValue(item.StatusId.Value, out var n) ? n : $"#{item.StatusId.Value}";
            if (!name.Trim().ToUpperInvariant().Contains("CONFIRM"))
                continue;

            list.Add(new ReservationView(
                item.Id.Value,
                item.Code?.Value ?? "NULL",
                name));
        }

        return list;
    }

    private async Task<List<FlightView>> GetCompatibleFlightsAsync(int currentFlightId, int routeId)
    {
        var flights = (await _getAllFlights.ExecuteAsync())
            .Where(x => x.Id.Value != currentFlightId
                        && x.RouteId.Value == routeId
                        && x.DepartureDateTime.Value > DateTime.Now)
            .OrderBy(x => x.DepartureDateTime.Value)
            .ToList();

        var states = (await _getAllFlightStates.ExecuteAsync())
            .ToDictionary(x => x.Id.Value, x => x.Name.Value.Trim().ToUpperInvariant());

        return flights
            .Where(f =>
            {
                var state = states.TryGetValue(f.StateId.Value, out var name) ? name : string.Empty;
                return state != "CANCELADO" && state != "COMPLETADO";
            })
            .Select(f => new FlightView(
                f.Id.Value,
                $"{f.Code.Value} | route_id={f.RouteId.Value} | {f.DepartureDateTime.Value:yyyy-MM-dd HH:mm}",
                f.AvailableSeats.Value))
            .ToList();
    }

    private int? ReadIntRequired(string label)
    {
        while (true)
        {
            var raw = AdminFlowConsole.ReadRaw(label);
            if (raw == AdminFlowConsole.CancelToken)
                return null;

            if (int.TryParse(raw, out var value))
                return value;

            AdminFlowConsole.PrintError("El valor ingresado no es valido. Intente nuevamente.");
        }
    }

    private void PrintHeader()
    {
        Console.Clear();
        AdminFlowConsole.PrintHeader("REPROGRAMAR RESERVA");
        Console.WriteLine($"Cliente: {_username} (customer_id={_customerId})");
        Console.WriteLine($"Cancela en cualquier campo con: {AdminFlowConsole.CancelToken}");
    }

    private static void Pause()
    {
        Console.WriteLine("\nPresiona una tecla para continuar...");
        Console.ReadKey();
    }

    private sealed record ReservationView(int Id, string Code, string StatusName);
    private sealed record FlightView(int Id, string Display, int AvailableSeats);
}

