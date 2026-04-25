// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\TicketStatuses\Infrastructure\Repository\TicketStatusRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.TicketStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.TicketStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.TicketStatuses.Domain.ValueObject;
using GestionAerolineas.src.Modules.TicketStatuses.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.TicketStatuses.Infrastructure.Repository;

public class TicketStatusRepository : ITicketStatusRepository
{
    private readonly AppDbContext _context;

    public TicketStatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TicketStatus>> GetAllAsync()
    {
        var entities = await _context.TicketStatuses
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<TicketStatus?> GetByIdAsync(TicketStatusId id)
    {
        var entity = await _context.TicketStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<TicketStatus?> GetByNameAsync(TicketStatusName name)
    {
        var entity = await _context.TicketStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(TicketStatus ticketStatus)
    {
        await _context.TicketStatuses.AddAsync(MapToEntity(ticketStatus));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TicketStatus ticketStatus)
    {
        var existing = await _context.TicketStatuses
            .FirstOrDefaultAsync(e => e.Id == ticketStatus.Id.Value);

        if (existing is null)
            return;

        existing.Name = ticketStatus.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TicketStatus ticketStatus)
    {
        var entity = await _context.TicketStatuses.FindAsync(ticketStatus.Id.Value);

        if (entity is null)
            return;

        _context.TicketStatuses.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(TicketStatusId id)
    {
        return _context.TicketStatuses.AnyAsync(e => e.Id == id.Value);
    }

    private static TicketStatus MapToDomain(TicketStatusEntity entity)
    {
        return TicketStatus.Create(
            TicketStatusId.Create(entity.Id),
            TicketStatusName.Create(entity.Name ?? string.Empty)
        );
    }

    private static TicketStatusEntity MapToEntity(TicketStatus ticketStatus)
    {
        return new TicketStatusEntity
        {
            Id = ticketStatus.Id.Value,
            Name = ticketStatus.Name.Value
        };
    }
}
