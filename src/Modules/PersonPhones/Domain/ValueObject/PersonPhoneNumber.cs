// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonPhones\Domain\ValueObject\PersonPhoneNumber.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Text.RegularExpressions;

namespace GestionAerolineas.src.Modules.PersonPhones.Domain.ValueObject
{
    public sealed record PersonPhoneNumber
    {
        private static readonly Regex ValidPattern = new("^[0-9+\\-() ]{1,20}$", RegexOptions.Compiled);

        public string Value { get; }

        private PersonPhoneNumber(string value)
        {
            Value = value;
        }

        public static PersonPhoneNumber Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El numero_telefono no puede estar vacio");

            var normalized = value.Trim();

            if (normalized.Length > 20)
                throw new ArgumentException("El numero_telefono no puede tener mas de 20 caracteres");

            if (!ValidPattern.IsMatch(normalized))
                throw new ArgumentException("El numero_telefono tiene un formato invalido");

            return new PersonPhoneNumber(normalized);
        }

        public static string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value.Trim().ToUpperInvariant();
        }
    }
}

