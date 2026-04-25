// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonEmails\Application\UseCases\GetPersonEmailByPersonAndEmailUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonEmails.Domain.Aggregate;
using GestionAerolineas.src.Modules.PersonEmails.Domain.Repositories;
using GestionAerolineas.src.Modules.PersonEmails.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PersonEmails.Application.UseCases;

public class GetPersonEmailByPersonAndEmailUseCase
{
    private readonly IPersonEmailRepository _repository;

    public GetPersonEmailByPersonAndEmailUseCase(IPersonEmailRepository repository)
    {
        _repository = repository;
    }

    public Task<PersonEmail?> ExecuteAsync(int personId, string user, int emailDomainId)
    {
        return _repository.GetByPersonAndUserAndDomainAsync(
            PersonEmailPersonId.Create(personId),
            PersonEmailUser.Create(user),
            PersonEmailDomainId.Create(emailDomainId)
        );
    }
}

