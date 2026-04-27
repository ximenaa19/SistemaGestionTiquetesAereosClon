using GestionAerolineas.src.Modules.Baggage.Application.UseCases;
using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Tickets.Application.UseCases;
using GestionAerolineas.src.Modules.Tickets.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.Baggage.UI;

// menu para cliente autenticado: solo permite operar sobre sus propias reservas y tiquetes
public class CustomerBaggageMenu
{
    // cliente que inicio sesion; por eso no se pide customer_id por consola
    private readonly int _customerId;
    // casos de uso del modulo Baggage
    private readonly RegisterBaggageUseCase _register;
    private readonly PreviewBaggageSurchargeUseCase _preview;
    private readonly GetBaggageByCustomerUseCase _getByCustomer;
    private readonly GetCabinTypesForBaggageUseCase _getCabinTypes;
    private readonly GetBaggageRegistrationContextUseCase _getRegistrationContext;
    // casos de uso externos para listar reservas y tiquetes propios del cliente
    private readonly GetReservationsByCustomerIdUseCase _getReservationsByCustomerId;
    private readonly GetTicketsByReservationCodeUseCase _getTicketsByReservationCode;

    public CustomerBaggageMenu(
        int customerId,
        RegisterBaggageUseCase register,
        PreviewBaggageSurchargeUseCase preview,
        GetBaggageByCustomerUseCase getByCustomer,
        GetCabinTypesForBaggageUseCase getCabinTypes,
        GetBaggageRegistrationContextUseCase getRegistrationContext,
        GetReservationsByCustomerIdUseCase getReservationsByCustomerId,
        GetTicketsByReservationCodeUseCase getTicketsByReservationCode)
    {
        // guarda el cliente autenticado y las dependencias necesarias del menu
        _customerId = customerId;
        _register = register;
        _preview = preview;
        _getByCustomer = getByCustomer;
        _getCabinTypes = getCabinTypes;
        _getRegistrationContext = getRegistrationContext;
        _getReservationsByCustomerId = getReservationsByCustomerId;
        _getTicketsByReservationCode = getTicketsByReservationCode;
    }

    public async Task StartAsync()
    {
        // ciclo principal del menu de autoservicio del cliente
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== MI EQUIPAJE Y RECARGOS ===");
            Console.WriteLine("1. Ver mi equipaje registrado");
            Console.WriteLine("2. Registrar equipaje por una de mis reservas");
            Console.WriteLine("3. Registrar equipaje por uno de mis tiquetes");
            Console.WriteLine("4. Ver mis recargos aplicados");
            Console.WriteLine("0. Volver");
            Console.Write("Opcion: ");

            var option = Console.ReadLine();
            Console.WriteLine();

            try
            {
                // ejecuta la opcion seleccionada por el cliente
                switch (option)
                {
                    case "1":
                        await PrintMyBaggageAsync();
                        break;
                    case "2":
                        await RegisterByOwnReservationAsync();
                        break;
                    case "3":
                        await RegisterByOwnTicketAsync();
                        break;
                    case "4":
                        await PrintMySurchargesAsync();
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
                // muestra mensajes de validacion sin cerrar la aplicacion
                Console.WriteLine($"Error: {ex.GetBaseException().Message}");
                Pause();
            }
        }
    }

    private async Task PrintMyBaggageAsync()
    {
        Console.WriteLine("=== Mi equipaje registrado ===");
        // consulta usando el customerId de la sesion actual
        var records = await _getByCustomer.ExecuteAsync(_customerId);
        PrintRecords(records);
        Pause();
    }

    private async Task PrintMySurchargesAsync()
    {
        Console.WriteLine("=== Mis recargos aplicados ===");
        // obtiene todos los registros del cliente y filtra en memoria los que tienen recargo
        var records = (await _getByCustomer.ExecuteAsync(_customerId))
            .Where(x => x.TotalSurcharge > 0)
            .ToList();

        PrintRecords(records);
        Pause();
    }

    private async Task RegisterByOwnReservationAsync()
    {
        Console.WriteLine("=== Registrar equipaje por reserva ===");
        // lista solo reservas que pertenecen al cliente autenticado
        var reservations = await GetOwnReservationsAsync();
        if (reservations.Count == 0)
        {
            Console.WriteLine("No tienes reservas registradas.");
            Pause();
            return;
        }

        PrintReservations(reservations);
        var reservationId = ReadInt("\nIngresa reservation_id: ");
        // seguridad funcional: bloquea reservas que no pertenezcan al cliente
        if (!reservations.Any(x => x.Id.Value == reservationId))
            throw new InvalidOperationException("Esa reserva no pertenece a tu cuenta.");

        // obtiene contexto y sigue el flujo normal de registro
        var context = await _getRegistrationContext.ExecuteByReservationAsync(reservationId);
        PrintRegistrationContext(context);
        var input = await ReadBaggageInputAsync();
        // previsualiza el recargo antes de guardar
        var preview = await _preview.ExecuteAsync(input.CabinTypeId, input.BaggageType, input.Quantity, input.TotalWeightKg);

        PrintCalculation(preview, input.Quantity, input.WeightPerBagKg, input.TotalWeightKg);
        // si el cliente no confirma, no se persiste el equipaje
        if (!Confirm("Confirmar registro y actualizar total de la reserva? (s/n): "))
            return;

        // registra sobre una reserva verificada como propia
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

    private async Task RegisterByOwnTicketAsync()
    {
        Console.WriteLine("=== Registrar equipaje por tiquete ===");
        // lista solo tiquetes asociados a reservas del cliente autenticado
        var tickets = await GetOwnTicketsAsync();
        if (tickets.Count == 0)
        {
            Console.WriteLine("No tienes tiquetes emitidos.");
            Pause();
            return;
        }

        PrintTickets(tickets);
        var ticketId = ReadInt("\nIngresa ticket_id: ");
        // seguridad funcional: evita usar tiquetes de otro cliente
        if (!tickets.Any(x => x.Id.Value == ticketId))
            throw new InvalidOperationException("Ese tiquete no pertenece a tu cuenta.");

        // obtiene contexto por tiquete y continua con el mismo flujo de calculo
        var context = await _getRegistrationContext.ExecuteByTicketAsync(ticketId);
        PrintRegistrationContext(context);
        var input = await ReadBaggageInputAsync();
        // muestra el calculo antes de guardar
        var preview = await _preview.ExecuteAsync(input.CabinTypeId, input.BaggageType, input.Quantity, input.TotalWeightKg);

        PrintCalculation(preview, input.Quantity, input.WeightPerBagKg, input.TotalWeightKg);
        // confirmacion final antes de registrar y actualizar total
        if (!Confirm("Confirmar registro y actualizar total de la reserva? (s/n): "))
            return;

        // registra sobre un tiquete verificado como propio
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

    private async Task<List<Reservation>> GetOwnReservationsAsync()
    {
        // obtiene reservas del cliente y las ordena de mas recientes a mas antiguas
        return (await _getReservationsByCustomerId.ExecuteAsync(_customerId))
            .OrderByDescending(x => x.ReservedAt.Value)
            .ToList();
    }

    private async Task<List<Ticket>> GetOwnTicketsAsync()
    {
        // parte de las reservas propias para no consultar tiquetes ajenos
        var reservations = (await GetOwnReservationsAsync())
            .Where(x => !string.IsNullOrWhiteSpace(x.Code?.Value))
            .ToList();

        // acumula los tiquetes de cada reserva propia
        var tickets = new List<Ticket>();
        foreach (var reservation in reservations)
            tickets.AddRange(await _getTicketsByReservationCode.ExecuteAsync(reservation.Code!.Value));

        // ordena los tiquetes por fecha de emision
        return tickets
            .OrderByDescending(x => x.IssuedAt.Value)
            .ToList();
    }

    private async Task<BaggageInput> ReadBaggageInputAsync()
    {
        // muestra cabinas disponibles antes de pedir el id
        await PrintCabinTypesAsync();
        var cabinTypeId = ReadInt("Id de clase/cabina: ");

        Console.WriteLine("Tipo de equipaje:");
        Console.WriteLine("1. Equipaje de mano");
        Console.WriteLine("2. Equipaje en bodega");
        Console.Write("Tipo: ");
        var baggageType = Console.ReadLine() ?? string.Empty;

        // lee cantidad y peso por maleta
        var quantity = ReadInt("Cantidad de maletas: ");
        var weightPerBagKg = ReadDecimal("Peso por maleta en kg: ");
        // calcula peso total automaticamente
        var totalWeightKg = decimal.Round(quantity * weightPerBagKg, 2);
        Console.Write("Observaciones/descripcion (opcional): ");
        var description = Console.ReadLine();

        return new BaggageInput(cabinTypeId, baggageType, quantity, weightPerBagKg, totalWeightKg, description);
    }

    private async Task PrintCabinTypesAsync()
    {
        // consulta cabinas reales de la base de datos
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

    private static void PrintReservations(IReadOnlyList<Reservation> reservations)
    {
        // imprime reservas propias con id para que el cliente seleccione una
        Console.WriteLine("Tus reservas:");
        foreach (var item in reservations)
            Console.WriteLine($"[{item.Id.Value}] PNR={item.Code?.Value ?? "NULL"} | Total={item.TotalAmount.Value:0.00} | Fecha={item.ReservedAt.Value:yyyy-MM-dd HH:mm}");
    }

    private static void PrintTickets(IReadOnlyList<Ticket> tickets)
    {
        // imprime tiquetes propios con id para que el cliente seleccione uno
        Console.WriteLine("Tus tiquetes:");
        foreach (var item in tickets)
            Console.WriteLine($"[{item.Id.Value}] Code={item.Code.Value} | Emision={item.IssuedAt.Value:yyyy-MM-dd HH:mm}");
    }

    private static void PrintRecords(IReadOnlyList<BaggageRecordView> records)
    {
        // muestra mensaje amigable si el cliente aun no tiene equipaje registrado
        if (records.Count == 0)
        {
            Console.WriteLine("No hay registros para mostrar.");
            return;
        }

        foreach (var item in records)
        {
            // imprime una linea resumida para cada registro del cliente
            Console.WriteLine(
                $"{item.Id} - reserva={item.ReservationCode ?? item.ReservationId.ToString()} - " +
                $"ticket={item.TicketCode ?? item.TicketId?.ToString() ?? "N/A"} - " +
                $"vuelo={item.FlightCode ?? item.FlightId?.ToString() ?? "N/A"} - " +
                $"pasajero={item.PassengerName ?? "N/A"} - cabina={item.CabinTypeName ?? item.CabinTypeId.ToString()} - " +
                $"tipo={item.BaggageType} - cantidad={item.Quantity} - kg={item.WeightKg:0.00} - " +
                $"recargo={item.TotalSurcharge:0.00} - fecha={item.RegisteredAt:yyyy-MM-dd HH:mm}");
        }
    }

    private static void PrintCalculation(BaggageSurchargeResult result, int quantity, decimal weightPerBagKg, decimal totalWeightKg)
    {
        // muestra la politica aplicada y el detalle del recargo antes de confirmar
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
        // confirma el registro y muestra como cambio el total de la reserva
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
        // confirma visualmente la reserva, tiquete, vuelo y pasajero relacionados
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
        // fuerza la lectura de enteros positivos
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
        // fuerza la lectura de decimales positivos
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
        // interpreta respuestas afirmativas para confirmar el registro
        Console.Write(prompt);
        var value = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();
        return value is "S" or "SI" or "Y" or "YES";
    }

    private static void Pause()
    {
        // detiene la pantalla para que el usuario pueda leer mensajes
        Console.WriteLine();
        Console.WriteLine("Presiona una tecla para continuar...");
        Console.ReadKey();
    }

    // estructura interna con los datos capturados del equipaje
    private sealed record BaggageInput(
        int CabinTypeId,
        string BaggageType,
        int Quantity,
        decimal WeightPerBagKg,
        decimal TotalWeightKg,
        string? Description);
}
