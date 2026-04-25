// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\Application\Services\ReservationStatusTransitionValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.Application.Services;

public class ReservationStatusTransitionValidator : IReservationStatusTransitionValidator
{
    private readonly IReservationStatusTransitionRepository _repository;

    public ReservationStatusTransitionValidator(IReservationStatusTransitionRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidatePairAsync(
        ReservationStatusOriginId originStatusId,
        ReservationStatusDestinationId destinationStatusId,
        ReservationStatusTransitionId? currentId = null)
    {
        if (originStatusId.Value == destinationStatusId.Value)
            throw new Exception("El estado de origen y destino no pueden ser el mismo");

        var existing = await _repository.GetByPairAsync(originStatusId, destinationStatusId);

        if (existing is null)
            return;

        if (currentId != null && existing.Id.Value == currentId.Value)
            return;

        throw new Exception("Ya existe una transición con ese origen y destino");
    }
}
