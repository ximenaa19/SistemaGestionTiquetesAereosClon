// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\Application\UseCases\CreatePassengerUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Passengers.Application.Interfaces;
using GestionAerolineas.src.Modules.Passengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.Passengers.Domain.Repositories;
using GestionAerolineas.src.Modules.Passengers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Passengers.Application.UseCases;

public class CreatePassengerUseCase
{
    private readonly IPassengerRepository _repository;
    private readonly IPassengerValidator _validator;

    public CreatePassengerUseCase(IPassengerRepository repository, IPassengerValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int personId, int passengerTypeId)
    {
        var personVO = PassengerPersonId.Create(personId);
        var passengerTypeVO = PassengerTypeId.Create(passengerTypeId);

        await _validator.ValidatePersonExistsAsync(personVO);
        await _validator.ValidatePassengerTypeExistsAsync(passengerTypeVO);
        await _validator.ValidatePersonIsUniqueAsync(personVO);

        var entity = Passenger.CreateNew(personVO, passengerTypeVO);

        await _repository.AddAsync(entity);
    }
}
