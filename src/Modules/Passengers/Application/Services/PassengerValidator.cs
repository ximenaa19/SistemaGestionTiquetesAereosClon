// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\Application\Services\PassengerValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PassengerTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Passengers.Application.Interfaces;
using GestionAerolineas.src.Modules.Passengers.Domain.Repositories;
using GestionAerolineas.src.Modules.Passengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.People.Domain.ValueObject;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using PassengerTypeEntityId = GestionAerolineas.src.Modules.PassengerTypes.Domain.ValueObject.PassengerTypeId;

namespace GestionAerolineas.src.Modules.Passengers.Application.Services;

public class PassengerValidator : IPassengerValidator
{
    private readonly IPassengerRepository _repository;
    private readonly PersonRepository _personRepository;
    private readonly PassengerTypeRepository _passengerTypeRepository;

    public PassengerValidator(
        IPassengerRepository repository,
        PersonRepository personRepository,
        PassengerTypeRepository passengerTypeRepository)
    {
        _repository = repository;
        _personRepository = personRepository;
        _passengerTypeRepository = passengerTypeRepository;
    }

    public async Task ValidatePersonExistsAsync(PassengerPersonId personId)
    {
        var exists = await _personRepository.ExistsAsync(PersonId.Create(personId.Value));
        if (!exists)
            throw new Exception("La persona no existe");
    }

    public async Task ValidatePassengerTypeExistsAsync(PassengerTypeId passengerTypeId)
    {
        var exists = await _passengerTypeRepository.ExistsAsync(PassengerTypeEntityId.Create(passengerTypeId.Value));
        if (!exists)
            throw new Exception("El tipo de pasajero no existe");
    }

    public async Task ValidatePersonIsUniqueAsync(PassengerPersonId personId, PassengerId? currentId = null)
    {
        var exists = await _repository.ExistsByPersonIdAsync(personId, currentId);
        if (exists)
            throw new Exception("Ya existe un passenger para esta persona");
    }
}
