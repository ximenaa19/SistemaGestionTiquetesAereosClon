// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Domain\ValueObject\StaffAvailabilityEndDateTime.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.StaffAvailability.Domain.ValueObject
{
    public sealed record StaffAvailabilityEndDateTime
    {
        public DateTime Value { get; }

        private StaffAvailabilityEndDateTime(DateTime value)
        {
            Value = value;
        }

        public static StaffAvailabilityEndDateTime Create(DateTime value)
        {
            if (value == default)
                throw new ArgumentException("La fecha_fin es invalida");

            return new StaffAvailabilityEndDateTime(value);
        }
    }
}

