// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Domain\ValueObject\FlightTotalCapacity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Flights.Domain.ValueObject
{
    public sealed record FlightTotalCapacity
    {
        public int Value { get; }

        private FlightTotalCapacity(int value)
        {
            Value = value;
        }

        public static FlightTotalCapacity Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("La capacidad_total debe ser mayor a 0");

            return new FlightTotalCapacity(value);
        }
    }
}

