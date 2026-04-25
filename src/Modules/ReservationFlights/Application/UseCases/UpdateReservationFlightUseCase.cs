// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\Application\UseCases\UpdateReservationFlightUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationFlights.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;

public class UpdateReservationFlightUseCase
{
    private readonly IReservationFlightRepository _repository;
    private readonly IReservationFlightValidator _validator;
    private readonly IReservationRepository _reservationRepository;

    public UpdateReservationFlightUseCase(
        IReservationFlightRepository repository,
        IReservationFlightValidator validator,
        IReservationRepository reservationRepository)
    {
        _repository = repository;
        _validator = validator;
        _reservationRepository = reservationRepository;
    }

    public async Task ExecuteAsync(int id, int reservationId, int flightId, decimal partialAmount)
    {
        var idVO = ReservationFlightId.Create(id);
        var reservationIdVO = ReservationFlightReservationId.Create(reservationId);
        var flightIdVO = ReservationFlightFlightId.Create(flightId);
        var partialVO = ReservationFlightPartialAmount.Create(partialAmount);

        await _validator.ValidateReservationExistsAsync(reservationIdVO);
        await _validator.ValidateReservationAllowsChangesAsync(reservationIdVO);
        await _validator.ValidateFlightExistsAsync(flightIdVO);
        await _validator.ValidateFlightNotInFinalStateAsync(flightIdVO);
        await _validator.ValidateUniquePairAsync(reservationIdVO, flightIdVO, idVO);

        var entity = ReservationFlight.Create(idVO, reservationIdVO, flightIdVO, partialVO);
        await _repository.UpdateAsync(entity);

        await RecalculateTotalAsync(reservationIdVO.Value);
    }

    private async Task RecalculateTotalAsync(int reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationId));
        if (reservation is null)
            return;

        var total = await _repository.SumPartialAmountByReservationIdAsync(reservationId);
        var updated = GestionAerolineas.src.Modules.Reservations.Domain.Aggregate.Reservation.Create(
            reservation.Id,
            reservation.Code,
            reservation.CustomerId,
            reservation.ReservedAt,
            reservation.StatusId,
            ReservationTotalAmount.Create(total),
            reservation.ExpiresAt,
            reservation.CreatedAt,
            reservation.UpdatedAt);

        await _reservationRepository.UpdateAsync(updated);
    }
}
