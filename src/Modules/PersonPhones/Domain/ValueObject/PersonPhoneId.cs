// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonPhones\Domain\ValueObject\PersonPhoneId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.PersonPhones.Domain.ValueObject
{
    public sealed record PersonPhoneId
    {
        public int Value { get; }

        private PersonPhoneId(int value)
        {
            Value = value;
        }

        public static PersonPhoneId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El valor no puede ser menor a 1");

            return new PersonPhoneId(value);
        }

        public static PersonPhoneId CreateEmpty()
        {
            return new PersonPhoneId(0);
        }
    }
}

