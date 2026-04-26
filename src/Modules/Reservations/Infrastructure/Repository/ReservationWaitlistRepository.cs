// [DocHeader]
// Modulo: Reservations
// Capa: Infrastructure
// Archivo: src\Modules\Reservations\Infrastructure\Repository\ReservationWaitlistRepository.cs
// Responsabilidad: Acceso a datos de lista de espera de reprogramacion.
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;

public sealed class ReservationWaitlistRepository : IReservationWaitlistRepository
{
    private readonly AppDbContext _context;

    public ReservationWaitlistRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ReservationWaitlistEntry entry)
    {
        await _context.Set<ReservationWaitlistEntity>().AddAsync(MapToEntity(entry));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ReservationWaitlistEntry entry)
    {
        var entity = await _context.Set<ReservationWaitlistEntity>().FirstOrDefaultAsync(x => x.Id == entry.Id);
        if (entity is null)
            return;

        entity.ReservationId = entry.ReservationId;
        entity.ReservationFlightId = entry.ReservationFlightId;
        entity.RequestedFlightId = entry.RequestedFlightId;
        entity.RequestedAt = entry.RequestedAt;
        entity.QueueOrder = entry.QueueOrder;
        entity.Status = entry.Status;
        entity.Reason = entry.Reason;
        entity.ProcessedAt = entry.ProcessedAt;

        await _context.SaveChangesAsync();
    }

    public async Task<ReservationWaitlistEntry?> GetByIdAsync(int id)
    {
        var entity = await _context.Set<ReservationWaitlistEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<ReservationWaitlistEntry?> GetFirstPendingByRequestedFlightIdAsync(int requestedFlightId)
    {
        var entity = await _context.Set<ReservationWaitlistEntity>()
            .AsNoTracking()
            .Where(x => x.RequestedFlightId == requestedFlightId && x.Status == "PENDIENTE")
            .OrderBy(x => x.QueueOrder)
            .ThenBy(x => x.RequestedAt)
            .ThenBy(x => x.Id)
            .FirstOrDefaultAsync();

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<ReservationWaitlistEntry>> GetPendingByRequestedFlightIdAsync(int requestedFlightId)
    {
        var entities = await _context.Set<ReservationWaitlistEntity>()
            .AsNoTracking()
            .Where(x => x.RequestedFlightId == requestedFlightId && x.Status == "PENDIENTE")
            .OrderBy(x => x.QueueOrder)
            .ThenBy(x => x.RequestedAt)
            .ThenBy(x => x.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public Task<bool> ExistsPendingByReservationFlightAndRequestedFlightAsync(int reservationFlightId, int requestedFlightId)
    {
        return _context.Set<ReservationWaitlistEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.ReservationFlightId == reservationFlightId
                           && x.RequestedFlightId == requestedFlightId
                           && x.Status == "PENDIENTE");
    }

    public async Task<int> GetNextQueueOrderForRequestedFlightAsync(int requestedFlightId)
    {
        var max = await _context.Set<ReservationWaitlistEntity>()
            .AsNoTracking()
            .Where(x => x.RequestedFlightId == requestedFlightId)
            .Select(x => (int?)x.QueueOrder)
            .MaxAsync();

        return (max ?? 0) + 1;
    }

    private static ReservationWaitlistEntry MapToDomain(ReservationWaitlistEntity entity)
    {
        return ReservationWaitlistEntry.Rehydrate(
            entity.Id,
            entity.ReservationId,
            entity.ReservationFlightId,
            entity.RequestedFlightId,
            entity.RequestedAt,
            entity.QueueOrder,
            entity.Status,
            entity.Reason,
            entity.ProcessedAt);
    }

    private static ReservationWaitlistEntity MapToEntity(ReservationWaitlistEntry entry)
    {
        return new ReservationWaitlistEntity
        {
            Id = entry.Id,
            ReservationId = entry.ReservationId,
            ReservationFlightId = entry.ReservationFlightId,
            RequestedFlightId = entry.RequestedFlightId,
            RequestedAt = entry.RequestedAt,
            QueueOrder = entry.QueueOrder,
            Status = entry.Status,
            Reason = entry.Reason,
            ProcessedAt = entry.ProcessedAt
        };
    }
}

