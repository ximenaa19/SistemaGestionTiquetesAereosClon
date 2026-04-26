// [DocHeader]
// Modulo: Reservations
// Capa: Domain
// Archivo: src\Modules\Reservations\Domain\Repositories\IReservationWaitlistRepository.cs
// Responsabilidad: Contrato para persistir y consultar lista de espera de reprogramaciones.
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.Reservations.Domain.Repositories;

public interface IReservationWaitlistRepository
{
    Task AddAsync(ReservationWaitlistEntry entry);
    Task UpdateAsync(ReservationWaitlistEntry entry);
    Task<ReservationWaitlistEntry?> GetByIdAsync(int id);
    Task<ReservationWaitlistEntry?> GetFirstPendingByRequestedFlightIdAsync(int requestedFlightId);
    Task<IEnumerable<ReservationWaitlistEntry>> GetPendingByRequestedFlightIdAsync(int requestedFlightId);
    Task<bool> ExistsPendingByReservationFlightAndRequestedFlightAsync(int reservationFlightId, int requestedFlightId);
    Task<int> GetNextQueueOrderForRequestedFlightAsync(int requestedFlightId);
}

