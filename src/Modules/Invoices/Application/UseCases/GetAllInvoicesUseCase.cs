// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Application\UseCases\GetAllInvoicesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Invoices.Domain.Aggregate;
using GestionAerolineas.src.Modules.Invoices.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Invoices.Application.UseCases;

public class GetAllInvoicesUseCase
{
    private readonly IInvoiceRepository _repository;

    public GetAllInvoicesUseCase(IInvoiceRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Invoice>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

