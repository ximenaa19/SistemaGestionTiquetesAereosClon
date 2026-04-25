// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonEmails\Domain\ValueObject\PersonEmailIsPrimary.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.PersonEmails.Domain.ValueObject
{
    public sealed record PersonEmailIsPrimary
    {
        public bool Value { get; }

        private PersonEmailIsPrimary(bool value)
        {
            Value = value;
        }

        public static PersonEmailIsPrimary Create(bool value)
        {
            return new PersonEmailIsPrimary(value);
        }
    }
}

