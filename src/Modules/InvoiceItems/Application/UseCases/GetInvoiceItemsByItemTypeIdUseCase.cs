// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Application\UseCases\GetInvoiceItemsByItemTypeIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Repositories;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItems.Application.UseCases;

public class GetInvoiceItemsByItemTypeIdUseCase
{
    private readonly IInvoiceItemRepository _repository;

    public GetInvoiceItemsByItemTypeIdUseCase(IInvoiceItemRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<InvoiceItem>> ExecuteAsync(int itemTypeId)
    {
        return _repository.GetByItemTypeIdAsync(InvoiceItemTypeId.Create(itemTypeId));
    }
}

