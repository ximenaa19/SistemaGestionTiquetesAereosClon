// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStatusTransitions\Application\Interfaces\IFlightStatusTransitionValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightStatusTransitions.Application.Interfaces;

public interface IFlightStatusTransitionValidator
{
    Task ValidatePairAsync(
        FlightStateOriginId originStateId,
        FlightStateDestinationId destinationStateId,
        FlightStatusTransitionId? currentId = null
    );
}

