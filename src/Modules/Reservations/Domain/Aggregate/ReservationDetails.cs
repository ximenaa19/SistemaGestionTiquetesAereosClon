// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Domain\Aggregate\ReservationDetails.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;

public sealed class ReservationDetails
{
    public Reservation Reservation { get; }
    public IReadOnlyList<ReservationFlight> ReservationFlights { get; }
    public IReadOnlyList<ReservationPassenger> ReservationPassengers { get; }

    private ReservationDetails(
        Reservation reservation,
        IReadOnlyList<ReservationFlight> reservationFlights,
        IReadOnlyList<ReservationPassenger> reservationPassengers)
    {
        Reservation = reservation;
        ReservationFlights = reservationFlights;
        ReservationPassengers = reservationPassengers;
    }

    public static ReservationDetails Create(
        Reservation reservation,
        IEnumerable<ReservationFlight> reservationFlights,
        IEnumerable<ReservationPassenger> reservationPassengers)
    {
        return new ReservationDetails(
            reservation,
            reservationFlights.ToList(),
            reservationPassengers.ToList());
    }
}

