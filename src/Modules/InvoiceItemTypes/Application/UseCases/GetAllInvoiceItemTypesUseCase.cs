// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\Application\UseCases\GetAllInvoiceItemTypesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Repositories;

namespace GestionAerolineas.src.Modules.InvoiceItemTypes.Application.UseCases;

public class GetAllInvoiceItemTypesUseCase
{
    private readonly IInvoiceItemTypeRepository _repository;

    public GetAllInvoiceItemTypesUseCase(IInvoiceItemTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<InvoiceItemType>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
