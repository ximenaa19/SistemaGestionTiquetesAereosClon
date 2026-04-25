// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\Domain\ValueObject\ReservationFlightFlightId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject
{
    public sealed record ReservationFlightFlightId
    {
        public int Value { get; }

        private ReservationFlightFlightId(int value)
        {
            Value = value;
        }

        public static ReservationFlightFlightId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El vuelo_id no puede ser menor a 1");

            return new ReservationFlightFlightId(value);
        }
    }
}

