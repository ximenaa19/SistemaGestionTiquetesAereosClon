// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Domain\ValueObject\StaffAvailabilityObservation.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.StaffAvailability.Domain.ValueObject
{
    public sealed record StaffAvailabilityObservation
    {
        public string? Value { get; }

        private StaffAvailabilityObservation(string? value)
        {
            Value = value;
        }

        public static StaffAvailabilityObservation Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new StaffAvailabilityObservation((string?)null);

            var trimmed = value.Trim();
            if (trimmed.Length > 255)
                throw new ArgumentException("La observacion no puede tener mas de 255 caracteres");

            return new StaffAvailabilityObservation(trimmed);
        }
    }
}
