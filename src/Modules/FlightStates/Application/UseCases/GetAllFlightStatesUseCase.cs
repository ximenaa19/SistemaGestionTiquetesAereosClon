// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStates\Application\UseCases\GetAllFlightStatesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStates.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightStates.Domain.Repositories;

namespace GestionAerolineas.src.Modules.FlightStates.Application.UseCases;

public class GetAllFlightStatesUseCase
{
    private readonly IFlightStateRepository _repository;

    public GetAllFlightStatesUseCase(IFlightStateRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<FlightState>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
