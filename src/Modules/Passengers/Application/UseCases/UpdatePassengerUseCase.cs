// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\Application\UseCases\UpdatePassengerUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Passengers.Application.Interfaces;
using GestionAerolineas.src.Modules.Passengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.Passengers.Domain.Repositories;
using GestionAerolineas.src.Modules.Passengers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Passengers.Application.UseCases;

public class UpdatePassengerUseCase
{
    private readonly IPassengerRepository _repository;
    private readonly IPassengerValidator _validator;

    public UpdatePassengerUseCase(IPassengerRepository repository, IPassengerValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, int personId, int passengerTypeId)
    {
        var idVO = PassengerId.Create(id);
        var existing = await _repository.GetByIdAsync(idVO);

        if (existing is null)
            throw new Exception("El passenger no existe");

        var personVO = PassengerPersonId.Create(personId);
        var passengerTypeVO = PassengerTypeId.Create(passengerTypeId);

        await _validator.ValidatePersonExistsAsync(personVO);
        await _validator.ValidatePassengerTypeExistsAsync(passengerTypeVO);
        await _validator.ValidatePersonIsUniqueAsync(personVO, idVO);

        var entity = Passenger.Create(idVO, personVO, passengerTypeVO);
        await _repository.UpdateAsync(entity);
    }
}
