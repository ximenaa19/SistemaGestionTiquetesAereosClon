// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\CustomerModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Application.Interfaces;
using GestionAerolineas.src.Modules.Customers.Application.Services;
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.Customers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Customers.UI;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Customers;

public static class CustomerModule
{
    public static CustomerMenu Build(AppDbContext context)
    {
        var repository = new CustomerRepository(context);

        var personRepository = new PersonRepository(context);
        ICustomerValidator validator = new CustomerValidator(repository, personRepository);

        var create = new CreateCustomerUseCase(repository, validator);
        var getAll = new GetAllCustomersUseCase(repository);
        var getById = new GetCustomerByIdUseCase(repository);
        var getByPersonId = new GetCustomerByPersonIdUseCase(repository);
        var getByPersonName = new GetCustomerByPersonNameUseCase(repository);
        var update = new UpdateCustomerUseCase(repository, validator);
        var delete = new DeleteCustomerUseCase(repository);

        var getAllPeople = new GetAllPeopleUseCase(personRepository);

        return new CustomerMenu(create, getAll, getById, getByPersonId, getByPersonName, update, delete, getAllPeople);
    }

    public static AdminCreateCustomerFlow BuildAdminCreateFlow(AppDbContext context)
    {
        var repository = new CustomerRepository(context);
        var personRepository = new PersonRepository(context);

        ICustomerValidator validator = new CustomerValidator(repository, personRepository);

        var create = new CreateCustomerUseCase(repository, validator);
        var getAllPeople = new GetAllPeopleUseCase(personRepository);

        return new AdminCreateCustomerFlow(create, getAllPeople);
    }
}
