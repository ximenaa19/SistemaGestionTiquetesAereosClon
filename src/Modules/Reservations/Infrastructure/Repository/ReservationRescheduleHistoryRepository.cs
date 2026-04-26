// [DocHeader]
// Modulo: Reservations
// Capa: Infrastructure
// Archivo: src\Modules\Reservations\Infrastructure\Repository\ReservationRescheduleHistoryRepository.cs
// Responsabilidad: Persistencia de historial de reprogramaciones/promociones.
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;

public sealed class ReservationRescheduleHistoryRepository : IReservationRescheduleHistoryRepository
{
    private readonly AppDbContext _context;

    public ReservationRescheduleHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ReservationRescheduleHistory entry)
    {
        await _context.Set<ReservationRescheduleHistoryEntity>().AddAsync(MapToEntity(entry));
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ReservationRescheduleHistory>> GetByReservationIdAsync(int reservationId)
    {
        var list = await _context.Set<ReservationRescheduleHistoryEntity>()
            .AsNoTracking()
            .Where(x => x.ReservationId == reservationId)
            .OrderByDescending(x => x.ChangedAt)
            .ThenByDescending(x => x.Id)
            .ToListAsync();

        return list.Select(MapToDomain).ToList();
    }

    private static ReservationRescheduleHistory MapToDomain(ReservationRescheduleHistoryEntity entity)
    {
        return ReservationRescheduleHistory.Rehydrate(
            entity.Id,
            entity.ReservationId,
            entity.OldFlightId,
            entity.NewFlightId,
            entity.ChangedAt,
            entity.Reason,
            entity.ActionStatus);
    }

    private static ReservationRescheduleHistoryEntity MapToEntity(ReservationRescheduleHistory entry)
    {
        return new ReservationRescheduleHistoryEntity
        {
            Id = entry.Id,
            ReservationId = entry.ReservationId,
            OldFlightId = entry.OldFlightId,
            NewFlightId = entry.NewFlightId,
            ChangedAt = entry.ChangedAt,
            Reason = entry.Reason,
            ActionStatus = entry.ActionStatus
        };
    }
}

