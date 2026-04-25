// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Application\UseCases\DeleteInvoiceItemUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Repositories;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItems.Application.UseCases;

public class DeleteInvoiceItemUseCase
{
    private readonly IInvoiceItemRepository _repository;

    public DeleteInvoiceItemUseCase(IInvoiceItemRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var idVO = InvoiceItemId.Create(id);
        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is null)
            throw new Exception("El item no existe");

        await _repository.DeleteAsync(existing);
    }
}

