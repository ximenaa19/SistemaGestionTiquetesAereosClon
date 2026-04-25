// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Domain\ValueObject\StaffIsActive.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Staff.Domain.ValueObject
{
    public sealed record StaffIsActive
    {
        public bool Value { get; }

        private StaffIsActive(bool value)
        {
            Value = value;
        }

        public static StaffIsActive Create(bool value)
        {
            return new StaffIsActive(value);
        }
    }
}

