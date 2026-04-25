// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Application\UseCases\UpdateReservationStatusUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reservations.Application.Interfaces;
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Application.UseCases;

public class UpdateReservationStatusUseCase
{
    private readonly IReservationRepository _repository;
    private readonly IReservationValidator _validator;

    public UpdateReservationStatusUseCase(IReservationRepository repository, IReservationValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int reservationId, int newStatusId)
    {
        var idVO = ReservationId.Create(reservationId);
        var newStatusIdVO = ReservationStatusId.Create(newStatusId);

        var reservation = await _repository.GetByIdAsync(idVO);
        if (reservation is null)
            throw new Exception("La reserva no existe");

        await _validator.ValidateStatusExistsAsync(newStatusIdVO);
        await _validator.ValidateStatusTransitionAsync(reservation.StatusId, newStatusIdVO);

        var updated = Reservation.Create(
            reservation.Id,
            reservation.Code,
            reservation.CustomerId,
            reservation.ReservedAt,
            newStatusIdVO,
            reservation.TotalAmount,
            reservation.ExpiresAt,
            reservation.CreatedAt,
            reservation.UpdatedAt);

        await _repository.UpdateAsync(updated);
    }
}

