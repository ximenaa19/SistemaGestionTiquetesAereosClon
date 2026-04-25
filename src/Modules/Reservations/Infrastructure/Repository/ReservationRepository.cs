// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Infrastructure\Repository\ReservationRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        var entities = await _context.Set<ReservationEntity>()
            .AsNoTracking()
            .OrderByDescending(e => e.ReservedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Reservation?> GetByIdAsync(ReservationId id)
    {
        var entity = await _context.Set<ReservationEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Reservation?> GetByCodeAsync(ReservationCode code)
    {
        var normalized = ReservationCode.Normalize(code.Value);

        var entity = await _context.Set<ReservationEntity>()
            .AsNoTracking()
            .Where(e => e.Code != null)
            .FirstOrDefaultAsync(e => e.Code!.Trim().ToUpper() == normalized);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Reservation>> GetByCustomerIdAsync(ReservationCustomerId customerId)
    {
        var entities = await _context.Set<ReservationEntity>()
            .AsNoTracking()
            .Where(e => e.CustomerId == customerId.Value)
            .OrderByDescending(e => e.ReservedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Reservation>> GetByStatusIdAsync(ReservationStatusId statusId)
    {
        var entities = await _context.Set<ReservationEntity>()
            .AsNoTracking()
            .Where(e => e.StatusId == statusId.Value)
            .OrderByDescending(e => e.ReservedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Reservation>> GetByReservedAtRangeAsync(DateTime fromInclusive, DateTime toInclusive)
    {
        var entities = await _context.Set<ReservationEntity>()
            .AsNoTracking()
            .Where(e => e.ReservedAt >= fromInclusive && e.ReservedAt <= toInclusive)
            .OrderByDescending(e => e.ReservedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(Reservation reservation)
    {
        await _context.Set<ReservationEntity>().AddAsync(MapToEntity(reservation));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Reservation reservation)
    {
        var existing = await _context.Set<ReservationEntity>()
            .FirstOrDefaultAsync(e => e.Id == reservation.Id.Value);

        if (existing is null)
            return;

        existing.Code = reservation.Code?.Value;
        existing.CustomerId = reservation.CustomerId.Value;
        existing.ReservedAt = reservation.ReservedAt.Value;
        existing.StatusId = reservation.StatusId.Value;
        existing.TotalAmount = reservation.TotalAmount.Value;
        existing.ExpiresAt = reservation.ExpiresAt.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Reservation reservation)
    {
        var entity = await _context.Set<ReservationEntity>().FindAsync(reservation.Id.Value);
        if (entity is null)
            return;

        _context.Set<ReservationEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(ReservationId id)
    {
        return _context.Set<ReservationEntity>().AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByNormalizedCodeAsync(string normalizedCode, int? excludingId = null)
    {
        var query = _context.Set<ReservationEntity>()
            .AsNoTracking()
            .Where(r => r.Code != null);

        if (excludingId.HasValue)
            query = query.Where(r => r.Id != excludingId.Value);

        return query.AnyAsync(r => r.Code!.Trim().ToUpper() == normalizedCode);
    }

    private static Reservation MapToDomain(ReservationEntity entity)
    {
        try
        {
            return Reservation.Create(
                ReservationId.Create(entity.Id),
                ReservationCode.CreateOptional(entity.Code),
                ReservationCustomerId.Create(entity.CustomerId),
                ReservationReservedAt.Create(entity.ReservedAt),
                ReservationStatusId.Create(entity.StatusId),
                ReservationTotalAmount.Create(entity.TotalAmount),
                ReservationExpiresAt.CreateOptional(entity.ExpiresAt),
                ReservationCreatedAt.CreateOptional(entity.CreatedAt),
                ReservationUpdatedAt.CreateOptional(entity.UpdatedAt)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro reservations(id={entity.Id}) tiene datos invalidos. " +
                $"cliente_id={entity.CustomerId}, estado_reserva_id={entity.StatusId}, valor_total={entity.TotalAmount}.",
                ex);
        }
    }

    private static ReservationEntity MapToEntity(Reservation reservation)
    {
        return new ReservationEntity
        {
            Id = reservation.Id.Value,
            Code = reservation.Code?.Value,
            CustomerId = reservation.CustomerId.Value,
            ReservedAt = reservation.ReservedAt.Value,
            StatusId = reservation.StatusId.Value,
            TotalAmount = reservation.TotalAmount.Value,
            ExpiresAt = reservation.ExpiresAt.Value,
            CreatedAt = reservation.CreatedAt.Value,
            UpdatedAt = reservation.UpdatedAt.Value
        };
    }
}
