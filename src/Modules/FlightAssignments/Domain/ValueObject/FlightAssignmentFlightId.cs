// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\Domain\ValueObject\FlightAssignmentFlightId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.FlightAssignments.Domain.ValueObject
{
    public sealed record FlightAssignmentFlightId
    {
        public int Value { get; }

        private FlightAssignmentFlightId(int value)
        {
            Value = value;
        }

        public static FlightAssignmentFlightId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El vuelo_id no puede ser menor a 1");

            return new FlightAssignmentFlightId(value);
        }
    }
}

