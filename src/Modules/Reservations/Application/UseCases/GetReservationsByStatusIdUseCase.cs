// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Application\UseCases\GetReservationsByStatusIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Application.UseCases;

public class GetReservationsByStatusIdUseCase
{
    private readonly IReservationRepository _repository;

    public GetReservationsByStatusIdUseCase(IReservationRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Reservation>> ExecuteAsync(int statusId)
    {
        return _repository.GetByStatusIdAsync(ReservationStatusId.Create(statusId));
    }
}

