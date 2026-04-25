// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\Application\UseCases\GetInvoiceItemTypeByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItemTypes.Application.UseCases;

public class GetInvoiceItemTypeByNameUseCase
{
    private readonly IInvoiceItemTypeRepository _repository;

    public GetInvoiceItemTypeByNameUseCase(IInvoiceItemTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<InvoiceItemType?> ExecuteAsync(string name)
    {
        var nameVO = InvoiceItemTypeName.Create(name);
        return await _repository.GetByNameAsync(nameVO);
    }
}
