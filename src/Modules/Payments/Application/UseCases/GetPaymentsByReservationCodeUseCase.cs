// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Application\UseCases\GetPaymentsByReservationCodeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Payments.Domain.Aggregate;
using GestionAerolineas.src.Modules.Payments.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Payments.Application.UseCases;

public class GetPaymentsByReservationCodeUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentsByReservationCodeUseCase(IReservationRepository reservationRepository, IPaymentRepository paymentRepository)
    {
        _reservationRepository = reservationRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<IEnumerable<Payment>> ExecuteAsync(string reservationCode)
    {
        var reservation = await _reservationRepository.GetByCodeAsync(ReservationCode.Create(reservationCode));
        if (reservation is null)
            return Enumerable.Empty<Payment>();

        return await _paymentRepository.GetByReservationIdAsync(
            Domain.ValueObject.PaymentReservationId.Create(reservation.Id.Value));
    }
}

