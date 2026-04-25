// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonPhones\Domain\ValueObject\PersonPhoneIsPrimary.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.PersonPhones.Domain.ValueObject
{
    public sealed record PersonPhoneIsPrimary
    {
        public bool Value { get; }

        private PersonPhoneIsPrimary(bool value)
        {
            Value = value;
        }

        public static PersonPhoneIsPrimary Create(bool value)
        {
            return new PersonPhoneIsPrimary(value);
        }
    }
}

