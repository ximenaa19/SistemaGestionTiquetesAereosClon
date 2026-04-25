// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\Domain\Repositories\IReservationStatusTransitionRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Repositories;

public interface IReservationStatusTransitionRepository
{
    Task<IEnumerable<ReservationStatusTransition>> GetAllAsync();
    Task<ReservationStatusTransition?> GetByIdAsync(ReservationStatusTransitionId id);

    Task<ReservationStatusTransition?> GetByPairAsync(
        ReservationStatusOriginId originStatusId,
        ReservationStatusDestinationId destinationStatusId
    );

    Task AddAsync(ReservationStatusTransition transition);
    Task UpdateAsync(ReservationStatusTransition transition);
    Task DeleteAsync(ReservationStatusTransition transition);

    Task<bool> ExistsAsync(ReservationStatusTransitionId id);
}
