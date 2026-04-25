// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\UI\InvoiceMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Globalization;
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.Invoices.Application.UseCases;
using GestionAerolineas.src.Modules.Invoices.Domain.Aggregate;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;

namespace GestionAerolineas.src.Modules.Invoices.UI;

public class InvoiceMenu
{
    private const int TopCount = 10;

    private readonly CreateInvoiceUseCase _create;
    private readonly GetAllInvoicesUseCase _getAll;
    private readonly GetInvoiceByIdUseCase _getById;
    private readonly GetInvoiceByNumberUseCase _getByNumber;
    private readonly GetInvoiceByReservationIdUseCase _getByReservationId;
    private readonly GetInvoicesByIssueDateRangeUseCase _getByDateRange;
    private readonly GetInvoiceDetailsByIdUseCase _getDetailsById;
    private readonly UpdateInvoiceUseCase _update;
    private readonly DeleteInvoiceUseCase _delete;

    private readonly GetAllReservationsUseCase _getAllReservations;
    private readonly GetAllCustomersUseCase _getAllCustomers;
    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllReservationStatusesUseCase _getAllStatuses;

    public InvoiceMenu(
        CreateInvoiceUseCase create,
        GetAllInvoicesUseCase getAll,
        GetInvoiceByIdUseCase getById,
        GetInvoiceByNumberUseCase getByNumber,
        GetInvoiceByReservationIdUseCase getByReservationId,
        GetInvoicesByIssueDateRangeUseCase getByDateRange,
        GetInvoiceDetailsByIdUseCase getDetailsById,
        UpdateInvoiceUseCase update,
        DeleteInvoiceUseCase delete,
        GetAllReservationsUseCase getAllReservations,
        GetAllCustomersUseCase getAllCustomers,
        GetAllPeopleUseCase getAllPeople,
        GetAllReservationStatusesUseCase getAllStatuses)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByNumber = getByNumber;
        _getByReservationId = getByReservationId;
        _getByDateRange = getByDateRange;
        _getDetailsById = getDetailsById;
        _update = update;
        _delete = delete;
        _getAllReservations = getAllReservations;
        _getAllCustomers = getAllCustomers;
        _getAllPeople = getAllPeople;
        _getAllStatuses = getAllStatuses;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear invoice",
            "Listar invoices",
            "Get invoice by ID",
            "Get invoice by invoice number",
            "Get invoice by reserva_id",
            "Get invoices by fecha_emision range",
            "Get invoice details by ID",
            "Actualizar invoice",
            "Eliminar invoice",
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

                        Console.Write("\nIngrese reserva_id: ");
                        int reservationId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese fecha_emision (yyyy-MM-dd HH:mm) [default=now]: ");
                        var issuedAtInput = Console.ReadLine();
                        DateTime? issuedAt = string.IsNullOrWhiteSpace(issuedAtInput)
                            ? null
                            : DateTime.Parse(issuedAtInput!, CultureInfo.InvariantCulture);

                        var created = await _create.ExecuteAsync(reservationId, issuedAt);
                        Console.WriteLine($"âœ” Creado: id={created.Id.Value}, number={created.Number.Value}");
                        break;

                    case 1:
                        await PrintListAsync(await _getAll.ExecuteAsync());
                        break;

                    case 2:
                        await PrintInvoicesForSelectionAsync();
                        Console.Write("\nIngrese el ID: ");
                        int id = int.Parse(Console.ReadLine()!);
                        var byId = await _getById.ExecuteAsync(id);
                        if (byId is null)
                        {
                            Console.WriteLine("(sin registros)");
                            break;
                        }
                        await PrintOneAsync(byId);
                        break;

                    case 3:
                        Console.Write("Ingrese numero_factura: ");
                        var number = Console.ReadLine() ?? string.Empty;
                        var byNumber = await _getByNumber.ExecuteAsync(number);
                        if (byNumber is null)
                        {
                            Console.WriteLine("(sin registros)");
                            break;
                        }
                        await PrintOneAsync(byNumber);
                        break;

                    case 4:
                        await PrintReservationsAsync();
                        Console.Write("\nIngrese reserva_id: ");
                        int rId = int.Parse(Console.ReadLine()!);
                        var byRes = await _getByReservationId.ExecuteAsync(rId);
                        if (byRes is null)
                        {
                            Console.WriteLine("(sin registros)");
                            break;
                        }
                        await PrintOneAsync(byRes);
                        break;

                    case 5:
                        Console.Write("Desde (yyyy-MM-dd): ");
                        var from = DateTime.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);
                        Console.Write("Hasta (yyyy-MM-dd): ");
                        var to = DateTime.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

                        var fromDt = from.Date;
                        var toDt = to.Date.AddDays(1).AddTicks(-1);
                        await PrintListAsync(await _getByDateRange.ExecuteAsync(fromDt, toDt));
                        break;

                    case 6:
                        await PrintInvoicesForSelectionAsync();
                        Console.Write("\nIngrese el ID: ");
                        int detailsId = int.Parse(Console.ReadLine()!);
                        var details = await _getDetailsById.ExecuteAsync(detailsId);
                        if (details is null)
                        {
                            Console.WriteLine("(sin registros)");
                            break;
                        }

                        await PrintOneAsync(details.Invoice);
                        Console.WriteLine("\n=== ITEMS ===");
                        if (details.Items.Count == 0)
                        {
                            Console.WriteLine("(sin items)");
                        }
                        else
                        {
                            foreach (var item in details.Items)
                                Console.WriteLine($"- itemId={item.Id.Value} - type={item.ItemTypeId.Value} - desc={item.Description.Value} - qty={item.Quantity.Value} - unit={item.UnitPrice.Value:0.00} - subtotal={item.Subtotal.Value:0.00} - reserva_pasajero_id={item.ReservationPassengerId.Value?.ToString() ?? "NULL"}");
                        }
                        break;

                    case 7:
                        await PrintInvoicesForSelectionAsync();
                        await PrintReservationsAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese reserva_id: ");
                        int updateReservationId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese numero_factura: ");
                        var updateNumber = Console.ReadLine() ?? string.Empty;

                        Console.Write("Ingrese fecha_emision (yyyy-MM-dd HH:mm): ");
                        var updateIssuedAt = DateTime.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

                        await _update.ExecuteAsync(updateId, updateReservationId, updateNumber, updateIssuedAt);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 8:
                        await PrintInvoicesForSelectionAsync();
                        Console.Write("\nIngrese el ID: ");
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
        }
    }

    private async Task PrintInvoicesForSelectionAsync()
    {
        Console.WriteLine("Invoices (primeros 30):");
        var list = (await _getAll.ExecuteAsync()).Take(30).ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        var reservationMap = await GetReservationDisplayMapAsync();
        foreach (var item in list)
            Console.WriteLine(Format(item, reservationMap));
    }

    private async Task PrintListAsync(IEnumerable<Invoice> list)
    {
        var items = list.ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("(sin resultados)");
            return;
        }

        var reservationMap = await GetReservationDisplayMapAsync();
        foreach (var item in items)
            Console.WriteLine(Format(item, reservationMap));
    }

    private async Task PrintOneAsync(Invoice item)
    {
        var reservationMap = await GetReservationDisplayMapAsync();
        Console.WriteLine(Format(item, reservationMap));
    }

    private async Task PrintReservationsAsync()
    {
        var reservations = (await _getAllReservations.ExecuteAsync()).ToList();
        var customers = (await _getAllCustomers.ExecuteAsync()).ToList();
        var people = (await _getAllPeople.ExecuteAsync()).ToList();
        var statuses = (await _getAllStatuses.ExecuteAsync()).ToList();

        var customerToPerson = customers.ToDictionary(c => c.Id.Value, c => c.PersonId.Value);
        var personMap = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
        var statusMap = statuses.ToDictionary(s => s.Id.Value, s => s.Name.Value);

        Console.WriteLine("Reservations (top 10):");
        foreach (var r in reservations.OrderByDescending(r => r.Id.Value).Take(TopCount))
        {
            var code = r.Code?.Value ?? "NULL";
            var customerName = personMap.TryGetValue(customerToPerson[r.CustomerId.Value], out var n) ? n : $"#{r.CustomerId.Value}";
            var statusName = statusMap.TryGetValue(r.StatusId.Value, out var s) ? s : $"#{r.StatusId.Value}";
            Console.WriteLine($"{r.Id.Value} - {code} - {customerName} [{r.CustomerId.Value}] - status={statusName} [{r.StatusId.Value}] - total={r.TotalAmount.Value:0.00}");
        }

        Console.Write("Buscar reserva (texto) [opcional]: ");
        var search = (Console.ReadLine() ?? string.Empty).Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalized = search.ToUpperInvariant();
            var matches = reservations
                .Select(r =>
                {
                    var code = (r.Code?.Value ?? string.Empty).Trim().ToUpperInvariant();
                    var customerName = personMap.TryGetValue(customerToPerson[r.CustomerId.Value], out var n) ? n.ToUpperInvariant() : string.Empty;
                    var haystack = $"{code} {customerName}";
                    return new { r, haystack };
                })
                .Where(x => x.haystack.Contains(normalized))
                .Select(x => x.r)
                .OrderByDescending(r => r.Id.Value)
                .Take(TopCount)
                .ToList();

            Console.WriteLine("\nCoincidencias (top 10):");
            if (matches.Count == 0)
            {
                Console.WriteLine("(sin registros)");
            }
            else
            {
                foreach (var r in matches)
                {
                    var code = r.Code?.Value ?? "NULL";
                    var customerName = personMap.TryGetValue(customerToPerson[r.CustomerId.Value], out var n) ? n : $"#{r.CustomerId.Value}";
                    var statusName = statusMap.TryGetValue(r.StatusId.Value, out var s) ? s : $"#{r.StatusId.Value}";
                    Console.WriteLine($"{r.Id.Value} - {code} - {customerName} [{r.CustomerId.Value}] - status={statusName} [{r.StatusId.Value}] - total={r.TotalAmount.Value:0.00}");
                }
            }
        }
    }

    private async Task<Dictionary<int, string>> GetReservationDisplayMapAsync()
    {
        var reservations = (await _getAllReservations.ExecuteAsync()).ToList();
        var customers = (await _getAllCustomers.ExecuteAsync()).ToList();
        var people = (await _getAllPeople.ExecuteAsync()).ToList();
        var statuses = (await _getAllStatuses.ExecuteAsync()).ToList();

        var customerToPerson = customers.ToDictionary(c => c.Id.Value, c => c.PersonId.Value);
        var personMap = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
        var statusMap = statuses.ToDictionary(s => s.Id.Value, s => s.Name.Value);

        var result = new Dictionary<int, string>();
        foreach (var r in reservations)
        {
            var code = r.Code?.Value ?? "NULL";
            var customerName = personMap.TryGetValue(customerToPerson[r.CustomerId.Value], out var n) ? n : $"#{r.CustomerId.Value}";
            var statusName = statusMap.TryGetValue(r.StatusId.Value, out var s) ? s : $"#{r.StatusId.Value}";
            result[r.Id.Value] = $"{code} - {customerName} - status={statusName}";
        }

        return result;
    }

    private static string Format(Invoice item, Dictionary<int, string> reservationMap)
    {
        var reservationDisplay = reservationMap.TryGetValue(item.ReservationId.Value, out var d) ? $"{d} [{item.ReservationId.Value}]" : $"#{item.ReservationId.Value}";
        var issuedAt = item.IssuedAt.Value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        return $"{item.Id.Value} - number={item.Number.Value} - reserva={reservationDisplay} - issuedAt={issuedAt} - subtotal={item.Subtotal.Value:0.00} - impuestos={item.Taxes.Value:0.00} - total={item.Total.Value:0.00}";
    }
}


