// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Application\UseCases\GetInvoiceItemByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Repositories;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItems.Application.UseCases;

public class GetInvoiceItemByIdUseCase
{
    private readonly IInvoiceItemRepository _repository;

    public GetInvoiceItemByIdUseCase(IInvoiceItemRepository repository)
    {
        _repository = repository;
    }

    public Task<InvoiceItem?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(InvoiceItemId.Create(id));
    }
}

