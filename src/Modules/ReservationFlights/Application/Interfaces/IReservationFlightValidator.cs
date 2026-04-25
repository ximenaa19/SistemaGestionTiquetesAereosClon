// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\Application\Interfaces\IReservationFlightValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationFlights.Application.Interfaces;

public interface IReservationFlightValidator
{
    Task ValidateReservationExistsAsync(ReservationFlightReservationId reservationId);
    Task ValidateFlightExistsAsync(ReservationFlightFlightId flightId);
    Task ValidateUniquePairAsync(ReservationFlightReservationId reservationId, ReservationFlightFlightId flightId, ReservationFlightId? currentId = null);
    Task ValidateFlightNotInFinalStateAsync(ReservationFlightFlightId flightId);
    Task ValidateReservationAllowsChangesAsync(ReservationFlightReservationId reservationId);
    Task ValidateNoPassengersAsync(ReservationFlightId reservationFlightId);
}
