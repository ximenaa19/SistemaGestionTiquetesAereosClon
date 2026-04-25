// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\Application\UseCases\CreateCustomerUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Application.Interfaces;
using GestionAerolineas.src.Modules.Customers.Domain.Aggregate;
using GestionAerolineas.src.Modules.Customers.Domain.Repositories;
using GestionAerolineas.src.Modules.Customers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Customers.Application.UseCases;

public class CreateCustomerUseCase
{
    private readonly ICustomerRepository _repository;
    private readonly ICustomerValidator _validator;

    public CreateCustomerUseCase(ICustomerRepository repository, ICustomerValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int personId)
    {
        var personVO = CustomerPersonId.Create(personId);

        await _validator.ValidatePersonExistsAsync(personVO);
        await _validator.ValidatePersonIsUniqueAsync(personVO);

        var entity = Customer.CreateNew(personVO);

        await _repository.AddAsync(entity);
    }
}
