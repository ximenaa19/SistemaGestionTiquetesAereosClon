// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Domain\ValueObject\PaymentReservationId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Payments.Domain.ValueObject
{
    public sealed record PaymentReservationId
    {
        public int Value { get; }

        private PaymentReservationId(int value)
        {
            Value = value;
        }

        public static PaymentReservationId Create(int value)
        {
            if (value <= 0)
                throw new ArgumentException("El reserva_id no puede ser menor a 1");

            return new PaymentReservationId(value);
        }
    }
}

