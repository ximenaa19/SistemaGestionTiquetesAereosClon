// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStatusTransitions\Application\UseCases\GetFlightStatusTransitionByPairUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightStatusTransitions.Application.UseCases;

public class GetFlightStatusTransitionByPairUseCase
{
    private readonly IFlightStatusTransitionRepository _repository;

    public GetFlightStatusTransitionByPairUseCase(IFlightStatusTransitionRepository repository)
    {
        _repository = repository;
    }

    public Task<FlightStatusTransition?> ExecuteAsync(int originStateId, int destinationStateId)
    {
        var originVO = FlightStateOriginId.Create(originStateId);
        var destinationVO = FlightStateDestinationId.Create(destinationStateId);

        return _repository.GetByPairAsync(originVO, destinationVO);
    }
}

