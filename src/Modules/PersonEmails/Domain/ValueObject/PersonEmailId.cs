// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonEmails\Domain\ValueObject\PersonEmailId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.PersonEmails.Domain.ValueObject
{
    public sealed record PersonEmailId
    {
        public int Value { get; }

        private PersonEmailId(int value)
        {
            Value = value;
        }

        public static PersonEmailId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El valor no puede ser menor a 1");

            return new PersonEmailId(value);
        }

        public static PersonEmailId CreateEmpty()
        {
            return new PersonEmailId(0);
        }
    }
}

