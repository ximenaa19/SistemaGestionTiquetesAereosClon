// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Infrastructure\Repository\TicketRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Tickets.Domain.Aggregate;
using GestionAerolineas.src.Modules.Tickets.Domain.Repositories;
using GestionAerolineas.src.Modules.Tickets.Domain.ValueObject;
using GestionAerolineas.src.Modules.Tickets.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Tickets.Infrastructure.Repository;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        var entities = await _context.Set<TicketEntity>()
            .AsNoTracking()
            .OrderByDescending(e => e.IssuedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Ticket?> GetByIdAsync(TicketId id)
    {
        var entity = await _context.Set<TicketEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Ticket?> GetByCodeAsync(TicketCode code)
    {
        var normalized = TicketCode.Normalize(code.Value);

        var entity = await _context.Set<TicketEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Code != null && e.Code.Trim().ToUpper() == normalized);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Ticket?> GetByReservationPassengerIdAsync(TicketReservationPassengerId reservationPassengerId)
    {
        var entity = await _context.Set<TicketEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ReservationPassengerId == reservationPassengerId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Ticket>> GetByStatusIdAsync(TicketStatusId statusId)
    {
        var entities = await _context.Set<TicketEntity>()
            .AsNoTracking()
            .Where(e => e.StatusId == statusId.Value)
            .OrderByDescending(e => e.IssuedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Ticket>> GetByPassengerIdAsync(int passengerId)
    {
        var query =
            from t in _context.Set<TicketEntity>().AsNoTracking()
            join rp in _context.Set<ReservationPassengerEntity>().AsNoTracking() on t.ReservationPassengerId equals rp.Id
            where rp.PassengerId == passengerId
            orderby t.IssuedAt descending, t.Id descending
            select t;

        var entities = await query.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Ticket>> GetByReservationCodeAsync(string reservationCode)
    {
        var normalized = (reservationCode ?? string.Empty).Trim().ToUpperInvariant();
        if (string.IsNullOrWhiteSpace(normalized))
            return Array.Empty<Ticket>();

        var query =
            from t in _context.Set<TicketEntity>().AsNoTracking()
            join rp in _context.Set<ReservationPassengerEntity>().AsNoTracking() on t.ReservationPassengerId equals rp.Id
            join rf in _context.Set<ReservationFlightEntity>().AsNoTracking() on rp.ReservationFlightId equals rf.Id
            join r in _context.Set<ReservationEntity>().AsNoTracking() on rf.ReservationId equals r.Id
            where r.Code != null && r.Code.Trim().ToUpper() == normalized
            orderby t.IssuedAt descending, t.Id descending
            select t;

        var entities = await query.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(Ticket ticket)
    {
        await _context.Set<TicketEntity>().AddAsync(MapToEntity(ticket));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Ticket ticket)
    {
        var existing = await _context.Set<TicketEntity>()
            .FirstOrDefaultAsync(e => e.Id == ticket.Id.Value);

        if (existing is null)
            return;

        existing.ReservationPassengerId = ticket.ReservationPassengerId.Value;
        existing.Code = ticket.Code.Value;
        existing.IssuedAt = ticket.IssuedAt.Value;
        existing.StatusId = ticket.StatusId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Ticket ticket)
    {
        var entity = await _context.Set<TicketEntity>().FindAsync(ticket.Id.Value);
        if (entity is null)
            return;

        _context.Set<TicketEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(TicketId id)
    {
        return _context.Set<TicketEntity>().AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByReservationPassengerIdAsync(int reservationPassengerId, int? excludingTicketId = null)
    {
        var query = _context.Set<TicketEntity>()
            .AsNoTracking()
            .Where(e => e.ReservationPassengerId == reservationPassengerId);

        if (excludingTicketId.HasValue)
            query = query.Where(e => e.Id != excludingTicketId.Value);

        return query.AnyAsync();
    }

    public Task<bool> ExistsByNormalizedCodeAsync(string normalizedCode, int? excludingTicketId = null)
    {
        var query = _context.Set<TicketEntity>()
            .AsNoTracking()
            .Where(e => e.Code != null);

        if (excludingTicketId.HasValue)
            query = query.Where(e => e.Id != excludingTicketId.Value);

        return query.AnyAsync(e => e.Code!.Trim().ToUpper() == normalizedCode);
    }

    private static Ticket MapToDomain(TicketEntity entity)
    {
        try
        {
            return Ticket.Create(
                TicketId.Create(entity.Id),
                TicketReservationPassengerId.Create(entity.ReservationPassengerId),
                TicketCode.Create(entity.Code ?? string.Empty),
                TicketIssuedAt.Create(entity.IssuedAt),
                TicketStatusId.Create(entity.StatusId),
                TicketCreatedAt.CreateOptional(entity.CreatedAt),
                TicketUpdatedAt.CreateOptional(entity.UpdatedAt));
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro tickets(id={entity.Id}) tiene datos invalidos. " +
                $"reserva_pasajero_id={entity.ReservationPassengerId}, codigo_tiquete='{entity.Code}', estado_tiquete_id={entity.StatusId}, fecha_emision='{entity.IssuedAt}'.",
                ex);
        }
    }

    private static TicketEntity MapToEntity(Ticket ticket)
    {
        return new TicketEntity
        {
            Id = ticket.Id.Value,
            ReservationPassengerId = ticket.ReservationPassengerId.Value,
            Code = ticket.Code.Value,
            IssuedAt = ticket.IssuedAt.Value,
            StatusId = ticket.StatusId.Value,
            CreatedAt = ticket.CreatedAt.Value,
            UpdatedAt = ticket.UpdatedAt.Value
        };
    }
}

