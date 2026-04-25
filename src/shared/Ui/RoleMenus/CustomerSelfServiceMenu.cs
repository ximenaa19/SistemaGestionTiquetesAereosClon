// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\shared\Ui\RoleMenus\CustomerSelfServiceMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethods.Application.UseCases;
using GestionAerolineas.src.Modules.PaymentStates.Application.UseCases;
using GestionAerolineas.src.Modules.Payments.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.TicketStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.Tickets.Application.UseCases;
using UpdateReservationStatusUseCase = GestionAerolineas.src.Modules.Reservations.Application.UseCases.UpdateReservationStatusUseCase;

namespace GestionAerolineas.src.shared.Ui.RoleMenus;

/// <summary>
/// Menú de autoservicio para cliente autenticado.
/// Limita consultas/acciones al contexto del customer actual para evitar cruces entre cuentas.
/// </summary>
public sealed class CustomerSelfServiceMenu
{
    private readonly int _customerId;
    private readonly int _personId;
    private readonly int? _passengerId;
    private readonly string _username;
    private readonly GetReservationsByCustomerIdUseCase _getReservationsByCustomerId;
    private readonly GetReservationDetailsByIdUseCase _getReservationDetailsById;
    private readonly GetTicketsByReservationCodeUseCase _getTicketsByReservationCode;
    private readonly GetPaymentsByReservationCodeUseCase _getPaymentsByReservationCode;
    private readonly GetAllReservationStatusesUseCase _getAllReservationStatuses;
    private readonly GetAllTicketStatusesUseCase _getAllTicketStatuses;
    private readonly GetAllPaymentStatesUseCase _getAllPaymentStates;
    private readonly GetAllPaymentMethodsUseCase _getAllPaymentMethods;
    private readonly UpdateReservationStatusUseCase _updateReservationStatus;
    private readonly Func<Task> _viewFlightsAction;
    private readonly Func<Task> _createReservationAction;
    private readonly Func<Task> _checkinAction;
    private readonly Func<Task> _updateProfileAction;
    private readonly Func<Task> _secondaryMenuAction;

    public CustomerSelfServiceMenu(
        int customerId,
        int personId,
        int? passengerId,
        string username,
        GetReservationsByCustomerIdUseCase getReservationsByCustomerId,
        GetReservationDetailsByIdUseCase getReservationDetailsById,
        GetTicketsByReservationCodeUseCase getTicketsByReservationCode,
        GetPaymentsByReservationCodeUseCase getPaymentsByReservationCode,
        GetAllReservationStatusesUseCase getAllReservationStatuses,
        GetAllTicketStatusesUseCase getAllTicketStatuses,
        GetAllPaymentStatesUseCase getAllPaymentStates,
        GetAllPaymentMethodsUseCase getAllPaymentMethods,
        UpdateReservationStatusUseCase updateReservationStatus,
        Func<Task> viewFlightsAction,
        Func<Task> createReservationAction,
        Func<Task> checkinAction,
        Func<Task> updateProfileAction,
        Func<Task> secondaryMenuAction)
    {
        _customerId = customerId;
        _personId = personId;
        _passengerId = passengerId;
        _username = username;
        _getReservationsByCustomerId = getReservationsByCustomerId;
        _getReservationDetailsById = getReservationDetailsById;
        _getTicketsByReservationCode = getTicketsByReservationCode;
        _getPaymentsByReservationCode = getPaymentsByReservationCode;
        _getAllReservationStatuses = getAllReservationStatuses;
        _getAllTicketStatuses = getAllTicketStatuses;
        _getAllPaymentStates = getAllPaymentStates;
        _getAllPaymentMethods = getAllPaymentMethods;
        _updateReservationStatus = updateReservationStatus;
        _viewFlightsAction = viewFlightsAction;
        _createReservationAction = createReservationAction;
        _checkinAction = checkinAction;
        _updateProfileAction = updateProfileAction;
        _secondaryMenuAction = secondaryMenuAction;
    }

    /// <summary>
    /// Inicia el menú principal de cliente con opciones de consulta y gestión personal.
    /// </summary>
    public Task StartAsync()
    {
        var menu = new CustomerRoleMenu(new List<RoleMenuOption>
        {
            new("Ver vuelos disponibles", ViewFlightsAsync),
            new("Crear reserva (wizard simple)", CreateReservationAsync),
            new("Mis reservas", MyReservationsAsync),
            new("Ver detalle de reserva", ReservationDetailsAsync),
            new("Mis tiquetes", MyTicketsAsync),
            new("Mis pagos", MyPaymentsAsync),
            new("Hacer check-in", CheckinAsync),
            new("Cancelar reserva", CancelReservationAsync),
            new("Actualizar mi perfil basico", UpdateProfileAsync),
            new("Menu secundario", _secondaryMenuAction)
        });

        return menu.StartAsync();
    }

    /// <summary>
    /// Redirige al módulo de vuelos manteniendo contexto visual del cliente.
    /// </summary>
    private async Task ViewFlightsAsync()
    {
        PrintContext("Ver vuelos disponibles");
        await _viewFlightsAction();
    }

    /// <summary>
    /// Guía al cliente para crear una reserva usando su <c>customer_id</c>.
    /// </summary>
    private async Task CreateReservationAsync()
    {
        PrintContext("Crear reserva");
        Console.WriteLine($"Usa este customer_id cuando el asistente lo pida: {_customerId}");
        Console.WriteLine("Presiona una tecla para continuar...");
        Console.ReadKey();
        await _createReservationAction();
    }

    /// <summary>
    /// Lista únicamente las reservas pertenecientes al cliente logueado.
    /// </summary>
    private async Task MyReservationsAsync()
    {
        PrintContext("Mis reservas");
        var statusMap = (await _getAllReservationStatuses.ExecuteAsync())
            .ToDictionary(x => x.Id.Value, x => x.Name.Value);

        var reservations = (await _getReservationsByCustomerId.ExecuteAsync(_customerId))
            .OrderByDescending(x => x.ReservedAt.Value)
            .ToList();

        if (reservations.Count == 0)
        {
            Console.WriteLine("No tienes reservas registradas.");
            Pause();
            return;
        }

        foreach (var item in reservations)
        {
            var statusName = statusMap.TryGetValue(item.StatusId.Value, out var value) ? value : $"#{item.StatusId.Value}";
            Console.WriteLine(
                $"[{item.Id.Value}] PNR={item.Code?.Value ?? "NULL"} | Estado={statusName} | Total={item.TotalAmount.Value:0.00} | Fecha={item.ReservedAt.Value:yyyy-MM-dd HH:mm}");
        }

        Pause();
    }

    /// <summary>
    /// Muestra detalle de una reserva validando previamente propiedad sobre la misma.
    /// </summary>
    private async Task ReservationDetailsAsync()
    {
        PrintContext("Detalle de reserva");
        Console.Write("Ingresa reservation_id: ");
        if (!int.TryParse(Console.ReadLine(), out var reservationId))
        {
            Console.WriteLine("ID invalido.");
            Pause();
            return;
        }

        var own = await _getReservationsByCustomerId.ExecuteAsync(_customerId);
        if (!own.Any(x => x.Id.Value == reservationId))
        {
            Console.WriteLine("Esa reserva no pertenece a tu cuenta.");
            Pause();
            return;
        }

        var details = await _getReservationDetailsById.ExecuteAsync(reservationId);
        if (details is null)
        {
            Console.WriteLine("No se encontro la reserva.");
            Pause();
            return;
        }

        Console.WriteLine($"Reserva [{details.Reservation.Id.Value}] PNR={details.Reservation.Code?.Value ?? "NULL"}");
        Console.WriteLine($"Total: {details.Reservation.TotalAmount.Value:0.00}");
        Console.WriteLine($"Estado ID: {details.Reservation.StatusId.Value}");
        Console.WriteLine($"Vuelos asociados: {details.ReservationFlights.Count}");
        Console.WriteLine($"Pasajeros asociados: {details.ReservationPassengers.Count}");
        Pause();
    }

    /// <summary>
    /// Consulta los tiquetes asociados a las reservas del cliente.
    /// </summary>
    private async Task MyTicketsAsync()
    {
        PrintContext("Mis tiquetes");
        var reservations = (await _getReservationsByCustomerId.ExecuteAsync(_customerId))
            .Where(x => !string.IsNullOrWhiteSpace(x.Code?.Value))
            .ToList();

        if (reservations.Count == 0)
        {
            Console.WriteLine("No tienes reservas con PNR para consultar tiquetes.");
            Pause();
            return;
        }

        var statusMap = (await _getAllTicketStatuses.ExecuteAsync())
            .ToDictionary(x => x.Id.Value, x => x.Name.Value);

        var total = 0;
        foreach (var reservation in reservations)
        {
            var pnr = reservation.Code!.Value;
            var tickets = (await _getTicketsByReservationCode.ExecuteAsync(pnr)).ToList();
            foreach (var ticket in tickets)
            {
                total++;
                var statusName = statusMap.TryGetValue(ticket.StatusId.Value, out var value) ? value : $"#{ticket.StatusId.Value}";
                Console.WriteLine(
                    $"[{ticket.Id.Value}] Code={ticket.Code.Value} | Estado={statusName} | Emision={ticket.IssuedAt.Value:yyyy-MM-dd HH:mm} | PNR={pnr}");
            }
        }

        if (total == 0)
            Console.WriteLine("No tienes tiquetes emitidos.");

        Pause();
    }

    /// <summary>
    /// Consulta pagos del cliente a partir de los PNR de sus reservas.
    /// </summary>
    private async Task MyPaymentsAsync()
    {
        PrintContext("Mis pagos");
        var reservations = (await _getReservationsByCustomerId.ExecuteAsync(_customerId))
            .Where(x => !string.IsNullOrWhiteSpace(x.Code?.Value))
            .ToList();

        if (reservations.Count == 0)
        {
            Console.WriteLine("No tienes reservas para consultar pagos.");
            Pause();
            return;
        }

        var stateMap = (await _getAllPaymentStates.ExecuteAsync())
            .ToDictionary(x => x.Id.Value, x => x.Name.Value);
        var methodMap = (await _getAllPaymentMethods.ExecuteAsync())
            .ToDictionary(x => x.Id.Value, x => x.CommercialName.Value);

        var total = 0;
        foreach (var reservation in reservations)
        {
            var pnr = reservation.Code!.Value;
            var payments = (await _getPaymentsByReservationCode.ExecuteAsync(pnr)).ToList();
            foreach (var payment in payments)
            {
                total++;
                var stateName = stateMap.TryGetValue(payment.StateId.Value, out var state) ? state : $"#{payment.StateId.Value}";
                var methodName = methodMap.TryGetValue(payment.MethodId.Value, out var method) ? method : $"#{payment.MethodId.Value}";
                Console.WriteLine(
                    $"[{payment.Id.Value}] Monto={payment.Amount.Value:0.00} | Estado={stateName} | Metodo={methodName} | Fecha={payment.PaidAt.Value:yyyy-MM-dd HH:mm} | PNR={pnr}");
            }
        }

        if (total == 0)
            Console.WriteLine("No tienes pagos registrados.");

        Pause();
    }

    /// <summary>
    /// Redirige al módulo de check-in mostrando contexto de pasajero cuando existe.
    /// </summary>
    private async Task CheckinAsync()
    {
        PrintContext("Hacer check-in");
        if (_passengerId.HasValue)
            Console.WriteLine($"Tu passenger_id detectado es: {_passengerId.Value}");
        else
            Console.WriteLine("No se detecto passenger_id para tu persona. Podras usar el menu general.");

        Console.WriteLine("Se abrira el modulo de check-in.");
        Console.WriteLine("Presiona una tecla para continuar...");
        Console.ReadKey();
        await _checkinAction();
    }

    /// <summary>
    /// Permite cancelar una reserva propia llevando su estado a "Cancelada" si la transición es válida.
    /// </summary>
    private async Task CancelReservationAsync()
    {
        PrintContext("Cancelar reserva");
        var reservations = (await _getReservationsByCustomerId.ExecuteAsync(_customerId))
            .OrderByDescending(x => x.ReservedAt.Value)
            .ToList();

        if (reservations.Count == 0)
        {
            Console.WriteLine("No tienes reservas para cancelar.");
            Pause();
            return;
        }

        foreach (var reservation in reservations)
            Console.WriteLine($"[{reservation.Id.Value}] PNR={reservation.Code?.Value ?? "NULL"} | EstadoId={reservation.StatusId.Value}");

        Console.Write("\nIngresa reservation_id a cancelar: ");
        if (!int.TryParse(Console.ReadLine(), out var reservationId))
        {
            Console.WriteLine("ID invalido.");
            Pause();
            return;
        }

        var ownReservation = reservations.FirstOrDefault(x => x.Id.Value == reservationId);
        if (ownReservation is null)
        {
            Console.WriteLine("Esa reserva no pertenece a tu cuenta.");
            Pause();
            return;
        }

        var cancelStatus = (await _getAllReservationStatuses.ExecuteAsync())
            .FirstOrDefault(x => x.Name.Value.Trim().ToUpperInvariant().Contains("CANCEL"));

        if (cancelStatus is null)
        {
            Console.WriteLine("No existe un estado de reserva 'Cancelada' en catalogos.");
            Pause();
            return;
        }

        try
        {
            await _updateReservationStatus.ExecuteAsync(reservationId, cancelStatus.Id.Value);
            Console.WriteLine("Reserva cancelada correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"No se pudo cancelar: {ex.GetBaseException().Message}");
        }

        Pause();
    }

    /// <summary>
    /// Abre el submenú de perfil básico (correo/teléfono) usando el <c>person_id</c> del cliente.
    /// </summary>
    private async Task UpdateProfileAsync()
    {
        PrintContext("Actualizar perfil basico");
        Console.WriteLine($"Tu person_id es: {_personId}");
        Console.WriteLine("Se abrira el submenu para gestionar correo/telefono.");
        Console.WriteLine("Usa ese person_id cuando se solicite.");
        Console.WriteLine("Presiona una tecla para continuar...");
        Console.ReadKey();
        await _updateProfileAction();
    }

    /// <summary>
    /// Imprime cabecera estandarizada del módulo cliente con identificadores de contexto.
    /// </summary>
    private void PrintContext(string title)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================");
        Console.WriteLine($"     CLIENTE - {title.ToUpperInvariant()}");
        Console.WriteLine("========================================");
        Console.ResetColor();
        Console.WriteLine($"Usuario: {_username} | customer_id: {_customerId} | person_id: {_personId}");
        Console.WriteLine();
    }

    /// <summary>
    /// Pausa estándar de consola para lectura de resultados.
    /// </summary>
    private static void Pause()
    {
        Console.WriteLine("\nPresiona una tecla para continuar...");
        Console.ReadKey();
    }
}
