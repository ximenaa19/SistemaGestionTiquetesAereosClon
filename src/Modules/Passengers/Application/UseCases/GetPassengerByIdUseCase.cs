// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\Application\UseCases\GetPassengerByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Passengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.Passengers.Domain.Repositories;
using GestionAerolineas.src.Modules.Passengers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Passengers.Application.UseCases;

public class GetPassengerByIdUseCase
{
    private readonly IPassengerRepository _repository;

    public GetPassengerByIdUseCase(IPassengerRepository repository)
    {
        _repository = repository;
    }

    public Task<Passenger?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(PassengerId.Create(id));
    }
}
