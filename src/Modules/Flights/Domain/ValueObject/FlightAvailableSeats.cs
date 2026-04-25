// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Domain\ValueObject\FlightAvailableSeats.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Flights.Domain.ValueObject
{
    public sealed record FlightAvailableSeats
    {
        public int Value { get; }

        private FlightAvailableSeats(int value)
        {
            Value = value;
        }

        public static FlightAvailableSeats Create(int value)
        {
            if (value < 0)
                throw new ArgumentException("Los asientos_disponibles no pueden ser negativos");

            return new FlightAvailableSeats(value);
        }
    }
}

