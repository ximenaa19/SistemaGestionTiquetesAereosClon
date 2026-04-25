// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Domain\ValueObject\PersonGender.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.People.Domain.ValueObject
{
    public sealed record PersonGender
    {
        public string? Value { get; }

        private PersonGender(string? value)
        {
            Value = value;
        }

        public static PersonGender Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new PersonGender((string?)null);

            var candidate = value.Trim().ToUpperInvariant();
            if (candidate is not ("M" or "F" or "N"))
                throw new ArgumentException("El genero debe ser M, F o N");

            return new PersonGender(candidate);
        }
    }
}
