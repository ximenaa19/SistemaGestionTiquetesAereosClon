// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\Application\UseCases\GetInvoiceItemTypeByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItemTypes.Application.UseCases;

public class GetInvoiceItemTypeByIdUseCase
{
    private readonly IInvoiceItemTypeRepository _repository;

    public GetInvoiceItemTypeByIdUseCase(IInvoiceItemTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<InvoiceItemType?> ExecuteAsync(int id)
    {
        var idVO = InvoiceItemTypeId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}
