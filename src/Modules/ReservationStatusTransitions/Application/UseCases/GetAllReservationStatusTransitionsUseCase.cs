// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\Application\UseCases\GetAllReservationStatusTransitionsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Repositories;

namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.Application.UseCases;

public class GetAllReservationStatusTransitionsUseCase
{
    private readonly IReservationStatusTransitionRepository _repository;

    public GetAllReservationStatusTransitionsUseCase(IReservationStatusTransitionRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<ReservationStatusTransition>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}
