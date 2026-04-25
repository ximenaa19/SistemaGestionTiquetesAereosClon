// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\Application\UseCases\DeleteReservationStatusTransitionUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.Application.UseCases;

public class DeleteReservationStatusTransitionUseCase
{
    private readonly IReservationStatusTransitionRepository _repository;

    public DeleteReservationStatusTransitionUseCase(IReservationStatusTransitionRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var idVO = ReservationStatusTransitionId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing is null)
            throw new Exception("La transición no existe");

        await _repository.DeleteAsync(existing);
    }
}
