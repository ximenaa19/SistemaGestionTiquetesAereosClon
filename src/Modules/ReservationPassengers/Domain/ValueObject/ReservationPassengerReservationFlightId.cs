// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Domain\ValueObject\ReservationPassengerReservationFlightId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject
{
    public sealed record ReservationPassengerReservationFlightId
    {
        public int Value { get; }

        private ReservationPassengerReservationFlightId(int value)
        {
            Value = value;
        }

        public static ReservationPassengerReservationFlightId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El reserva_vuelo_id no puede ser menor a 1");

            return new ReservationPassengerReservationFlightId(value);
        }
    }
}

