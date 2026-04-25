// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStatusTransitions\Application\Services\FlightStatusTransitionValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStatusTransitions.Application.Interfaces;
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightStatusTransitions.Application.Services;

public class FlightStatusTransitionValidator : IFlightStatusTransitionValidator
{
    private readonly IFlightStatusTransitionRepository _repository;

    public FlightStatusTransitionValidator(IFlightStatusTransitionRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidatePairAsync(
        FlightStateOriginId originStateId,
        FlightStateDestinationId destinationStateId,
        FlightStatusTransitionId? currentId = null)
    {
        if (originStateId.Value == destinationStateId.Value)
            throw new Exception("El estado de origen y destino no pueden ser el mismo");

        var existing = await _repository.GetByPairAsync(originStateId, destinationStateId);

        if (existing is null)
            return;

        if (currentId != null && existing.Id.Value == currentId.Value)
            return;

        throw new Exception("Ya existe una transición con ese origen y destino");
    }
}

