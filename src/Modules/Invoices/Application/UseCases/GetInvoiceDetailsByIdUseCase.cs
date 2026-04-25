// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Application\UseCases\GetInvoiceDetailsByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Repositories;
using GestionAerolineas.src.Modules.Invoices.Domain.Aggregate;
using GestionAerolineas.src.Modules.Invoices.Domain.Repositories;
using GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Invoices.Application.UseCases;

public record InvoiceDetails(Invoice Invoice, IReadOnlyList<InvoiceItem> Items);

public class GetInvoiceDetailsByIdUseCase
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IInvoiceItemRepository _itemRepository;

    public GetInvoiceDetailsByIdUseCase(IInvoiceRepository invoiceRepository, IInvoiceItemRepository itemRepository)
    {
        _invoiceRepository = invoiceRepository;
        _itemRepository = itemRepository;
    }

    public async Task<InvoiceDetails?> ExecuteAsync(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(InvoiceId.Create(id));
        if (invoice is null)
            return null;

        var items = (await _itemRepository.GetByInvoiceIdAsync(invoice.Id.Value)).ToList();
        return new InvoiceDetails(invoice, items);
    }
}

