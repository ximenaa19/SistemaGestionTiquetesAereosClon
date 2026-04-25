// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Application\Interfaces\IPaymentValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Payments.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Payments.Application.Interfaces;

public interface IPaymentValidator
{
    Task ValidateReservationExistsAsync(PaymentReservationId reservationId);
    Task ValidateReservationAllowsPaymentsAsync(PaymentReservationId reservationId);
    Task ValidatePaymentStateExistsAsync(PaymentStateId stateId);
    Task ValidatePaymentMethodExistsAsync(PaymentMethodId methodId);
    Task ValidateNotOverpayAsync(PaymentReservationId reservationId, PaymentAmount amount, PaymentStateId stateId, PaymentId? excludingId = null);
    Task ValidateDeletableAsync(PaymentId paymentId);
}

