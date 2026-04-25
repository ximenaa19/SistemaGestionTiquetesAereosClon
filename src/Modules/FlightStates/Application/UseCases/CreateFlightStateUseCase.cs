// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStates\Application\UseCases\CreateFlightStateUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStates.Application.Interfaces;
using GestionAerolineas.src.Modules.FlightStates.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightStates.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightStates.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightStates.Application.UseCases;

public class CreateFlightStateUseCase
{
    private readonly IFlightStateRepository _repository;
    private readonly IFlightStateValidator _validator;

    public CreateFlightStateUseCase(
        IFlightStateRepository repository,
        IFlightStateValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(string name)
    {
        var nameVO = FlightStateName.Create(name);

        await _validator.ValidateNameAsync(nameVO);

        var entity = FlightState.CreateNew(nameVO);

        await _repository.AddAsync(entity);
    }
}
