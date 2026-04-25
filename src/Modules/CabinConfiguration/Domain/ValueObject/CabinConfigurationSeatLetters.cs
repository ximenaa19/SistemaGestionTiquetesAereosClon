// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Domain\ValueObject\CabinConfigurationSeatLetters.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.CabinConfiguration.Domain.ValueObject
{
    public sealed record CabinConfigurationSeatLetters
    {
        public string Value { get; }

        private CabinConfigurationSeatLetters(string value)
        {
            Value = value;
        }

        public static CabinConfigurationSeatLetters Create(string value, int seatsPerRow)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("letras_asientos no puede estar vacio");

            var normalized = value.Trim().ToUpperInvariant();

            if (normalized.Length > 10)
                throw new ArgumentException("letras_asientos no puede tener mas de 10 caracteres");

            if (normalized.Any(ch => ch < 'A' || ch > 'Z'))
                throw new ArgumentException("letras_asientos solo puede contener letras A-Z");

            if (normalized.Distinct().Count() != normalized.Length)
                throw new ArgumentException("letras_asientos no puede tener letras repetidas");

            if (normalized.Length != seatsPerRow)
                throw new ArgumentException("letras_asientos debe coincidir con asientos_por_fila");

            return new CabinConfigurationSeatLetters(normalized);
        }
    }
}

