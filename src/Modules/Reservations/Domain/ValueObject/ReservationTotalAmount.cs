// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Domain\ValueObject\ReservationTotalAmount.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Reservations.Domain.ValueObject
{
    public sealed record ReservationTotalAmount
    {
        public decimal Value { get; }

        private ReservationTotalAmount(decimal value)
        {
            Value = value;
        }

        public static ReservationTotalAmount Create(decimal value)
        {
            if (value < 0)
                throw new ArgumentException("El valor_total no puede ser negativo");

            return new ReservationTotalAmount(value);
        }
    }
}

