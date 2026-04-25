// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatuses\Domain\Repositories\IReservationStatusRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatuses.Domain.Repositories;

public interface IReservationStatusRepository
{
    Task<IEnumerable<ReservationStatus>> GetAllAsync();
    Task<ReservationStatus?> GetByIdAsync(ReservationStatusId id);
    Task<ReservationStatus?> GetByNameAsync(ReservationStatusName name);
    Task AddAsync(ReservationStatus reservationStatus);
    Task UpdateAsync(ReservationStatus reservationStatus);
    Task DeleteAsync(ReservationStatus reservationStatus);
    Task<bool> ExistsAsync(ReservationStatusId id);
}

