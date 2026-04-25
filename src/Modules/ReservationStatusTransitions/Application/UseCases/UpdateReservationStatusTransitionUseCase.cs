// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\Application\UseCases\UpdateReservationStatusTransitionUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.Application.UseCases;

public class UpdateReservationStatusTransitionUseCase
{
    private readonly IReservationStatusTransitionRepository _repository;
    private readonly IReservationStatusTransitionValidator _validator;

    public UpdateReservationStatusTransitionUseCase(
        IReservationStatusTransitionRepository repository,
        IReservationStatusTransitionValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, int originStatusId, int destinationStatusId)
    {
        var idVO = ReservationStatusTransitionId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing is null)
            throw new Exception("La transición no existe");

        var originVO = ReservationStatusOriginId.Create(originStatusId);
        var destinationVO = ReservationStatusDestinationId.Create(destinationStatusId);

        await _validator.ValidatePairAsync(originVO, destinationVO, idVO);

        var updated = ReservationStatusTransition.Create(idVO, originVO, destinationVO);

        await _repository.UpdateAsync(updated);
    }
}
