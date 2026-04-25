// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Application\UseCases\GetInvoiceItemsByInvoiceIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Repositories;

namespace GestionAerolineas.src.Modules.InvoiceItems.Application.UseCases;

public class GetInvoiceItemsByInvoiceIdUseCase
{
    private readonly IInvoiceItemRepository _repository;

    public GetInvoiceItemsByInvoiceIdUseCase(IInvoiceItemRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<InvoiceItem>> ExecuteAsync(int invoiceId)
    {
        return _repository.GetByInvoiceIdAsync(invoiceId);
    }
}

