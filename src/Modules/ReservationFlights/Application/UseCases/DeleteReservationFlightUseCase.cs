// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\Application\UseCases\DeleteReservationFlightUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationFlights.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;

public class DeleteReservationFlightUseCase
{
    private readonly IReservationFlightRepository _repository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationFlightValidator _validator;

    public DeleteReservationFlightUseCase(
        IReservationFlightRepository repository,
        IReservationRepository reservationRepository,
        IReservationFlightValidator validator)
    {
        _repository = repository;
        _reservationRepository = reservationRepository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(ReservationFlightId.Create(id));
        if (entity is null)
            return;

        await _validator.ValidateReservationAllowsChangesAsync(entity.ReservationId);
        await _validator.ValidateNoPassengersAsync(entity.Id);

        var reservationId = entity.ReservationId.Value;

        await _repository.DeleteAsync(entity);
        await RecalculateTotalAsync(reservationId);
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
