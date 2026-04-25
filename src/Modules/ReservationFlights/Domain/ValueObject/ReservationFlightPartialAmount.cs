// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\Domain\ValueObject\ReservationFlightPartialAmount.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject
{
    public sealed record ReservationFlightPartialAmount
    {
        public decimal Value { get; }

        private ReservationFlightPartialAmount(decimal value)
        {
            Value = value;
        }

        public static ReservationFlightPartialAmount Create(decimal value)
        {
            if (value < 0)
                throw new ArgumentException("El valor_parcial no puede ser negativo");

            return new ReservationFlightPartialAmount(value);
        }
    }
}

