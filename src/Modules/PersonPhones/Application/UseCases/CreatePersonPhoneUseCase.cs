// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonPhones\Application\UseCases\CreatePersonPhoneUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonPhones.Application.Interfaces;
using GestionAerolineas.src.Modules.PersonPhones.Domain.Aggregate;
using GestionAerolineas.src.Modules.PersonPhones.Domain.Repositories;
using GestionAerolineas.src.Modules.PersonPhones.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PersonPhones.Application.UseCases;

public class CreatePersonPhoneUseCase
{
    private readonly IPersonPhoneRepository _repository;
    private readonly IPersonPhoneValidator _validator;

    public CreatePersonPhoneUseCase(IPersonPhoneRepository repository, IPersonPhoneValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int personId, int phoneCodeId, string phoneNumber, bool isPrimary = false)
    {
        var personIdVO = PersonPhonePersonId.Create(personId);
        var phoneCodeIdVO = PersonPhoneCodeId.Create(phoneCodeId);
        var phoneNumberVO = PersonPhoneNumber.Create(phoneNumber);
        var primaryVO = PersonPhoneIsPrimary.Create(isPrimary);

        await _validator.ValidatePersonExistsAsync(personIdVO);
        await _validator.ValidatePhoneCodeExistsAsync(phoneCodeIdVO);
        await _validator.ValidateUniquePhoneForPersonAsync(personIdVO, phoneCodeIdVO, phoneNumberVO);
        await _validator.ValidatePrimaryPhoneAsync(personIdVO, primaryVO);

        var entity = PersonPhone.CreateNew(personIdVO, phoneCodeIdVO, phoneNumberVO, primaryVO);
        await _repository.AddAsync(entity);
    }
}

