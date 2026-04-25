// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Domain\ValueObject\FlightSeatFlightId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.FlightSeats.Domain.ValueObject
{
    public sealed record FlightSeatFlightId
    {
        public int Value { get; }

        private FlightSeatFlightId(int value)
        {
            Value = value;
        }

        public static FlightSeatFlightId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El vuelo_id no puede ser menor a 1");

            return new FlightSeatFlightId(value);
        }
    }
}

