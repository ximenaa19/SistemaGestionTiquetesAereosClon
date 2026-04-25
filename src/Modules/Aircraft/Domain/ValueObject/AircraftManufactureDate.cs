// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\Domain\ValueObject\AircraftManufactureDate.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Aircraft.Domain.ValueObject
{
    public sealed record AircraftManufactureDate
    {
        public DateTime? Value { get; }

        private AircraftManufactureDate(DateTime? value)
        {
            Value = value;
        }

        public static AircraftManufactureDate Create(DateTime? value)
        {
            if (value.HasValue && value.Value.Date > DateTime.UtcNow.Date)
                throw new ArgumentException("La fecha de fabricacion no puede ser futura");

            return new AircraftManufactureDate(value?.Date);
        }
    }
}

