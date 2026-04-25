// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStatusTransitions\Domain\Repositories\IFlightStatusTransitionRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.Repositories;

public interface IFlightStatusTransitionRepository
{
    Task<IEnumerable<FlightStatusTransition>> GetAllAsync();
    Task<FlightStatusTransition?> GetByIdAsync(FlightStatusTransitionId id);

    Task<FlightStatusTransition?> GetByPairAsync(
        FlightStateOriginId originStateId,
        FlightStateDestinationId destinationStateId
    );

    Task AddAsync(FlightStatusTransition transition);
    Task UpdateAsync(FlightStatusTransition transition);
    Task DeleteAsync(FlightStatusTransition transition);

    Task<bool> ExistsAsync(FlightStatusTransitionId id);
}

