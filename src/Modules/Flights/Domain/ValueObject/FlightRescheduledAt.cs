// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Domain\ValueObject\FlightRescheduledAt.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Flights.Domain.ValueObject
{
    public sealed record FlightRescheduledAt
    {
        public DateTime? Value { get; }

        private FlightRescheduledAt(DateTime? value)
        {
            Value = value;
        }

        public static FlightRescheduledAt Create(DateTime? value)
        {
            return new FlightRescheduledAt(value);
        }
    }
}

