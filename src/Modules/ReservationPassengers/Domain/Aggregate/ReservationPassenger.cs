// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Domain\Aggregate\ReservationPassenger.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationPassengers.Domain.Aggregate;

public class ReservationPassenger
{
    public ReservationPassengerId Id { get; private set; }
    public ReservationPassengerReservationFlightId ReservationFlightId { get; private set; }
    public ReservationPassengerPassengerId PassengerId { get; private set; }

    private ReservationPassenger(
        ReservationPassengerId id,
        ReservationPassengerReservationFlightId reservationFlightId,
        ReservationPassengerPassengerId passengerId)
    {
        Id = id;
        ReservationFlightId = reservationFlightId;
        PassengerId = passengerId;
    }

    public static ReservationPassenger Create(
        ReservationPassengerId id,
        ReservationPassengerReservationFlightId reservationFlightId,
        ReservationPassengerPassengerId passengerId)
    {
        return new ReservationPassenger(id, reservationFlightId, passengerId);
    }

    public static ReservationPassenger CreateNew(
        ReservationPassengerReservationFlightId reservationFlightId,
        ReservationPassengerPassengerId passengerId)
    {
        return new ReservationPassenger(ReservationPassengerId.CreateEmpty(), reservationFlightId, passengerId);
    }
}

