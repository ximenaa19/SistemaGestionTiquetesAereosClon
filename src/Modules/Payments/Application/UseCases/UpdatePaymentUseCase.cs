// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Application\UseCases\UpdatePaymentUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Payments.Application.Interfaces;
using GestionAerolineas.src.Modules.Payments.Domain.Aggregate;
using GestionAerolineas.src.Modules.Payments.Domain.Repositories;
using GestionAerolineas.src.Modules.Payments.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PaymentStates.Infrastructure.Repository;
using PaymentStateIdEntity = GestionAerolineas.src.Modules.PaymentStates.Domain.ValueObject.PaymentStateId;

namespace GestionAerolineas.src.Modules.Payments.Application.UseCases;

public class UpdatePaymentUseCase
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentValidator _validator;
    private readonly ReservationRepository _reservationRepository;
    private readonly ReservationStatusRepository _reservationStatusRepository;
    private readonly ReservationStatusTransitionRepository _reservationStatusTransitionRepository;
    private readonly PaymentStateRepository _paymentStateRepository;

    public UpdatePaymentUseCase(
        IPaymentRepository paymentRepository,
        IPaymentValidator validator,
        ReservationRepository reservationRepository,
        ReservationStatusRepository reservationStatusRepository,
        ReservationStatusTransitionRepository reservationStatusTransitionRepository,
        PaymentStateRepository paymentStateRepository)
    {
        _paymentRepository = paymentRepository;
        _validator = validator;
        _reservationRepository = reservationRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _reservationStatusTransitionRepository = reservationStatusTransitionRepository;
        _paymentStateRepository = paymentStateRepository;
    }

    public async Task ExecuteAsync(int id, int reservationId, decimal amount, DateTime paidAt, int stateId, int methodId)
    {
        var idVO = PaymentId.Create(id);
        var reservationIdVO = PaymentReservationId.Create(reservationId);
        var amountVO = PaymentAmount.Create(amount);
        var paidAtVO = PaymentPaidAt.Create(paidAt);
        var stateIdVO = PaymentStateId.Create(stateId);
        var methodIdVO = PaymentMethodId.Create(methodId);

        var existing = await _paymentRepository.GetByIdAsync(idVO);
        if (existing is null)
            throw new Exception("No encontrado");

        await _validator.ValidateReservationExistsAsync(reservationIdVO);
        await _validator.ValidateReservationAllowsPaymentsAsync(reservationIdVO);
        await _validator.ValidatePaymentStateExistsAsync(stateIdVO);
        await _validator.ValidatePaymentMethodExistsAsync(methodIdVO);
        await _validator.ValidateNotOverpayAsync(reservationIdVO, amountVO, stateIdVO, idVO);

        var payment = Payment.Create(
            idVO,
            reservationIdVO,
            amountVO,
            paidAtVO,
            stateIdVO,
            methodIdVO,
            PaymentCreatedAt.CreateOptional(existing.CreatedAt.Value),
            PaymentUpdatedAt.CreateOptional(null));

        await _paymentRepository.UpdateAsync(payment);
        await TryConfirmReservationIfPaidAsync(reservationIdVO, stateIdVO);
    }

    private async Task TryConfirmReservationIfPaidAsync(PaymentReservationId reservationId, PaymentStateId paymentStateId)
    {
        var state = await _paymentStateRepository.GetByIdAsync(PaymentStateIdEntity.Create(paymentStateId.Value));
        var stateName = (state?.Name.Value ?? string.Empty).Trim().ToUpperInvariant();
        if (stateName != "PAGADO")
            return;

        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationId.Value));
        if (reservation is null)
            return;

        var confirmStatus = await _reservationStatusRepository.GetByNameAsync(ReservationStatusName.Create("Confirmada"));
        if (confirmStatus is null)
            return;

        if (reservation.StatusId.Value == confirmStatus.Id.Value)
            return;

        var transition = await _reservationStatusTransitionRepository.GetByPairAsync(
            GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject.ReservationStatusOriginId.Create(reservation.StatusId.Value),
            GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject.ReservationStatusDestinationId.Create(confirmStatus.Id.Value));

        if (transition is null)
            return;

        var updated = GestionAerolineas.src.Modules.Reservations.Domain.Aggregate.Reservation.Create(
            reservation.Id,
            reservation.Code,
            reservation.CustomerId,
            reservation.ReservedAt,
            GestionAerolineas.src.Modules.Reservations.Domain.ValueObject.ReservationStatusId.Create(confirmStatus.Id.Value),
            reservation.TotalAmount,
            reservation.ExpiresAt,
            reservation.CreatedAt,
            reservation.UpdatedAt);

        await _reservationRepository.UpdateAsync(updated);
    }
}

