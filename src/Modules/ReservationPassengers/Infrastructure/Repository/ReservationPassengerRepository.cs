// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Infrastructure\Repository\ReservationPassengerRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Repository;

public class ReservationPassengerRepository : IReservationPassengerRepository
{
    private readonly AppDbContext _context;

    public ReservationPassengerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReservationPassenger>> GetAllAsync()
    {
        var entities = await _context.Set<ReservationPassengerEntity>()
            .AsNoTracking()
            .OrderBy(e => e.ReservationFlightId)
            .ThenBy(e => e.PassengerId)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<ReservationPassenger?> GetByIdAsync(ReservationPassengerId id)
    {
        var entity = await _context.Set<ReservationPassengerEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<ReservationPassenger>> GetByReservationFlightIdAsync(ReservationPassengerReservationFlightId reservationFlightId)
    {
        var entities = await _context.Set<ReservationPassengerEntity>()
            .AsNoTracking()
            .Where(e => e.ReservationFlightId == reservationFlightId.Value)
            .OrderBy(e => e.PassengerId)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<ReservationPassenger>> GetByPassengerIdAsync(ReservationPassengerPassengerId passengerId)
    {
        var entities = await _context.Set<ReservationPassengerEntity>()
            .AsNoTracking()
            .Where(e => e.PassengerId == passengerId.Value)
            .OrderBy(e => e.ReservationFlightId)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<ReservationPassenger?> GetByReservationFlightAndPassengerAsync(
        ReservationPassengerReservationFlightId reservationFlightId,
        ReservationPassengerPassengerId passengerId)
    {
        var entity = await _context.Set<ReservationPassengerEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ReservationFlightId == reservationFlightId.Value && e.PassengerId == passengerId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(ReservationPassenger reservationPassenger)
    {
        await _context.Set<ReservationPassengerEntity>().AddAsync(MapToEntity(reservationPassenger));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ReservationPassenger reservationPassenger)
    {
        var existing = await _context.Set<ReservationPassengerEntity>()
            .FirstOrDefaultAsync(e => e.Id == reservationPassenger.Id.Value);

        if (existing is null)
            return;

        existing.ReservationFlightId = reservationPassenger.ReservationFlightId.Value;
        existing.PassengerId = reservationPassenger.PassengerId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ReservationPassenger reservationPassenger)
    {
        var entity = await _context.Set<ReservationPassengerEntity>().FindAsync(reservationPassenger.Id.Value);
        if (entity is null)
            return;

        _context.Set<ReservationPassengerEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(ReservationPassengerId id)
    {
        return _context.Set<ReservationPassengerEntity>().AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByReservationFlightAndPassengerAsync(int reservationFlightId, int passengerId, int? excludingId = null)
    {
        var query = _context.Set<ReservationPassengerEntity>()
            .AsNoTracking()
            .Where(e => e.ReservationFlightId == reservationFlightId && e.PassengerId == passengerId);

        if (excludingId.HasValue)
            query = query.Where(e => e.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static ReservationPassenger MapToDomain(ReservationPassengerEntity entity)
    {
        try
        {
            return ReservationPassenger.Create(
                ReservationPassengerId.Create(entity.Id),
                ReservationPassengerReservationFlightId.Create(entity.ReservationFlightId),
                ReservationPassengerPassengerId.Create(entity.PassengerId));
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro reservationpassengers(id={entity.Id}) tiene datos invalidos. " +
                $"reserva_vuelo_id={entity.ReservationFlightId}, pasajero_id={entity.PassengerId}.",
                ex);
        }
    }

    private static ReservationPassengerEntity MapToEntity(ReservationPassenger reservationPassenger)
    {
        return new ReservationPassengerEntity
        {
            Id = reservationPassenger.Id.Value,
            ReservationFlightId = reservationPassenger.ReservationFlightId.Value,
            PassengerId = reservationPassenger.PassengerId.Value
        };
    }
}

