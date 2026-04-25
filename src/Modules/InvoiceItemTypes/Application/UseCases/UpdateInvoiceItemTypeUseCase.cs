// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\Application\UseCases\UpdateInvoiceItemTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItemTypes.Application.UseCases;

public class UpdateInvoiceItemTypeUseCase
{
    private readonly IInvoiceItemTypeRepository _repository;
    private readonly IInvoiceItemTypeValidator _validator;

    public UpdateInvoiceItemTypeUseCase(
        IInvoiceItemTypeRepository repository,
        IInvoiceItemTypeValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name)
    {
        var idVO = InvoiceItemTypeId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing is null)
            throw new Exception("El tipo de item de factura no existe");

        var nameVO = InvoiceItemTypeName.Create(name);

        await _validator.ValidateNameAsync(nameVO, idVO);

        var updated = InvoiceItemType.Create(idVO, nameVO);

        await _repository.UpdateAsync(updated);
    }
}
