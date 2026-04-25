// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\Application\UseCases\GetPassengerByPersonNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Passengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.Passengers.Domain.Repositories;
using GestionAerolineas.src.Modules.Passengers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Passengers.Application.UseCases;

public class GetPassengerByPersonNameUseCase
{
    private readonly IPassengerRepository _repository;

    public GetPassengerByPersonNameUseCase(IPassengerRepository repository)
    {
        _repository = repository;
    }

    public Task<Passenger?> ExecuteAsync(string personName)
    {
        return _repository.GetByPersonNameAsync(PassengerPersonName.Create(personName));
    }
}
