// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Domain\ValueObject\PaymentPaidAt.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Payments.Domain.ValueObject
{
    public sealed record PaymentPaidAt
    {
        public DateTime Value { get; }

        private PaymentPaidAt(DateTime value)
        {
            Value = value;
        }

        public static PaymentPaidAt Create(DateTime value)
        {
            return new PaymentPaidAt(value);
        }
    }
}

