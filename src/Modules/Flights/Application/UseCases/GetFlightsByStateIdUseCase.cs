// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Application\UseCases\GetFlightsByStateIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.Repositories;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Flights.Application.UseCases;

public class GetFlightsByStateIdUseCase
{
    private readonly IFlightRepository _repository;

    public GetFlightsByStateIdUseCase(IFlightRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Flight>> ExecuteAsync(int stateId)
    {
        return _repository.GetByStateIdAsync(FlightStateId.Create(stateId));
    }
}

