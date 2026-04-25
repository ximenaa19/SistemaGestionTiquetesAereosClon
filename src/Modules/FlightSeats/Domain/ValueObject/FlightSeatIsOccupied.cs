// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Domain\ValueObject\FlightSeatIsOccupied.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.FlightSeats.Domain.ValueObject
{
    public sealed record FlightSeatIsOccupied
    {
        public bool Value { get; }

        private FlightSeatIsOccupied(bool value)
        {
            Value = value;
        }

        public static FlightSeatIsOccupied Create(bool value)
        {
            return new FlightSeatIsOccupied(value);
        }
    }
}

