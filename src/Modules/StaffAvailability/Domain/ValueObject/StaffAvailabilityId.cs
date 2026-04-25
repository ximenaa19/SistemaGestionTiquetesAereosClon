// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Domain\ValueObject\StaffAvailabilityId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.StaffAvailability.Domain.ValueObject
{
    public sealed record StaffAvailabilityId
    {
        public int Value { get; }

        private StaffAvailabilityId(int value)
        {
            Value = value;
        }

        public static StaffAvailabilityId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El id no puede ser menor a 1");

            return new StaffAvailabilityId(value);
        }

        public static StaffAvailabilityId CreateEmpty()
        {
            return new StaffAvailabilityId(0);
        }
    }
}

