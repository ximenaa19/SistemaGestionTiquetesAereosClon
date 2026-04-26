// [DocHeader]
// Modulo: Reservations
// Capa: Domain
// Archivo: src\Modules\Reservations\Domain\Repositories\IReservationRescheduleHistoryRepository.cs
// Responsabilidad: Contrato para registrar trazabilidad de reprogramaciones y promociones.
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.Reservations.Domain.Repositories;

public interface IReservationRescheduleHistoryRepository
{
    Task AddAsync(ReservationRescheduleHistory entry);
    Task<IEnumerable<ReservationRescheduleHistory>> GetByReservationIdAsync(int reservationId);
}

