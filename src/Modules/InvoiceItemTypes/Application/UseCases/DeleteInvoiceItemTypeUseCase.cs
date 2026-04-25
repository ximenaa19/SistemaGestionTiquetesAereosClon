// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\Application\UseCases\DeleteInvoiceItemTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItemTypes.Application.UseCases;

public class DeleteInvoiceItemTypeUseCase
{
    private readonly IInvoiceItemTypeRepository _repository;

    public DeleteInvoiceItemTypeUseCase(IInvoiceItemTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var invoiceItemTypeId = InvoiceItemTypeId.Create(id);
        var invoiceItemType = await _repository.GetByIdAsync(invoiceItemTypeId);

        if (invoiceItemType is null)
            throw new KeyNotFoundException($"InvoiceItemType con id '{invoiceItemTypeId.Value}' no existe.");

        await _repository.DeleteAsync(invoiceItemType);
    }
}
