// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\Application\UseCases\GetAllCustomersUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Domain.Aggregate;
using GestionAerolineas.src.Modules.Customers.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Customers.Application.UseCases;

public class GetAllCustomersUseCase
{
    private readonly ICustomerRepository _repository;

    public GetAllCustomersUseCase(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Customer>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}
