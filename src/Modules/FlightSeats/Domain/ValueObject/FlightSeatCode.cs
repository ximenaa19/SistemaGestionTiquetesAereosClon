// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Domain\ValueObject\FlightSeatCode.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Text.RegularExpressions;

namespace GestionAerolineas.src.Modules.FlightSeats.Domain.ValueObject
{
    public sealed partial record FlightSeatCode
    {
        public string Value { get; }

        private FlightSeatCode(string value)
        {
            Value = value;
        }

        public static FlightSeatCode Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El codigo_asiento no puede ser nulo ni vacio");

            var normalized = Normalize(value);
            if (normalized.Length > 5)
                throw new ArgumentException("El codigo_asiento no puede tener mas de 5 caracteres");

            if (!SeatCodeRegex().IsMatch(normalized))
                throw new ArgumentException("El codigo_asiento debe tener formato tipo '12A'");

            return new FlightSeatCode(normalized);
        }

        public static string Normalize(string value)
        {
            return (value ?? string.Empty).Trim().ToUpperInvariant();
        }

        [GeneratedRegex(@"^\d{1,4}[A-Z]$")]
        private static partial Regex SeatCodeRegex();
    }
}

