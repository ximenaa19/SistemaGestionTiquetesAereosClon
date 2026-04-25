// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\Infrastructure\Repository\ReservationFlightRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;

public class ReservationFlightRepository : IReservationFlightRepository
{
    private readonly AppDbContext _context;

    public ReservationFlightRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReservationFlight>> GetAllAsync()
    {
        var entities = await _context.Set<ReservationFlightEntity>()
            .AsNoTracking()
            .OrderBy(e => e.ReservationId)
            .ThenBy(e => e.FlightId)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<ReservationFlight?> GetByIdAsync(ReservationFlightId id)
    {
        var entity = await _context.Set<ReservationFlightEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<ReservationFlight>> GetByReservationIdAsync(ReservationFlightReservationId reservationId)
    {
        var entities = await _context.Set<ReservationFlightEntity>()
            .AsNoTracking()
            .Where(e => e.ReservationId == reservationId.Value)
            .OrderBy(e => e.FlightId)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<ReservationFlight>> GetByFlightIdAsync(ReservationFlightFlightId flightId)
    {
        var entities = await _context.Set<ReservationFlightEntity>()
            .AsNoTracking()
            .Where(e => e.FlightId == flightId.Value)
            .OrderBy(e => e.ReservationId)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<ReservationFlight?> GetByReservationAndFlightAsync(ReservationFlightReservationId reservationId, ReservationFlightFlightId flightId)
    {
        var entity = await _context.Set<ReservationFlightEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ReservationId == reservationId.Value && e.FlightId == flightId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(ReservationFlight reservationFlight)
    {
        await _context.Set<ReservationFlightEntity>().AddAsync(MapToEntity(reservationFlight));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ReservationFlight reservationFlight)
    {
        var existing = await _context.Set<ReservationFlightEntity>()
            .FirstOrDefaultAsync(e => e.Id == reservationFlight.Id.Value);

        if (existing is null)
            return;

        existing.ReservationId = reservationFlight.ReservationId.Value;
        existing.FlightId = reservationFlight.FlightId.Value;
        existing.PartialAmount = reservationFlight.PartialAmount.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ReservationFlight reservationFlight)
    {
        var entity = await _context.Set<ReservationFlightEntity>().FindAsync(reservationFlight.Id.Value);
        if (entity is null)
            return;

        _context.Set<ReservationFlightEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(ReservationFlightId id)
    {
        return _context.Set<ReservationFlightEntity>().AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByReservationAndFlightAsync(int reservationId, int flightId, int? excludingId = null)
    {
        var query = _context.Set<ReservationFlightEntity>()
            .AsNoTracking()
            .Where(e => e.ReservationId == reservationId && e.FlightId == flightId);

        if (excludingId.HasValue)
            query = query.Where(e => e.Id != excludingId.Value);

        return query.AnyAsync();
    }

    public Task<decimal> SumPartialAmountByReservationIdAsync(int reservationId)
    {
        return _context.Set<ReservationFlightEntity>()
            .AsNoTracking()
            .Where(e => e.ReservationId == reservationId)
            .SumAsync(e => e.PartialAmount);
    }

    private static ReservationFlight MapToDomain(ReservationFlightEntity entity)
    {
        try
        {
            return ReservationFlight.Create(
                ReservationFlightId.Create(entity.Id),
                ReservationFlightReservationId.Create(entity.ReservationId),
                ReservationFlightFlightId.Create(entity.FlightId),
                ReservationFlightPartialAmount.Create(entity.PartialAmount));
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro reservationflights(id={entity.Id}) tiene datos invalidos. " +
                $"reserva_id={entity.ReservationId}, vuelo_id={entity.FlightId}, valor_parcial={entity.PartialAmount}.",
                ex);
        }
    }

    private static ReservationFlightEntity MapToEntity(ReservationFlight reservationFlight)
    {
        return new ReservationFlightEntity
        {
            Id = reservationFlight.Id.Value,
            ReservationId = reservationFlight.ReservationId.Value,
            FlightId = reservationFlight.FlightId.Value,
            PartialAmount = reservationFlight.PartialAmount.Value
        };
    }
}

