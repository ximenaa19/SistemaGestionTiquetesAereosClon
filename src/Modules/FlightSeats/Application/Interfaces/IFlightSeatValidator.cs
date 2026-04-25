// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Application\Interfaces\IFlightSeatValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightSeats.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightSeats.Application.Interfaces;

public interface IFlightSeatValidator
{
    Task ValidateFlightExistsAsync(FlightSeatFlightId flightId);
    Task ValidateCabinTypeExistsAsync(FlightSeatCabinTypeId cabinTypeId);
    Task ValidateLocationTypeExistsAsync(FlightSeatLocationTypeId locationTypeId);
    Task ValidateUniqueSeatCodeInFlightAsync(FlightSeatFlightId flightId, FlightSeatCode code, FlightSeatId? currentId = null);
    Task ValidateSeatCountWithinFlightCapacityAsync(FlightSeatFlightId flightId, FlightSeatId? currentId = null);
}

