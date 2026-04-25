// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\Application\Services\InvoiceItemTypeValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItemTypes.Application.Services;

public class InvoiceItemTypeValidator : IInvoiceItemTypeValidator
{
    private readonly IInvoiceItemTypeRepository _repository;

    public InvoiceItemTypeValidator(IInvoiceItemTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateNameAsync(InvoiceItemTypeName name, InvoiceItemTypeId? currentId = null)
    {
        var existingByName = await _repository.GetByNameAsync(name);

        if (existingByName is not null && existingByName.Id != currentId)
            throw new Exception("Ya existe un tipo de item de factura con ese nombre");
    }
}
