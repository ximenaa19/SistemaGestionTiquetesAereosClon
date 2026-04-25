// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\Domain\ValueObject\FlightAssignmentStaffId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.FlightAssignments.Domain.ValueObject
{
    public sealed record FlightAssignmentStaffId
    {
        public int Value { get; }

        private FlightAssignmentStaffId(int value)
        {
            Value = value;
        }

        public static FlightAssignmentStaffId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El personal_id no puede ser menor a 1");

            return new FlightAssignmentStaffId(value);
        }
    }
}

