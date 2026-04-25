// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonPhones\Application\UseCases\GetPersonPhoneByPersonAndPhoneUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonPhones.Domain.Aggregate;
using GestionAerolineas.src.Modules.PersonPhones.Domain.Repositories;
using GestionAerolineas.src.Modules.PersonPhones.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PersonPhones.Application.UseCases;

public class GetPersonPhoneByPersonAndPhoneUseCase
{
    private readonly IPersonPhoneRepository _repository;

    public GetPersonPhoneByPersonAndPhoneUseCase(IPersonPhoneRepository repository)
    {
        _repository = repository;
    }

    public Task<PersonPhone?> ExecuteAsync(int personId, int phoneCodeId, string phoneNumber)
    {
        return _repository.GetByPersonAndCodeAndNumberAsync(
            PersonPhonePersonId.Create(personId),
            PersonPhoneCodeId.Create(phoneCodeId),
            PersonPhoneNumber.Create(phoneNumber)
        );
    }
}

