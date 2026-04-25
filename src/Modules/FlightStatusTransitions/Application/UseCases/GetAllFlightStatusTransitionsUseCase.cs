// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStatusTransitions\Application\UseCases\GetAllFlightStatusTransitionsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.Repositories;

namespace GestionAerolineas.src.Modules.FlightStatusTransitions.Application.UseCases;

public class GetAllFlightStatusTransitionsUseCase
{
    private readonly IFlightStatusTransitionRepository _repository;

    public GetAllFlightStatusTransitionsUseCase(IFlightStatusTransitionRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<FlightStatusTransition>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

