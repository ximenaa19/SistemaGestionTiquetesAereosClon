// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonEmails\PersonEmailModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Application.UseCases;
using GestionAerolineas.src.Modules.EmailDomains.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PersonEmails.Application.Interfaces;
using GestionAerolineas.src.Modules.PersonEmails.Application.Services;
using GestionAerolineas.src.Modules.PersonEmails.Application.UseCases;
using GestionAerolineas.src.Modules.PersonEmails.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PersonEmails.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.PersonEmails;

public static class PersonEmailModule
{
    public static PersonEmailMenu Build(AppDbContext context)
    {
        var repository = new PersonEmailRepository(context);

        var personRepository = new PersonRepository(context);
        var emailDomainRepository = new EmailDomainRepository(context);
        IPersonEmailValidator validator = new PersonEmailValidator(repository, personRepository, emailDomainRepository);

        var create = new CreatePersonEmailUseCase(repository, validator);
        var getAll = new GetAllPersonEmailsUseCase(repository);
        var getById = new GetPersonEmailByIdUseCase(repository);
        var getByAddress = new GetPersonEmailByPersonAndEmailUseCase(repository);
        var update = new UpdatePersonEmailUseCase(repository, validator);
        var delete = new DeletePersonEmailUseCase(repository);

        var getAllPeople = new GetAllPeopleUseCase(personRepository);
        var getAllEmailDomains = new GetAllEmailDomainsUseCase(emailDomainRepository);

        return new PersonEmailMenu(
            create,
            getAll,
            getById,
            getByAddress,
            update,
            delete,
            getAllPeople,
            getAllEmailDomains
        );
    }
}

