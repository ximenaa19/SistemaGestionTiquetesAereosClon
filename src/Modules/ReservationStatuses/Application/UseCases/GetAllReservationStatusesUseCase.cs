// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatuses\Application\UseCases\GetAllReservationStatusesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.Repositories;

namespace GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;

public class GetAllReservationStatusesUseCase
{
    private readonly IReservationStatusRepository _repository;

    public GetAllReservationStatusesUseCase(IReservationStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ReservationStatus>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}

