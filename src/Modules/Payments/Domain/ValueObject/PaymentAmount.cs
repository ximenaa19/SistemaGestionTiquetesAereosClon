// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Domain\ValueObject\PaymentAmount.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Payments.Domain.ValueObject
{
    public sealed record PaymentAmount
    {
        public decimal Value { get; }

        private PaymentAmount(decimal value)
        {
            Value = value;
        }

        public static PaymentAmount Create(decimal value)
        {
            if (value < 0)
                throw new ArgumentException("El monto no puede ser negativo");

            return new PaymentAmount(value);
        }
    }
}

