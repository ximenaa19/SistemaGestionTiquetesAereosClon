// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\Domain\ValueObject\ReservationFlightReservationId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject
{
    public sealed record ReservationFlightReservationId
    {
        public int Value { get; }

        private ReservationFlightReservationId(int value)
        {
            Value = value;
        }

        public static ReservationFlightReservationId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El reserva_id no puede ser menor a 1");

            return new ReservationFlightReservationId(value);
        }
    }
}

