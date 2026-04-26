using GestionAerolineas.src.Modules.Baggage.Application.UseCases;
using GestionAerolineas.src.Modules.Baggage.Domain.Models;

namespace GestionAerolineas.src.Modules.Baggage.UI;

public class BaggageMenu
{
    private readonly RegisterBaggageUseCase _register;
    private readonly PreviewBaggageSurchargeUseCase _preview;
    private readonly GetBaggageByCustomerUseCase _getByCustomer;
    private readonly GetBaggageByFlightUseCase _getByFlight;
    private readonly GetBaggageSurchargesUseCase _getSurcharges;
    private readonly GetCabinTypesForBaggageUseCase _getCabinTypes;
    private readonly GetBaggageRegistrationContextUseCase _getRegistrationContext;

    public BaggageMenu(
        RegisterBaggageUseCase register,
        PreviewBaggageSurchargeUseCase preview,
        GetBaggageByCustomerUseCase getByCustomer,
        GetBaggageByFlightUseCase getByFlight,
        GetBaggageSurchargesUseCase getSurcharges,
        GetCabinTypesForBaggageUseCase getCabinTypes,
        GetBaggageRegistrationContextUseCase getRegistrationContext)
    {
        _register = register;
        _preview = preview;
        _getByCustomer = getByCustomer;
        _getByFlight = getByFlight;
        _getSurcharges = getSurcharges;
        _getCabinTypes = getCabinTypes;
        _getRegistrationContext = getRegistrationContext;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== GESTION DE EQUIPAJE Y RECARGOS ===");
            Console.WriteLine("1. Registrar equipaje por tiquete");
            Console.WriteLine("2. Registrar equipaje por reserva");
            Console.WriteLine("3. Consultar equipaje por cliente");
            Console.WriteLine("4. Consultar equipaje por vuelo");
            Console.WriteLine("5. Ver recargos aplicados");
            Console.WriteLine("0. Volver");
            Console.Write("Opcion: ");

            var option = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (option)
                {
                    case "1":
                        await RegisterByTicketAsync();
                        break;
                    case "2":
                        await RegisterByReservationAsync();
                        break;
                    case "3":
                        await PrintByCustomerAsync();
                        break;
                    case "4":
                        await PrintByFlightAsync();
                        break;
                    case "5":
                        await PrintSurchargesAsync();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opcion invalida.");
                        Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetBaseException().Message}");
                Pause();
            }
        }
    }

    private async Task RegisterByTicketAsync()
    {
        Console.WriteLine("=== Registrar equipaje por tiquete ===");
        var ticketId = ReadInt("Id del tiquete: ");
        var context = await _getRegistrationContext.ExecuteByTicketAsync(ticketId);
        PrintRegistrationContext(context);
        var input = await ReadBaggageInputAsync();
        var preview = await _preview.ExecuteAsync(input.CabinTypeId, input.BaggageType, input.Quantity, input.TotalWeightKg);

        PrintCalculation(preview, input.Quantity, input.WeightPerBagKg, input.TotalWeightKg);
        if (!Confirm("Confirmar registro y actualizar total de la reserva? (s/n): "))
            return;

        var result = await _register.ExecuteByTicketAsync(
            ticketId,
            input.CabinTypeId,
            input.BaggageType,
            input.Quantity,
            input.TotalWeightKg,
            input.Description);

        PrintRegistrationResult(result);
        Pause();
    }

    private async Task RegisterByReservationAsync()
    {
        Console.WriteLine("=== Registrar equipaje por reserva ===");
        var reservationId = ReadInt("Id de la reserva: ");
        var context = await _getRegistrationContext.ExecuteByReservationAsync(reservationId);
        PrintRegistrationContext(context);
        var input = await ReadBaggageInputAsync();
        var preview = await _preview.ExecuteAsync(input.CabinTypeId, input.BaggageType, input.Quantity, input.TotalWeightKg);

        PrintCalculation(preview, input.Quantity, input.WeightPerBagKg, input.TotalWeightKg);
        if (!Confirm("Confirmar registro y actualizar total de la reserva? (s/n): "))
            return;

        var result = await _register.ExecuteByReservationAsync(
            reservationId,
            input.CabinTypeId,
            input.BaggageType,
            input.Quantity,
            input.TotalWeightKg,
            input.Description);

        PrintRegistrationResult(result);
        Pause();
    }

    private async Task<BaggageInput> ReadBaggageInputAsync()
    {
        await PrintCabinTypesAsync();
        var cabinTypeId = ReadInt("Id de clase/cabina: ");

        Console.WriteLine("Tipo de equipaje:");
        Console.WriteLine("1. Equipaje de mano");
        Console.WriteLine("2. Equipaje en bodega");
        Console.Write("Tipo: ");
        var baggageType = Console.ReadLine() ?? string.Empty;

        var quantity = ReadInt("Cantidad de maletas: ");
        var weightPerBagKg = ReadDecimal("Peso por maleta en kg: ");
        var totalWeightKg = decimal.Round(quantity * weightPerBagKg, 2);
        Console.Write("Observaciones/descripcion (opcional): ");
        var description = Console.ReadLine();

        return new BaggageInput(cabinTypeId, baggageType, quantity, weightPerBagKg, totalWeightKg, description);
    }

    private async Task PrintCabinTypesAsync()
    {
        var cabinTypes = await _getCabinTypes.ExecuteAsync();
        Console.WriteLine("Clases/cabinas disponibles:");

        if (cabinTypes.Count == 0)
        {
            Console.WriteLine("No hay clases/cabinas registradas.");
            return;
        }

        foreach (var cabin in cabinTypes)
            Console.WriteLine($"{cabin.Id}. {cabin.Name}");

        Console.WriteLine();
    }

    private async Task PrintByCustomerAsync()
    {
        Console.WriteLine("=== Equipaje por cliente ===");
        var customerId = ReadInt("Id del cliente: ");
        var records = await _getByCustomer.ExecuteAsync(customerId);
        PrintRecords(records);
        Pause();
    }

    private async Task PrintByFlightAsync()
    {
        Console.WriteLine("=== Equipaje por vuelo ===");
        var flightId = ReadInt("Id del vuelo: ");
        var records = await _getByFlight.ExecuteAsync(flightId);
        PrintRecords(records);
        Pause();
    }

    private async Task PrintSurchargesAsync()
    {
        Console.WriteLine("=== Recargos aplicados ===");
        var records = await _getSurcharges.ExecuteAsync();
        PrintRecords(records);
        Pause();
    }

    private static void PrintRecords(IReadOnlyList<BaggageRecordView> records)
    {
        if (records.Count == 0)
        {
            Console.WriteLine("No hay registros para mostrar.");
            return;
        }

        foreach (var item in records)
        {
            Console.WriteLine(
                $"{item.Id} - reserva={item.ReservationCode ?? item.ReservationId.ToString()} - " +
                $"ticket={item.TicketCode ?? item.TicketId?.ToString() ?? "N/A"} - " +
                $"vuelo={item.FlightCode ?? item.FlightId?.ToString() ?? "N/A"} - " +
                $"pasajero={item.PassengerName ?? "N/A"} - cabina={item.CabinTypeName ?? item.CabinTypeId.ToString()} - " +
                $"tipo={item.BaggageType} - cantidad={item.Quantity} - kg={item.WeightKg:0.00} - " +
                $"excesoCant={item.ExcessQuantity} - excesoKg={item.ExcessWeightKg:0.00} - recargo={item.TotalSurcharge:0.00} - " +
                $"fecha={item.RegisteredAt:yyyy-MM-dd HH:mm}");
        }
    }

    private static void PrintCalculation(BaggageSurchargeResult result, int quantity, decimal weightPerBagKg, decimal totalWeightKg)
    {
        Console.WriteLine();
        Console.WriteLine("Detalle del calculo:");
        Console.WriteLine($"Clase aplicada: {result.Policy.CabinFamily}");
        Console.WriteLine($"Tipo de equipaje: {result.Policy.BaggageType}");
        Console.WriteLine($"Cantidad permitida: {result.Policy.AllowedQuantity}");
        Console.WriteLine($"Cantidad registrada: {quantity}");
        Console.WriteLine($"Peso permitido por maleta: {result.Policy.AllowedWeightPerBagKg:0.00} kg");
        Console.WriteLine($"Peso registrado por maleta: {weightPerBagKg:0.00} kg");
        Console.WriteLine($"Peso total permitido: {result.Policy.AllowedTotalWeightKg:0.00} kg");
        Console.WriteLine($"Peso registrado: {totalWeightKg:0.00} kg");
        Console.WriteLine($"Exceso de cantidad: {result.ExcessQuantity}");
        Console.WriteLine($"Exceso de peso: {result.ExcessWeightKg:0.00} kg");
        Console.WriteLine($"Recargo por cantidad: {result.QuantitySurcharge:0.00}");
        Console.WriteLine($"Recargo por peso: {result.WeightSurcharge:0.00}");
        Console.WriteLine($"Recargo total: {result.TotalSurcharge:0.00}");
        Console.WriteLine();
    }

    private static void PrintRegistrationResult(RegisterBaggageResult result)
    {
        Console.WriteLine();
        Console.WriteLine("Equipaje registrado correctamente.");
        Console.WriteLine($"Reserva: {result.Context.ReservationCode ?? result.Context.ReservationId.ToString()}");
        Console.WriteLine($"Tiquete: {result.Context.TicketCode ?? result.Context.TicketId?.ToString() ?? "N/A"}");
        Console.WriteLine($"Vuelo: {result.Context.FlightCode ?? result.Context.FlightId?.ToString() ?? "N/A"}");
        Console.WriteLine($"Pasajero: {result.Context.PassengerName ?? "N/A"}");
        Console.WriteLine($"Total anterior reserva: {result.PreviousReservationTotal:0.00}");
        Console.WriteLine($"Recargo aplicado: {result.Surcharge.TotalSurcharge:0.00}");
        Console.WriteLine($"Nuevo total reserva: {result.NewReservationTotal:0.00}");
    }

    private static void PrintRegistrationContext(BaggageRegistrationContext context)
    {
        Console.WriteLine();
        Console.WriteLine("Datos encontrados:");
        Console.WriteLine($"Reserva: {context.ReservationCode ?? context.ReservationId.ToString()}");
        Console.WriteLine($"Tiquete: {context.TicketCode ?? context.TicketId?.ToString() ?? "N/A"}");
        Console.WriteLine($"Vuelo: {context.FlightCode ?? context.FlightId?.ToString() ?? "N/A"}");
        Console.WriteLine($"Pasajero: {context.PassengerName ?? "N/A"}");
        Console.WriteLine($"Valor actual reserva: {context.CurrentReservationTotal:0.00}");
        Console.WriteLine();
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var value) && value > 0)
                return value;

            Console.WriteLine("Ingresa un numero entero mayor que cero.");
        }
    }

    private static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out var value) && value > 0)
                return value;

            Console.WriteLine("Ingresa un numero decimal mayor que cero.");
        }
    }

    private static bool Confirm(string prompt)
    {
        Console.Write(prompt);
        var value = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();
        return value is "S" or "SI" or "Y" or "YES";
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Presiona una tecla para continuar...");
        Console.ReadKey();
    }

    private sealed record BaggageInput(
        int CabinTypeId,
        string BaggageType,
        int Quantity,
        decimal WeightPerBagKg,
        decimal TotalWeightKg,
        string? Description);
}
