// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\Application\UseCases\GetReservationStatusTransitionByPairUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.Application.UseCases;

public class GetReservationStatusTransitionByPairUseCase
{
    private readonly IReservationStatusTransitionRepository _repository;

    public GetReservationStatusTransitionByPairUseCase(IReservationStatusTransitionRepository repository)
    {
        _repository = repository;
    }

    public Task<ReservationStatusTransition?> ExecuteAsync(int originStatusId, int destinationStatusId)
    {
        var originVO = ReservationStatusOriginId.Create(originStatusId);
        var destinationVO = ReservationStatusDestinationId.Create(destinationStatusId);

        return _repository.GetByPairAsync(originVO, destinationVO);
    }
}
