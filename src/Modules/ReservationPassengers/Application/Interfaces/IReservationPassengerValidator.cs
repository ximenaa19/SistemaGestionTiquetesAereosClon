// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Application\Interfaces\IReservationPassengerValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationPassengers.Application.Interfaces;

public interface IReservationPassengerValidator
{
    Task ValidateReservationFlightExistsAsync(ReservationPassengerReservationFlightId reservationFlightId);
    Task ValidatePassengerExistsAsync(ReservationPassengerPassengerId passengerId);
    Task ValidateUniquePairAsync(ReservationPassengerReservationFlightId reservationFlightId, ReservationPassengerPassengerId passengerId, ReservationPassengerId? currentId = null);
    Task ValidateReservationAllowsChangesAsync(ReservationPassengerReservationFlightId reservationFlightId);
    Task ValidateFlightHasAvailabilityAsync(ReservationPassengerReservationFlightId reservationFlightId, int seatsToConsume);
}

