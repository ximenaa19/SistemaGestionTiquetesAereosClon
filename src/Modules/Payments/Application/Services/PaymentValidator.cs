// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Application\Services\PaymentValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethods.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PaymentStates.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Payments.Application.Interfaces;
using GestionAerolineas.src.Modules.Payments.Domain.Repositories;
using GestionAerolineas.src.Modules.Payments.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.Payments.Application.Services;

public class PaymentValidator : IPaymentValidator
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ReservationRepository _reservationRepository;
    private readonly ReservationStatusRepository _reservationStatusRepository;
    private readonly PaymentStateRepository _paymentStateRepository;
    private readonly PaymentMethodRepository _paymentMethodRepository;

    public PaymentValidator(
        IPaymentRepository paymentRepository,
        ReservationRepository reservationRepository,
        ReservationStatusRepository reservationStatusRepository,
        PaymentStateRepository paymentStateRepository,
        PaymentMethodRepository paymentMethodRepository)
    {
        _paymentRepository = paymentRepository;
        _reservationRepository = reservationRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _paymentStateRepository = paymentStateRepository;
        _paymentMethodRepository = paymentMethodRepository;
    }

    public async Task ValidateReservationExistsAsync(PaymentReservationId reservationId)
    {
        var exists = await _reservationRepository.ExistsAsync(ReservationId.Create(reservationId.Value));
        if (!exists)
            throw new Exception("La reserva no existe");
    }

    public async Task ValidateReservationAllowsPaymentsAsync(PaymentReservationId reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationId.Value));
        if (reservation is null)
            throw new Exception("La reserva no existe");

        var status = await _reservationStatusRepository.GetByIdAsync(
            GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject.ReservationStatusId.Create(reservation.StatusId.Value));

        var name = (status?.Name.Value ?? string.Empty).Trim().ToUpperInvariant();
        if (name == "CANCELADA" || name == "VENCIDA")
            throw new Exception($"No se puede registrar pagos para una reserva '{status!.Name.Value}'");
    }

    public async Task ValidatePaymentStateExistsAsync(PaymentStateId stateId)
    {
        var exists = await _paymentStateRepository.ExistsAsync(
            GestionAerolineas.src.Modules.PaymentStates.Domain.ValueObject.PaymentStateId.Create(stateId.Value));

        if (!exists)
            throw new Exception("El estado de pago no existe");
    }

    public async Task ValidatePaymentMethodExistsAsync(PaymentMethodId methodId)
    {
        var exists = await _paymentMethodRepository.ExistsAsync(
            GestionAerolineas.src.Modules.PaymentMethods.Domain.ValueObject.PaymentMethodId.Create(methodId.Value));

        if (!exists)
            throw new Exception("El metodo de pago no existe");
    }

    public async Task ValidateNotOverpayAsync(
        PaymentReservationId reservationId,
        PaymentAmount amount,
        PaymentStateId stateId,
        PaymentId? excludingId = null)
    {
        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationId.Value));
        if (reservation is null)
            throw new Exception("La reserva no existe");

        var state = await _paymentStateRepository.GetByIdAsync(
            GestionAerolineas.src.Modules.PaymentStates.Domain.ValueObject.PaymentStateId.Create(stateId.Value));

        var stateName = (state?.Name.Value ?? string.Empty).Trim().ToUpperInvariant();
        if (stateName != "PAGADO")
            return;

        var paidStateId = state!.Id.Value;
        var alreadyPaid = await _paymentRepository.SumPaidAmountByReservationIdAsync(
            reservationId.Value,
            paidStateId,
            excludingId?.Value);

        if (alreadyPaid + amount.Value > reservation.TotalAmount.Value)
            throw new Exception("El monto excede el total de la reserva");
    }

    public async Task ValidateDeletableAsync(PaymentId paymentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment is null)
            return;

        var state = await _paymentStateRepository.GetByIdAsync(
            GestionAerolineas.src.Modules.PaymentStates.Domain.ValueObject.PaymentStateId.Create(payment.StateId.Value));

        var name = (state?.Name.Value ?? string.Empty).Trim().ToUpperInvariant();
        if (name == "PAGADO")
            throw new Exception("No se puede eliminar un pago en estado 'Pagado'");
    }
}

