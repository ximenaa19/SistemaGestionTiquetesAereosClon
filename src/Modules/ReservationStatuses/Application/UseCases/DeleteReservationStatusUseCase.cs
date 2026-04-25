// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatuses\Application\UseCases\DeleteReservationStatusUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;

public class DeleteReservationStatusUseCase
{
    private readonly IReservationStatusRepository _repository;

    public DeleteReservationStatusUseCase(IReservationStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var reservationStatusId = ReservationStatusId.Create(id);
        var reservationStatus = await _repository.GetByIdAsync(reservationStatusId);

        if (reservationStatus is null)
            throw new KeyNotFoundException($"ReservationStatus con id '{reservationStatusId.Value}' no existe.");

        await _repository.DeleteAsync(reservationStatus);
    }
}

