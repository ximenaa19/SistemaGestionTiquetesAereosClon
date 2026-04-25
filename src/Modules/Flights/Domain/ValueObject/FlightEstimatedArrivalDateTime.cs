// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Domain\ValueObject\FlightEstimatedArrivalDateTime.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Flights.Domain.ValueObject
{
    public sealed record FlightEstimatedArrivalDateTime
    {
        public DateTime Value { get; }

        private FlightEstimatedArrivalDateTime(DateTime value)
        {
            Value = value;
        }

        public static FlightEstimatedArrivalDateTime Create(DateTime value)
        {
            if (value == default)
                throw new ArgumentException("La fecha_llegada_estimada es invalida");

            return new FlightEstimatedArrivalDateTime(value);
        }
    }
}

