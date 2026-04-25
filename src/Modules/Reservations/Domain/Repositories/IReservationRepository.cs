// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Domain\Repositories\IReservationRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Domain.Repositories;

public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task<Reservation?> GetByIdAsync(ReservationId id);
    Task<Reservation?> GetByCodeAsync(ReservationCode code);
    Task<IEnumerable<Reservation>> GetByCustomerIdAsync(ReservationCustomerId customerId);
    Task<IEnumerable<Reservation>> GetByStatusIdAsync(ReservationStatusId statusId);
    Task<IEnumerable<Reservation>> GetByReservedAtRangeAsync(DateTime fromInclusive, DateTime toInclusive);
    Task AddAsync(Reservation reservation);
    Task UpdateAsync(Reservation reservation);
    Task DeleteAsync(Reservation reservation);
    Task<bool> ExistsAsync(ReservationId id);
    Task<bool> ExistsByNormalizedCodeAsync(string normalizedCode, int? excludingId = null);
}
