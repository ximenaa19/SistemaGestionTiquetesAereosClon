// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStates\Application\UseCases\GetFlightStateByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStates.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightStates.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightStates.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightStates.Application.UseCases;

public class GetFlightStateByNameUseCase
{
    private readonly IFlightStateRepository _repository;

    public GetFlightStateByNameUseCase(IFlightStateRepository repository)
    {
        _repository = repository;
    }

    public async Task<FlightState?> ExecuteAsync(string name)
    {
        var nameVO = FlightStateName.Create(name);
        return await _repository.GetByNameAsync(nameVO);
    }
}
