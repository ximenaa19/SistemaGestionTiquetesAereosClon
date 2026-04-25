// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Domain\ValueObject\FlightAirlineId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Flights.Domain.ValueObject
{
    public sealed record FlightAirlineId
    {
        public int Value { get; }

        private FlightAirlineId(int value)
        {
            Value = value;
        }

        public static FlightAirlineId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El aerolinea_id no puede ser menor a 1");

            return new FlightAirlineId(value);
        }
    }
}

