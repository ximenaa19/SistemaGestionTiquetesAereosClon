// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\UI\PaymentMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.PaymentMethods.Application.UseCases;
using GestionAerolineas.src.Modules.PaymentStates.Application.UseCases;
using GestionAerolineas.src.Modules.Payments.Application.UseCases;
using GestionAerolineas.src.Modules.Payments.Domain.Aggregate;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;

namespace GestionAerolineas.src.Modules.Payments.UI;

public class PaymentMenu
{
    private const int TopCount = 10;

    private readonly CreatePaymentUseCase _create;
    private readonly GetAllPaymentsUseCase _getAll;
    private readonly GetPaymentByIdUseCase _getById;
    private readonly GetPaymentsByReservationIdUseCase _getByReservationId;
    private readonly GetPaymentsByReservationCodeUseCase _getByReservationCode;
    private readonly GetPaymentsByStateIdUseCase _getByStateId;
    private readonly GetPaymentsByMethodIdUseCase _getByMethodId;
    private readonly GetPaymentsByDateRangeUseCase _getByDateRange;
    private readonly UpdatePaymentUseCase _update;
    private readonly DeletePaymentUseCase _delete;

    private readonly GetAllReservationsUseCase _getAllReservations;
    private readonly GetAllCustomersUseCase _getAllCustomers;
    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllReservationStatusesUseCase _getAllReservationStatuses;
    private readonly GetAllPaymentStatesUseCase _getAllPaymentStates;
    private readonly GetAllPaymentMethodsUseCase _getAllPaymentMethods;

    public PaymentMenu(
        CreatePaymentUseCase create,
        GetAllPaymentsUseCase getAll,
        GetPaymentByIdUseCase getById,
        GetPaymentsByReservationIdUseCase getByReservationId,
        GetPaymentsByReservationCodeUseCase getByReservationCode,
        GetPaymentsByStateIdUseCase getByStateId,
        GetPaymentsByMethodIdUseCase getByMethodId,
        GetPaymentsByDateRangeUseCase getByDateRange,
        UpdatePaymentUseCase update,
        DeletePaymentUseCase delete,
        GetAllReservationsUseCase getAllReservations,
        GetAllCustomersUseCase getAllCustomers,
        GetAllPeopleUseCase getAllPeople,
        GetAllReservationStatusesUseCase getAllReservationStatuses,
        GetAllPaymentStatesUseCase getAllPaymentStates,
        GetAllPaymentMethodsUseCase getAllPaymentMethods)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByReservationId = getByReservationId;
        _getByReservationCode = getByReservationCode;
        _getByStateId = getByStateId;
        _getByMethodId = getByMethodId;
        _getByDateRange = getByDateRange;
        _update = update;
        _delete = delete;
        _getAllReservations = getAllReservations;
        _getAllCustomers = getAllCustomers;
        _getAllPeople = getAllPeople;
        _getAllReservationStatuses = getAllReservationStatuses;
        _getAllPaymentStates = getAllPaymentStates;
        _getAllPaymentMethods = getAllPaymentMethods;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear payment",
            "Listar payments",
            "Get payment by ID",
            "Get payments by reserva_id",
            "Get payments by reservation code (PNR)",
            "Get payments by estado_pago_id",
            "Get payments by metodo_pago_id",
            "Get payments by fecha_pago range",
            "Actualizar payment",
            "Eliminar payment",
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
                        await PrintPaymentStatesAsync();
                        await PrintPaymentMethodsAsync();

                        Console.Write("\nIngrese reserva_id: ");
                        int reservationId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese monto: ");
                        decimal amount = decimal.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_pago (yyyy-MM-dd HH:mm) [default=now]: ");
                        var paidAtInput = Console.ReadLine();
                        DateTime paidAt = string.IsNullOrWhiteSpace(paidAtInput) ? DateTime.Now : DateTime.Parse(paidAtInput!);

                        Console.Write("Ingrese estado_pago_id: ");
                        int stateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese metodo_pago_id: ");
                        int methodId = int.Parse(Console.ReadLine()!);

                        await _create.ExecuteAsync(reservationId, amount, paidAt, stateId, methodId);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        await PrintManyAsync(await _getAll.ExecuteAsync());
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);
                        await PrintOneAsync(await _getById.ExecuteAsync(searchId));
                        break;

                    case 3:
                        await PrintReservationsAsync();
                        Console.Write("\nIngrese reserva_id: ");
                        int byReservationId = int.Parse(Console.ReadLine()!);
                        await PrintManyAsync(await _getByReservationId.ExecuteAsync(byReservationId));
                        break;

                    case 4:
                        Console.Write("Ingrese el codigo_reserva (PNR): ");
                        var code = Console.ReadLine()!;
                        await PrintManyAsync(await _getByReservationCode.ExecuteAsync(code));
                        break;

                    case 5:
                        await PrintPaymentStatesAsync();
                        Console.Write("\nIngrese estado_pago_id: ");
                        int byStateId = int.Parse(Console.ReadLine()!);
                        await PrintManyAsync(await _getByStateId.ExecuteAsync(byStateId));
                        break;

                    case 6:
                        await PrintPaymentMethodsAsync();
                        Console.Write("\nIngrese metodo_pago_id: ");
                        int byMethodId = int.Parse(Console.ReadLine()!);
                        await PrintManyAsync(await _getByMethodId.ExecuteAsync(byMethodId));
                        break;

                    case 7:
                        Console.Write("Desde (yyyy-MM-dd): ");
                        var from = DateTime.Parse(Console.ReadLine()!);
                        Console.Write("Hasta (yyyy-MM-dd): ");
                        var to = DateTime.Parse(Console.ReadLine()!);
                        await PrintManyAsync(await _getByDateRange.ExecuteAsync(from.Date, to.Date.AddDays(1).AddTicks(-1)));
                        break;

                    case 8:
                        await PrintReservationsAsync();
                        await PrintPaymentStatesAsync();
                        await PrintPaymentMethodsAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese reserva_id: ");
                        int newReservationId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese monto: ");
                        decimal newAmount = decimal.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_pago (yyyy-MM-dd HH:mm): ");
                        DateTime newPaidAt = DateTime.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese estado_pago_id: ");
                        int newStateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese metodo_pago_id: ");
                        int newMethodId = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newReservationId, newAmount, newPaidAt, newStateId, newMethodId);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 9:
                        Console.Write("Ingrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);
                        var deleted = await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine(deleted ? "âœ” Eliminado" : "No encontrado");
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

    private async Task PrintPaymentStatesAsync()
    {
        var states = (await _getAllPaymentStates.ExecuteAsync()).ToList();
        Console.WriteLine("\nPaymentStates:");
        PrintTopWithFormat(states, s => $"{s.Id.Value} - {s.Name.Value}");
    }

    private async Task PrintPaymentMethodsAsync()
    {
        var methods = (await _getAllPaymentMethods.ExecuteAsync()).ToList();
        Console.WriteLine("\nPaymentMethods:");
        PrintTopWithFormat(methods, m =>
        {
            var cardType = m.CardTypeId is null ? "NULL" : m.CardTypeId.Value.ToString();
            var cardIssuer = m.CardIssuerId is null ? "NULL" : m.CardIssuerId.Value.ToString();
            return $"{m.Id.Value} - {m.CommercialName.Value} - type={m.PaymentMethodTypeId.Value} - cardType={cardType} - issuer={cardIssuer}";
        });

        Console.Write("Buscar metodo (texto) [opcional]: ");
        var search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
            return;

        var normalized = search.Trim().ToUpperInvariant();
        var matches = methods
            .Where(m => m.CommercialName.Value.ToUpperInvariant().Contains(normalized))
            .ToList();

        Console.WriteLine($"\nCoincidencias (top {TopCount}):");
        PrintTopWithFormat(matches, m => $"{m.Id.Value} - {m.CommercialName.Value}");
    }

    private async Task PrintOneAsync(Payment? item)
    {
        if (item is null)
        {
            Console.WriteLine("No encontrado");
            return;
        }

        var reservationMap = await GetReservationDisplayMapAsync();
        var stateMap = await GetPaymentStateDisplayMapAsync();
        var methodMap = await GetPaymentMethodDisplayMapAsync();
        Console.WriteLine(Format(item, reservationMap, stateMap, methodMap));
    }

    private async Task PrintManyAsync(IEnumerable<Payment> items)
    {
        var reservationMap = await GetReservationDisplayMapAsync();
        var stateMap = await GetPaymentStateDisplayMapAsync();
        var methodMap = await GetPaymentMethodDisplayMapAsync();

        var list = items.ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        foreach (var item in list)
            Console.WriteLine(Format(item, reservationMap, stateMap, methodMap));
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

    private async Task<Dictionary<int, string>> GetPaymentStateDisplayMapAsync()
    {
        var states = await _getAllPaymentStates.ExecuteAsync();
        return states.ToDictionary(s => s.Id.Value, s => s.Name.Value);
    }

    private async Task<Dictionary<int, string>> GetPaymentMethodDisplayMapAsync()
    {
        var methods = await _getAllPaymentMethods.ExecuteAsync();
        return methods.ToDictionary(m => m.Id.Value, m => m.CommercialName.Value);
    }

    private static string Format(
        Payment item,
        Dictionary<int, string> reservationMap,
        Dictionary<int, string> stateMap,
        Dictionary<int, string> methodMap)
    {
        var reservation = GetDisplay(reservationMap, item.ReservationId.Value);
        var state = GetDisplay(stateMap, item.StateId.Value);
        var method = GetDisplay(methodMap, item.MethodId.Value);

        return $"{item.Id.Value} - reserva={reservation} - monto={item.Amount.Value:0.00} - fecha={item.PaidAt.Value:yyyy-MM-dd HH:mm} - estado={state} - metodo={method}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}


