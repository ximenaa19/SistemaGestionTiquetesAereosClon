// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\TicketStatuses\Domain\Repositories\ITicketStatusRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.TicketStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.TicketStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.TicketStatuses.Domain.Repositories;

public interface ITicketStatusRepository
{
    Task<IEnumerable<TicketStatus>> GetAllAsync();
    Task<TicketStatus?> GetByIdAsync(TicketStatusId id);
    Task<TicketStatus?> GetByNameAsync(TicketStatusName name);
    Task AddAsync(TicketStatus ticketStatus);
    Task UpdateAsync(TicketStatus ticketStatus);
    Task DeleteAsync(TicketStatus ticketStatus);
    Task<bool> ExistsAsync(TicketStatusId id);
}
