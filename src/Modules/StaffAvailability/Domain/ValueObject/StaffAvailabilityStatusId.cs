// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Domain\ValueObject\StaffAvailabilityStatusId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.StaffAvailability.Domain.ValueObject
{
    public sealed record StaffAvailabilityStatusId
    {
        public int Value { get; }

        private StaffAvailabilityStatusId(int value)
        {
            Value = value;
        }

        public static StaffAvailabilityStatusId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El estado_disponibilidad_id no puede ser menor a 1");

            return new StaffAvailabilityStatusId(value);
        }
    }
}

