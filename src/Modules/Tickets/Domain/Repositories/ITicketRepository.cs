// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Domain\Repositories\ITicketRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Tickets.Domain.Aggregate;
using GestionAerolineas.src.Modules.Tickets.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Tickets.Domain.Repositories;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<Ticket?> GetByIdAsync(TicketId id);
    Task<Ticket?> GetByCodeAsync(TicketCode code);
    Task<Ticket?> GetByReservationPassengerIdAsync(TicketReservationPassengerId reservationPassengerId);
    Task<IEnumerable<Ticket>> GetByStatusIdAsync(TicketStatusId statusId);
    Task<IEnumerable<Ticket>> GetByPassengerIdAsync(int passengerId);
    Task<IEnumerable<Ticket>> GetByReservationCodeAsync(string reservationCode);
    Task AddAsync(Ticket ticket);
    Task UpdateAsync(Ticket ticket);
    Task DeleteAsync(Ticket ticket);
    Task<bool> ExistsAsync(TicketId id);
    Task<bool> ExistsByReservationPassengerIdAsync(int reservationPassengerId, int? excludingTicketId = null);
    Task<bool> ExistsByNormalizedCodeAsync(string normalizedCode, int? excludingTicketId = null);
}

