// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Domain\ValueObject\StaffAirlineId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Staff.Domain.ValueObject
{
    public sealed record StaffAirlineId
    {
        public int? Value { get; }

        private StaffAirlineId(int? value)
        {
            Value = value;
        }

        public static StaffAirlineId Create(int? value)
        {
            if (!value.HasValue)
                return new StaffAirlineId((int?)null);

            if (value.Value <= 0)
                throw new ArgumentException("El aerolinea_id no puede ser menor a 1");

            return new StaffAirlineId(value.Value);
        }
    }
}

