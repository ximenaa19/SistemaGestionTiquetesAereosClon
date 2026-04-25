// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Domain\ValueObject\StaffAvailabilityStartDateTime.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.StaffAvailability.Domain.ValueObject
{
    public sealed record StaffAvailabilityStartDateTime
    {
        public DateTime Value { get; }

        private StaffAvailabilityStartDateTime(DateTime value)
        {
            Value = value;
        }

        public static StaffAvailabilityStartDateTime Create(DateTime value)
        {
            if (value == default)
                throw new ArgumentException("La fecha_inicio es invalida");

            return new StaffAvailabilityStartDateTime(value);
        }
    }
}

