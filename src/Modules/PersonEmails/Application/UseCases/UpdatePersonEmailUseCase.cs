// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonEmails\Application\UseCases\UpdatePersonEmailUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonEmails.Application.Interfaces;
using GestionAerolineas.src.Modules.PersonEmails.Domain.Aggregate;
using GestionAerolineas.src.Modules.PersonEmails.Domain.Repositories;
using GestionAerolineas.src.Modules.PersonEmails.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PersonEmails.Application.UseCases;

public class UpdatePersonEmailUseCase
{
    private readonly IPersonEmailRepository _repository;
    private readonly IPersonEmailValidator _validator;

    public UpdatePersonEmailUseCase(IPersonEmailRepository repository, IPersonEmailValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, int personId, string user, int emailDomainId, bool isPrimary)
    {
        var idVO = PersonEmailId.Create(id);
        var personIdVO = PersonEmailPersonId.Create(personId);
        var userVO = PersonEmailUser.Create(user);
        var domainIdVO = PersonEmailDomainId.Create(emailDomainId);
        var primaryVO = PersonEmailIsPrimary.Create(isPrimary);

        await _validator.ValidatePersonExistsAsync(personIdVO);
        await _validator.ValidateEmailDomainExistsAsync(domainIdVO);
        await _validator.ValidateUniqueEmailForPersonAsync(personIdVO, userVO, domainIdVO, idVO);
        await _validator.ValidatePrimaryEmailAsync(personIdVO, primaryVO, idVO);

        var entity = PersonEmail.Create(idVO, personIdVO, userVO, domainIdVO, primaryVO);
        await _repository.UpdateAsync(entity);
    }
}

