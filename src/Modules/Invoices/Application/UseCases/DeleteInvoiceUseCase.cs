// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Application\UseCases\DeleteInvoiceUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Invoices.Application.Interfaces;
using GestionAerolineas.src.Modules.Invoices.Domain.Repositories;
using GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Invoices.Application.UseCases;

public class DeleteInvoiceUseCase
{
    private readonly IInvoiceRepository _repository;
    private readonly IInvoiceValidator _validator;

    public DeleteInvoiceUseCase(IInvoiceRepository repository, IInvoiceValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id)
    {
        var idVO = InvoiceId.Create(id);
        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is null)
            throw new Exception("La factura no existe");

        await _validator.ValidateDeletableAsync(idVO);
        await _repository.DeleteAsync(existing);
    }
}

