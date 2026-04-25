// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Domain\Aggregate\Payment.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Payments.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Payments.Domain.Aggregate
{
    public class Payment
    {
        public PaymentId Id { get; private set; }
        public PaymentReservationId ReservationId { get; private set; }
        public PaymentAmount Amount { get; private set; }
        public PaymentPaidAt PaidAt { get; private set; }
        public PaymentStateId StateId { get; private set; }
        public PaymentMethodId MethodId { get; private set; }
        public PaymentCreatedAt CreatedAt { get; private set; }
        public PaymentUpdatedAt UpdatedAt { get; private set; }

        private Payment(
            PaymentId id,
            PaymentReservationId reservationId,
            PaymentAmount amount,
            PaymentPaidAt paidAt,
            PaymentStateId stateId,
            PaymentMethodId methodId,
            PaymentCreatedAt createdAt,
            PaymentUpdatedAt updatedAt)
        {
            Id = id;
            ReservationId = reservationId;
            Amount = amount;
            PaidAt = paidAt;
            StateId = stateId;
            MethodId = methodId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static Payment Create(
            PaymentId id,
            PaymentReservationId reservationId,
            PaymentAmount amount,
            PaymentPaidAt paidAt,
            PaymentStateId stateId,
            PaymentMethodId methodId,
            PaymentCreatedAt createdAt,
            PaymentUpdatedAt updatedAt)
        {
            return new Payment(id, reservationId, amount, paidAt, stateId, methodId, createdAt, updatedAt);
        }

        public static Payment CreateNew(
            PaymentReservationId reservationId,
            PaymentAmount amount,
            PaymentPaidAt paidAt,
            PaymentStateId stateId,
            PaymentMethodId methodId)
        {
            return new Payment(
                PaymentId.CreateEmpty(),
                reservationId,
                amount,
                paidAt,
                stateId,
                methodId,
                PaymentCreatedAt.CreateOptional(null),
                PaymentUpdatedAt.CreateOptional(null));
        }
    }
}

