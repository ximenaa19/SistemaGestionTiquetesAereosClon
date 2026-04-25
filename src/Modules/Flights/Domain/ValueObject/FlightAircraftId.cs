// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Domain\ValueObject\FlightAircraftId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Flights.Domain.ValueObject
{
    public sealed record FlightAircraftId
    {
        public int Value { get; }

        private FlightAircraftId(int value)
        {
            Value = value;
        }

        public static FlightAircraftId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El aeronave_id no puede ser menor a 1");

            return new FlightAircraftId(value);
        }
    }
}

