// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Domain\ValueObject\PersonBirthDate.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.People.Domain.ValueObject
{
    public sealed record PersonBirthDate
    {
        public DateTime? Value { get; }

        private PersonBirthDate(DateTime? value)
        {
            Value = value;
        }

        public static PersonBirthDate Create(DateTime? value)
        {
            if (value.HasValue && value.Value.Date > DateTime.UtcNow.Date)
                throw new ArgumentException("La fecha de nacimiento no puede ser futura");

            return new PersonBirthDate(value?.Date);
        }
    }
}

