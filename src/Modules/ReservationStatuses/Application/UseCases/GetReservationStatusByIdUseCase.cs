// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatuses\Application\UseCases\GetReservationStatusByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;

public class GetReservationStatusByIdUseCase
{
    private readonly IReservationStatusRepository _repository;

    public GetReservationStatusByIdUseCase(IReservationStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReservationStatus?> ExecuteAsync(int id)
    {
        var idVO = ReservationStatusId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}

