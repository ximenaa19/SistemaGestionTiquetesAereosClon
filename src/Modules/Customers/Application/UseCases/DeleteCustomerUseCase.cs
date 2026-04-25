// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\Application\UseCases\DeleteCustomerUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Domain.Repositories;
using GestionAerolineas.src.Modules.Customers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Customers.Application.UseCases;

public class DeleteCustomerUseCase
{
    private readonly ICustomerRepository _repository;

    public DeleteCustomerUseCase(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(CustomerId.Create(id));
        if (entity is null)
            return;

        await _repository.DeleteAsync(entity);
    }
}
