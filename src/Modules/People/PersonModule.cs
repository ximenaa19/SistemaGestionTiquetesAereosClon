// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\PersonModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Application.UseCases;
using GestionAerolineas.src.Modules.Addresses.Application.Services;
using GestionAerolineas.src.Modules.Addresses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Cities.Application.UseCases;
using GestionAerolineas.src.Modules.Cities.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Customers.Application.Interfaces;
using GestionAerolineas.src.Modules.Customers.Application.Services;
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.Customers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.DocumentTypes.Application.UseCases;
using GestionAerolineas.src.Modules.DocumentTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.EmailDomains.Application.UseCases;
using GestionAerolineas.src.Modules.EmailDomains.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.Application.Interfaces;
using GestionAerolineas.src.Modules.People.Application.Services;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PersonEmails.Application.Interfaces;
using GestionAerolineas.src.Modules.PersonEmails.Application.Services;
using GestionAerolineas.src.Modules.PersonEmails.Application.UseCases;
using GestionAerolineas.src.Modules.PersonEmails.Infrastructure.Repository;
using GestionAerolineas.src.Modules.RoadTypes.Application.UseCases;
using GestionAerolineas.src.Modules.RoadTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Staff.Application.Interfaces;
using GestionAerolineas.src.Modules.Staff.Application.Services;
using GestionAerolineas.src.Modules.Staff.Application.UseCases;
using GestionAerolineas.src.Modules.Staff.Infrastructure.Repository;
using GestionAerolineas.src.Modules.StaffRoles.Application.UseCases;
using GestionAerolineas.src.Modules.StaffRoles.Infrastructure.Repository;
using GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Users.Application.Interfaces;
using GestionAerolineas.src.Modules.Users.Application.Services;
using GestionAerolineas.src.Modules.Users.Application.UseCases;
using GestionAerolineas.src.Modules.Users.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.People;

public static class PersonModule
{
    public static PersonMenu Build(AppDbContext context)
    {
        var repository = new PersonRepository(context);

        var documentTypeRepository = new DocumentTypeRepository(context);
        var addressRepository = new AddressRepository(context);

        IPersonValidator validator = new PersonValidator(repository, documentTypeRepository, addressRepository);

        var create = new CreatePersonUseCase(repository, validator);
        var getAll = new GetAllPeopleUseCase(repository);
        var getById = new GetPersonByIdUseCase(repository);
        var getByDocument = new GetPersonByDocumentUseCase(repository);
        var update = new UpdatePersonUseCase(repository, validator);
        var delete = new DeletePersonUseCase(repository);

        var getAllDocumentTypes = new GetAllDocumentTypesUseCase(documentTypeRepository);
        var getAllAddresses = new GetAllAddressesUseCase(addressRepository);

        return new PersonMenu(
            create,
            getAll,
            getById,
            getByDocument,
            update,
            delete,
            getAllDocumentTypes,
            getAllAddresses
        );
    }

    public static AdminCreatePersonFlow BuildAdminCreateFlow(AppDbContext context)
    {
        var personRepository = new PersonRepository(context);
        var documentTypeRepository = new DocumentTypeRepository(context);
        var addressRepository = new AddressRepository(context);
        var roadTypeRepository = new RoadTypeRepository(context);
        var cityRepository = new CityRepository(context);

        IPersonValidator personValidator = new PersonValidator(personRepository, documentTypeRepository, addressRepository);
        var createPerson = new CreatePersonUseCase(personRepository, personValidator);
        var getPersonByDocument = new GetPersonByDocumentUseCase(personRepository);
        var getAllDocumentTypes = new GetAllDocumentTypesUseCase(documentTypeRepository);
        var getAllAddresses = new GetAllAddressesUseCase(addressRepository);
        var createAddress = new CreateAddressUseCase(addressRepository, new AddressValidator(roadTypeRepository, cityRepository));
        var getAllRoadTypes = new GetAllRoadTypesUseCase(roadTypeRepository);
        var getAllCities = new GetAllCitiesUseCase(cityRepository);

        var personEmailRepository = new PersonEmailRepository(context);
        var emailDomainRepository = new EmailDomainRepository(context);
        IPersonEmailValidator personEmailValidator = new PersonEmailValidator(personEmailRepository, personRepository, emailDomainRepository);

        var createPersonEmail = new CreatePersonEmailUseCase(personEmailRepository, personEmailValidator);
        var getAllEmailDomains = new GetAllEmailDomainsUseCase(emailDomainRepository);

        var systemRoleRepository = new SystemRoleRepository(context);
        var getAllSystemRoles = new GetAllSystemRolesUseCase(systemRoleRepository);

        var userRepository = new UserRepository(context);
        IUserValidator userValidator = new UserValidator(userRepository, personRepository, systemRoleRepository);
        var createUser = new CreateUserUseCase(userRepository, userValidator);

        var customerRepository = new CustomerRepository(context);
        ICustomerValidator customerValidator = new CustomerValidator(customerRepository, personRepository);
        var createCustomer = new CreateCustomerUseCase(customerRepository, customerValidator);

        var staffRepository = new StaffRepository(context);
        var staffRoleRepository = new StaffRoleRepository(context);
        var airlineRepository = new AirlineRepository(context);
        var airportRepository = new AirportRepository(context);
        IStaffValidator staffValidator = new StaffValidator(staffRepository, personRepository, staffRoleRepository, airlineRepository, airportRepository);
        var createStaff = new CreateStaffUseCase(staffRepository, staffValidator);
        var getAllStaffRoles = new GetAllStaffRolesUseCase(staffRoleRepository);
        var getAllAirlines = new GetAllAirlinesUseCase(airlineRepository);
        var getAllAirports = new GetAllAirportsUseCase(airportRepository);

        return new AdminCreatePersonFlow(
            createPerson,
            getPersonByDocument,
            createPersonEmail,
            getAllDocumentTypes,
            getAllEmailDomains,
            createAddress,
            getAllAddresses,
            getAllRoadTypes,
            getAllCities,
            getAllSystemRoles,
            createUser,
            createCustomer,
            createStaff,
            getAllStaffRoles,
            getAllAirlines,
            getAllAirports
        );
    }

    public static AdminUpdatePersonFlow BuildAdminUpdateFlow(AppDbContext context)
    {
        var personRepository = new PersonRepository(context);
        var documentTypeRepository = new DocumentTypeRepository(context);
        var addressRepository = new AddressRepository(context);

        IPersonValidator validator = new PersonValidator(personRepository, documentTypeRepository, addressRepository);

        var getAll = new GetAllPeopleUseCase(personRepository);
        var getById = new GetPersonByIdUseCase(personRepository);
        var update = new UpdatePersonUseCase(personRepository, validator);
        var getAllDocumentTypes = new GetAllDocumentTypesUseCase(documentTypeRepository);
        var getAllAddresses = new GetAllAddressesUseCase(addressRepository);

        return new AdminUpdatePersonFlow(getAll, getById, update, getAllDocumentTypes, getAllAddresses);
    }

    public static AdminDeletePersonFlow BuildAdminDeleteFlow(AppDbContext context)
    {
        var personRepository = new PersonRepository(context);

        var getAll = new GetAllPeopleUseCase(personRepository);
        var getById = new GetPersonByIdUseCase(personRepository);
        var delete = new DeletePersonUseCase(personRepository);

        return new AdminDeletePersonFlow(getAll, getById, delete);
    }
}

