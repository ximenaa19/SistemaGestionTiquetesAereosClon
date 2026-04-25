// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonEmails\Application\Services\PersonEmailValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Infrastructure.Repository;
using GestionAerolineas.src.Modules.EmailDomains.Domain.ValueObject;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.Domain.ValueObject;
using GestionAerolineas.src.Modules.PersonEmails.Application.Interfaces;
using GestionAerolineas.src.Modules.PersonEmails.Domain.Repositories;
using GestionAerolineas.src.Modules.PersonEmails.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PersonEmails.Application.Services;

public class PersonEmailValidator : IPersonEmailValidator
{
    private readonly IPersonEmailRepository _repository;
    private readonly PersonRepository _personRepository;
    private readonly EmailDomainRepository _emailDomainRepository;

    public PersonEmailValidator(
        IPersonEmailRepository repository,
        PersonRepository personRepository,
        EmailDomainRepository emailDomainRepository)
    {
        _repository = repository;
        _personRepository = personRepository;
        _emailDomainRepository = emailDomainRepository;
    }

    public async Task ValidatePersonExistsAsync(PersonEmailPersonId personId)
    {
        var exists = await _personRepository.ExistsAsync(PersonId.Create(personId.Value));
        if (!exists)
            throw new Exception("La persona no existe");
    }

    public async Task ValidateEmailDomainExistsAsync(PersonEmailDomainId emailDomainId)
    {
        var exists = await _emailDomainRepository.ExistsAsync(EmailDomainId.Create(emailDomainId.Value));
        if (!exists)
            throw new Exception("El dominio de email no existe");
    }

    public async Task ValidateUniqueEmailForPersonAsync(
        PersonEmailPersonId personId,
        PersonEmailUser user,
        PersonEmailDomainId emailDomainId,
        PersonEmailId? currentId = null)
    {
        var normalizedUser = PersonEmailUser.Normalize(user.Value);
        var exists = await _repository.ExistsByNormalizedUserAndDomainForPersonAsync(
            personId.Value,
            normalizedUser,
            emailDomainId.Value,
            currentId?.Value);

        if (exists)
            throw new Exception("Ya existe ese email para esta persona");
    }

    public async Task ValidatePrimaryEmailAsync(PersonEmailPersonId personId, PersonEmailIsPrimary isPrimary, PersonEmailId? currentId = null)
    {
        if (!isPrimary.Value)
            return;

        var exists = await _repository.ExistsPrimaryForPersonAsync(personId.Value, currentId?.Value);
        if (exists)
            throw new Exception("Esta persona ya tiene un email principal");
    }
}

