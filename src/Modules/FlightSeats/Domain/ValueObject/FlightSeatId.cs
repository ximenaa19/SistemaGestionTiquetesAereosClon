// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Domain\ValueObject\FlightSeatId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.FlightSeats.Domain.ValueObject
{
    public sealed record FlightSeatId
    {
        public int Value { get; }

        private FlightSeatId(int value)
        {
            Value = value;
        }

        public static FlightSeatId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El id no puede ser menor a 1");

            return new FlightSeatId(value);
        }

        public static FlightSeatId CreateEmpty()
        {
            return new FlightSeatId(0);
        }
    }
}

