// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Application\UseCases\GetInvoiceByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Invoices.Domain.Aggregate;
using GestionAerolineas.src.Modules.Invoices.Domain.Repositories;
using GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Invoices.Application.UseCases;

public class GetInvoiceByIdUseCase
{
    private readonly IInvoiceRepository _repository;

    public GetInvoiceByIdUseCase(IInvoiceRepository repository)
    {
        _repository = repository;
    }

    public Task<Invoice?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(InvoiceId.Create(id));
    }
}

