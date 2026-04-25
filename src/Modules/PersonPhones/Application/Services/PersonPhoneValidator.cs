// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonPhones\Application\Services\PersonPhoneValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Domain.ValueObject;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PersonPhones.Application.Interfaces;
using GestionAerolineas.src.Modules.PersonPhones.Domain.Repositories;
using GestionAerolineas.src.Modules.PersonPhones.Domain.ValueObject;
using GestionAerolineas.src.Modules.PhoneCodes.Domain.ValueObject;
using GestionAerolineas.src.Modules.PhoneCodes.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.PersonPhones.Application.Services;

public class PersonPhoneValidator : IPersonPhoneValidator
{
    private readonly IPersonPhoneRepository _repository;
    private readonly PersonRepository _personRepository;
    private readonly PhoneCodeRepository _phoneCodeRepository;

    public PersonPhoneValidator(
        IPersonPhoneRepository repository,
        PersonRepository personRepository,
        PhoneCodeRepository phoneCodeRepository)
    {
        _repository = repository;
        _personRepository = personRepository;
        _phoneCodeRepository = phoneCodeRepository;
    }

    public async Task ValidatePersonExistsAsync(PersonPhonePersonId personId)
    {
        var exists = await _personRepository.ExistsAsync(PersonId.Create(personId.Value));
        if (!exists)
            throw new Exception("La persona no existe");
    }

    public async Task ValidatePhoneCodeExistsAsync(PersonPhoneCodeId phoneCodeId)
    {
        var exists = await _phoneCodeRepository.ExistsAsync(PhoneCodeId.Create(phoneCodeId.Value));
        if (!exists)
            throw new Exception("El codigo de telefono no existe");
    }

    public async Task ValidateUniquePhoneForPersonAsync(
        PersonPhonePersonId personId,
        PersonPhoneCodeId phoneCodeId,
        PersonPhoneNumber phoneNumber,
        PersonPhoneId? currentId = null)
    {
        var normalizedNumber = PersonPhoneNumber.Normalize(phoneNumber.Value);
        var exists = await _repository.ExistsByNormalizedPhoneForPersonAsync(
            personId.Value,
            phoneCodeId.Value,
            normalizedNumber,
            currentId?.Value);

        if (exists)
            throw new Exception("Ya existe ese telefono para esta persona");
    }

    public async Task ValidatePrimaryPhoneAsync(PersonPhonePersonId personId, PersonPhoneIsPrimary isPrimary, PersonPhoneId? currentId = null)
    {
        if (!isPrimary.Value)
            return;

        var exists = await _repository.ExistsPrimaryForPersonAsync(personId.Value, currentId?.Value);
        if (exists)
            throw new Exception("Esta persona ya tiene un telefono principal");
    }
}

