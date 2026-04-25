// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Infrastructure\Repository\SessionRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Sessions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Sessions.Domain.Repositories;
using GestionAerolineas.src.Modules.Sessions.Domain.ValueObject;
using GestionAerolineas.src.Modules.Sessions.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Sessions.Infrastructure.Repository;

public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _context;

    public SessionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Session>> GetAllAsync()
    {
        var entities = await _context.Sessions
            .AsNoTracking()
            .OrderByDescending(e => e.StartedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Session?> GetByIdAsync(SessionId id)
    {
        var entity = await _context.Sessions
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Session>> GetByUserIdAsync(SessionUserId userId)
    {
        var entities = await _context.Sessions
            .AsNoTracking()
            .Where(e => e.UserId == userId.Value)
            .OrderByDescending(e => e.StartedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Session>> GetByIsActiveAsync(SessionIsActive isActive)
    {
        var entities = await _context.Sessions
            .AsNoTracking()
            .Where(e => e.IsActive == isActive.Value)
            .OrderByDescending(e => e.StartedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Session>> GetByDateRangeAsync(DateTime from, DateTime to)
    {
        var entities = await _context.Sessions
            .AsNoTracking()
            .Where(e => e.StartedAt.HasValue && e.StartedAt.Value >= from && e.StartedAt.Value <= to)
            .OrderByDescending(e => e.StartedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Session>> GetActiveByUserIdAsync(SessionUserId userId)
    {
        var entities = await _context.Sessions
            .AsNoTracking()
            .Where(e => e.UserId == userId.Value && e.IsActive)
            .OrderByDescending(e => e.StartedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(Session session)
    {
        await _context.Sessions.AddAsync(MapToEntity(session));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Session session)
    {
        var existing = await _context.Sessions.FirstOrDefaultAsync(e => e.Id == session.Id.Value);
        if (existing is null)
            return;

        existing.UserId = session.UserId.Value;
        existing.StartedAt = session.StartedAt.Value;
        existing.EndedAt = session.EndedAt.Value;
        existing.IpAddress = session.IpAddress.Value;
        existing.IsActive = session.IsActive.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Session session)
    {
        var entity = await _context.Sessions.FindAsync(session.Id.Value);
        if (entity is null)
            return;

        _context.Sessions.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(SessionId id)
    {
        return _context.Sessions.AnyAsync(e => e.Id == id.Value);
    }

    private static Session MapToDomain(SessionEntity entity)
    {
        try
        {
            return Session.Create(
                SessionId.Create(entity.Id),
                SessionUserId.Create(entity.UserId),
                SessionStartedAt.Create(entity.StartedAt),
                SessionEndedAt.Create(entity.EndedAt),
                SessionIpAddress.Create(entity.IpAddress),
                SessionIsActive.Create(entity.IsActive));
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro sessions(id={entity.Id}) tiene datos invalidos. " +
                $"usuario_id={entity.UserId}, iniciada_en='{entity.StartedAt}', cerrada_en='{entity.EndedAt}', ip_origen='{entity.IpAddress}', activa={entity.IsActive}.",
                ex);
        }
    }

    private static SessionEntity MapToEntity(Session session)
    {
        return new SessionEntity
        {
            Id = session.Id.Value,
            UserId = session.UserId.Value,
            StartedAt = session.StartedAt.Value,
            EndedAt = session.EndedAt.Value,
            IpAddress = session.IpAddress.Value,
            IsActive = session.IsActive.Value
        };
    }
}
