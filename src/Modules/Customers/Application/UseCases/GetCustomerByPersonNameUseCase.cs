// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\Application\UseCases\GetCustomerByPersonNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Domain.Aggregate;
using GestionAerolineas.src.Modules.Customers.Domain.Repositories;
using GestionAerolineas.src.Modules.Customers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Customers.Application.UseCases;

public class GetCustomerByPersonNameUseCase
{
    private readonly ICustomerRepository _repository;

    public GetCustomerByPersonNameUseCase(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public Task<Customer?> ExecuteAsync(string personName)
    {
        return _repository.GetByPersonNameAsync(CustomerPersonName.Create(personName));
    }
}
