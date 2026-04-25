// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStates\Application\UseCases\DeleteFlightStateUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStates.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightStates.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightStates.Application.UseCases;

public class DeleteFlightStateUseCase
{
    private readonly IFlightStateRepository _repository;

    public DeleteFlightStateUseCase(IFlightStateRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var flightStateId = FlightStateId.Create(id);
        var flightState = await _repository.GetByIdAsync(flightStateId);

        if (flightState is null)
            throw new KeyNotFoundException($"FlightState con id '{flightStateId.Value}' no existe.");

        await _repository.DeleteAsync(flightState);
    }
}
