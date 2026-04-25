// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\Application\UseCases\CreateInvoiceItemTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItemTypes.Application.UseCases;

public class CreateInvoiceItemTypeUseCase
{
    private readonly IInvoiceItemTypeRepository _repository;
    private readonly IInvoiceItemTypeValidator _validator;

    public CreateInvoiceItemTypeUseCase(
        IInvoiceItemTypeRepository repository,
        IInvoiceItemTypeValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(string name)
    {
        var nameVO = InvoiceItemTypeName.Create(name);

        await _validator.ValidateNameAsync(nameVO);

        var entity = InvoiceItemType.CreateNew(nameVO);

        await _repository.AddAsync(entity);
    }
}
