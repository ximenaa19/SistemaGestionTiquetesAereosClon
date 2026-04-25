// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Domain\ValueObject\StaffId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Staff.Domain.ValueObject
{
    public sealed record StaffId
    {
        public int Value { get; }

        private StaffId(int value)
        {
            Value = value;
        }

        public static StaffId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El id no puede ser menor a 1");

            return new StaffId(value);
        }

        public static StaffId CreateEmpty()
        {
            return new StaffId(0);
        }
    }
}

