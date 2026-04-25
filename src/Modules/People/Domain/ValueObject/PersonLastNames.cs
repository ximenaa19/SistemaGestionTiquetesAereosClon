// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Domain\ValueObject\PersonLastNames.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.People.Domain.ValueObject
{
    public sealed record PersonLastNames
    {
        public string Value { get; }

        private PersonLastNames(string value)
        {
            Value = value;
        }

        public static PersonLastNames Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("El valor no puede ser nulo ni vacio");
            }

            if (value.Length > 100)
            {
                throw new ArgumentException("El valor no puede tener mas de 100 caracteres");
            }

            return new PersonLastNames(value.Trim());
        }
    }
}

