// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\Application\UseCases\GetReservationStatusTransitionByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.Application.UseCases;

public class GetReservationStatusTransitionByIdUseCase
{
    private readonly IReservationStatusTransitionRepository _repository;

    public GetReservationStatusTransitionByIdUseCase(IReservationStatusTransitionRepository repository)
    {
        _repository = repository;
    }

    public Task<ReservationStatusTransition?> ExecuteAsync(int id)
    {
        var idVO = ReservationStatusTransitionId.Create(id);
        return _repository.GetByIdAsync(idVO);
    }
}
