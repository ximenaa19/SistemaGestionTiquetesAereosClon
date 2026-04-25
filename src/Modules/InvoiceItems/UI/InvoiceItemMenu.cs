// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\UI\InvoiceItemMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Globalization;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Application.UseCases;
using GestionAerolineas.src.Modules.InvoiceItems.Application.UseCases;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Aggregate;
using GestionAerolineas.src.Modules.Invoices.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.InvoiceItems.UI;

public class InvoiceItemMenu
{
    private const int TopCount = 10;

    private readonly CreateInvoiceItemUseCase _create;
    private readonly GetAllInvoiceItemsUseCase _getAll;
    private readonly GetInvoiceItemByIdUseCase _getById;
    private readonly GetInvoiceItemsByInvoiceIdUseCase _getByInvoiceId;
    private readonly GetInvoiceItemsByItemTypeIdUseCase _getByItemTypeId;
    private readonly GetInvoiceItemsByReservationPassengerIdUseCase _getByReservationPassengerId;
    private readonly UpdateInvoiceItemUseCase _update;
    private readonly DeleteInvoiceItemUseCase _delete;

    private readonly GetAllInvoicesUseCase _getAllInvoices;
    private readonly GetAllInvoiceItemTypesUseCase _getAllTypes;
    private readonly ReservationFlightRepository _reservationFlightRepository;
    private readonly ReservationPassengerRepository _reservationPassengerRepository;
    private readonly GetAllPassengersUseCase _getAllPassengers;
    private readonly GetAllPeopleUseCase _getAllPeople;

    public InvoiceItemMenu(
        CreateInvoiceItemUseCase create,
        GetAllInvoiceItemsUseCase getAll,
        GetInvoiceItemByIdUseCase getById,
        GetInvoiceItemsByInvoiceIdUseCase getByInvoiceId,
        GetInvoiceItemsByItemTypeIdUseCase getByItemTypeId,
        GetInvoiceItemsByReservationPassengerIdUseCase getByReservationPassengerId,
        UpdateInvoiceItemUseCase update,
        DeleteInvoiceItemUseCase delete,
        GetAllInvoicesUseCase getAllInvoices,
        GetAllInvoiceItemTypesUseCase getAllTypes,
        ReservationFlightRepository reservationFlightRepository,
        ReservationPassengerRepository reservationPassengerRepository,
        GetAllPassengersUseCase getAllPassengers,
        GetAllPeopleUseCase getAllPeople)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByInvoiceId = getByInvoiceId;
        _getByItemTypeId = getByItemTypeId;
        _getByReservationPassengerId = getByReservationPassengerId;
        _update = update;
        _delete = delete;
        _getAllInvoices = getAllInvoices;
        _getAllTypes = getAllTypes;
        _reservationFlightRepository = reservationFlightRepository;
        _reservationPassengerRepository = reservationPassengerRepository;
        _getAllPassengers = getAllPassengers;
        _getAllPeople = getAllPeople;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear invoice item",
            "Listar invoice items",
            "Get invoice item by ID",
            "Get invoice items by factura_id",
            "Get invoice items by tipo_item_id",
            "Get invoice items by reserva_pasajero_id",
            "Actualizar invoice item",
            "Eliminar invoice item",
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
                        await PrintInvoicesAsync();
                        await PrintTypesAsync();

                        Console.Write("\nIngrese factura_id: ");
                        int invoiceId = int.Parse(Console.ReadLine()!);

                        await PrintReservationPassengersForInvoiceAsync(invoiceId);

                        Console.Write("Ingrese tipo_item_id: ");
                        int typeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese descripcion: ");
                        var desc = Console.ReadLine() ?? string.Empty;

                        Console.Write("Ingrese cantidad [default=1]: ");
                        var qtyInput = Console.ReadLine();
                        int qty = string.IsNullOrWhiteSpace(qtyInput) ? 1 : int.Parse(qtyInput!);

                        Console.Write("Ingrese precio_unitario: ");
                        decimal unit = decimal.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese reserva_pasajero_id [opcional, 0=NULL]: ");
                        var rpInput = Console.ReadLine();
                        int? rpId = string.IsNullOrWhiteSpace(rpInput) ? null : int.Parse(rpInput!);
                        if (rpId == 0) rpId = null;

                        await _create.ExecuteAsync(invoiceId, typeId, desc, qty, unit, rpId);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        await PrintListAsync(await _getAll.ExecuteAsync());
                        break;

                    case 2:
                        await PrintItemsForSelectionAsync();
                        Console.Write("\nIngrese el ID: ");
                        int id = int.Parse(Console.ReadLine()!);
                        var byId = await _getById.ExecuteAsync(id);
                        if (byId is null)
                        {
                            Console.WriteLine("(sin registros)");
                            break;
                        }
                        PrintOne(byId);
                        break;

                    case 3:
                        await PrintInvoicesAsync();
                        Console.Write("\nIngrese factura_id: ");
                        int invId = int.Parse(Console.ReadLine()!);
                        await PrintListAsync(await _getByInvoiceId.ExecuteAsync(invId));
                        break;

                    case 4:
                        await PrintTypesAsync();
                        Console.Write("\nIngrese tipo_item_id: ");
                        int tId = int.Parse(Console.ReadLine()!);
                        await PrintListAsync(await _getByItemTypeId.ExecuteAsync(tId));
                        break;

                    case 5:
                        Console.Write("\nIngrese reserva_pasajero_id: ");
                        int rpSearch = int.Parse(Console.ReadLine()!);
                        await PrintListAsync(await _getByReservationPassengerId.ExecuteAsync(rpSearch));
                        break;

                    case 6:
                        await PrintItemsForSelectionAsync();
                        await PrintInvoicesAsync();
                        await PrintTypesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese factura_id: ");
                        int updateInvoiceId = int.Parse(Console.ReadLine()!);

                        await PrintReservationPassengersForInvoiceAsync(updateInvoiceId);

                        Console.Write("Ingrese tipo_item_id: ");
                        int updateTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese descripcion: ");
                        var updateDesc = Console.ReadLine() ?? string.Empty;

                        Console.Write("Ingrese cantidad: ");
                        int updateQty = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese precio_unitario: ");
                        decimal updateUnit = decimal.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese reserva_pasajero_id [opcional, 0=NULL]: ");
                        var updateRpInput = Console.ReadLine();
                        int? updateRpId = string.IsNullOrWhiteSpace(updateRpInput) ? null : int.Parse(updateRpInput!);
                        if (updateRpId == 0) updateRpId = null;

                        await _update.ExecuteAsync(updateId, updateInvoiceId, updateTypeId, updateDesc, updateQty, updateUnit, updateRpId);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 7:
                        await PrintItemsForSelectionAsync();
                        Console.Write("\nIngrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);
                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("âœ” Eliminado");
                        break;

                    case 8:
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

    private async Task PrintInvoicesAsync()
    {
        var invoices = (await _getAllInvoices.ExecuteAsync()).ToList();

        Console.WriteLine("Invoices (top 10):");
        foreach (var inv in invoices.Take(TopCount))
            Console.WriteLine($"{inv.Id.Value} - {inv.Number.Value} - reserva_id={inv.ReservationId.Value} - total={inv.Total.Value:0.00}");

        Console.Write("Buscar factura (texto) [opcional]: ");
        var search = (Console.ReadLine() ?? string.Empty).Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalized = search.ToUpperInvariant();
            var matches = invoices
                .Where(i => $"{i.Number.Value} {i.ReservationId.Value}".ToUpperInvariant().Contains(normalized))
                .Take(TopCount)
                .ToList();

            Console.WriteLine("\nCoincidencias (top 10):");
            if (matches.Count == 0) Console.WriteLine("(sin registros)");
            else
                foreach (var inv in matches)
                    Console.WriteLine($"{inv.Id.Value} - {inv.Number.Value} - reserva_id={inv.ReservationId.Value} - total={inv.Total.Value:0.00}");
        }
    }

    private async Task PrintTypesAsync()
    {
        var types = (await _getAllTypes.ExecuteAsync()).ToList();
        Console.WriteLine("\nInvoiceItemTypes (top 10):");
        foreach (var t in types.Take(TopCount))
            Console.WriteLine($"{t.Id.Value} - {t.Name.Value}");

        Console.Write("Buscar tipo (texto) [opcional]: ");
        var search = (Console.ReadLine() ?? string.Empty).Trim();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalized = search.ToUpperInvariant();
            var matches = types.Where(t => t.Name.Value.ToUpperInvariant().Contains(normalized)).Take(TopCount).ToList();
            Console.WriteLine("\nCoincidencias (top 10):");
            if (matches.Count == 0) Console.WriteLine("(sin registros)");
            else foreach (var t in matches) Console.WriteLine($"{t.Id.Value} - {t.Name.Value}");
        }
    }

    private async Task PrintReservationPassengersForInvoiceAsync(int invoiceId)
    {
        var invoices = (await _getAllInvoices.ExecuteAsync()).ToList();
        var invoice = invoices.FirstOrDefault(i => i.Id.Value == invoiceId);
        if (invoice is null)
            return;

        var reservationId = invoice.ReservationId.Value;
        var flights = (await _reservationFlightRepository.GetByReservationIdAsync(
            ReservationFlightReservationId.Create(reservationId))).ToList();

        if (flights.Count == 0)
            return;

        var passengers = (await _getAllPassengers.ExecuteAsync()).ToList();
        var people = (await _getAllPeople.ExecuteAsync()).ToList();
        var passengerToPerson = passengers.ToDictionary(p => p.Id.Value, p => p.PersonId.Value);
        var personMap = people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");

        var reservationPassengers = new List<(int rpId, int passengerId, string name)>();

        foreach (var f in flights)
        {
            var rps = await _reservationPassengerRepository.GetByReservationFlightIdAsync(
                ReservationPassengerReservationFlightId.Create(f.Id.Value));

            foreach (var rp in rps)
            {
                var pid = rp.PassengerId.Value;
                var name = passengerToPerson.TryGetValue(pid, out var personId) && personMap.TryGetValue(personId, out var n)
                    ? n
                    : $"#{pid}";
                reservationPassengers.Add((rp.Id.Value, pid, name));
            }
        }

        if (reservationPassengers.Count == 0)
            return;

        Console.WriteLine("\nReservationPassengers (para esta reserva, top 10):");
        foreach (var rp in reservationPassengers.Take(TopCount))
            Console.WriteLine($"{rp.rpId} - passenger={rp.name} [{rp.passengerId}]");
    }

    private async Task PrintItemsForSelectionAsync()
    {
        Console.WriteLine("InvoiceItems (primeros 30):");
        var list = (await _getAll.ExecuteAsync()).Take(30).ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        foreach (var item in list)
            Console.WriteLine(Format(item));
    }

    private async Task PrintListAsync(IEnumerable<InvoiceItem> list)
    {
        var items = list.ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("(sin resultados)");
            return;
        }

        foreach (var item in items)
            Console.WriteLine(Format(item));
    }

    private static void PrintOne(InvoiceItem item)
    {
        Console.WriteLine(Format(item));
    }

    private static string Format(InvoiceItem item)
    {
        var rp = item.ReservationPassengerId.Value?.ToString() ?? "NULL";
        return $"{item.Id.Value} - factura_id={item.InvoiceId.Value} - tipo_item_id={item.ItemTypeId.Value} - desc={item.Description.Value} - qty={item.Quantity.Value} - unit={item.UnitPrice.Value:0.00} - subtotal={item.Subtotal.Value:0.00} - reserva_pasajero_id={rp}";
    }
}

