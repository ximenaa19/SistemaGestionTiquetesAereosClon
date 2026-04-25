// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\Domain\ValueObject\FlightAssignmentFlightRoleId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.FlightAssignments.Domain.ValueObject
{
    public sealed record FlightAssignmentFlightRoleId
    {
        public int Value { get; }

        private FlightAssignmentFlightRoleId(int value)
        {
            Value = value;
        }

        public static FlightAssignmentFlightRoleId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El rol_vuelo_id no puede ser menor a 1");

            return new FlightAssignmentFlightRoleId(value);
        }
    }
}

