// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Domain\ValueObject\StaffHireDate.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Staff.Domain.ValueObject
{
    public sealed record StaffHireDate
    {
        public DateTime Value { get; }

        private StaffHireDate(DateTime value)
        {
            Value = value.Date;
        }

        public static StaffHireDate Create(DateTime value)
        {
            var date = value.Date;
            if (date > DateTime.Today)
                throw new ArgumentException("La fecha_ingreso no puede ser futura");

            return new StaffHireDate(date);
        }
    }
}

