// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStatusTransitions\Domain\Aggregate\FlightStatusTransition.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.Aggregate;

public class FlightStatusTransition
{
    public FlightStatusTransitionId Id { get; private set; }
    public FlightStateOriginId OriginStateId { get; private set; }
    public FlightStateDestinationId DestinationStateId { get; private set; }

    private FlightStatusTransition(
        FlightStatusTransitionId id,
        FlightStateOriginId originStateId,
        FlightStateDestinationId destinationStateId)
    {
        Id = id;
        OriginStateId = originStateId;
        DestinationStateId = destinationStateId;
    }

    public static FlightStatusTransition Create(
        FlightStatusTransitionId id,
        FlightStateOriginId originStateId,
        FlightStateDestinationId destinationStateId)
    {
        return new FlightStatusTransition(id, originStateId, destinationStateId);
    }

    public static FlightStatusTransition CreateNew(
        FlightStateOriginId originStateId,
        FlightStateDestinationId destinationStateId)
    {
        return new FlightStatusTransition(
            FlightStatusTransitionId.CreateEmpty(),
            originStateId,
            destinationStateId
        );
    }
}

