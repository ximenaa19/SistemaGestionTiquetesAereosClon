// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\Infrastructure\Repository\RouteStopRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RouteStops.Domain.Aggregate;
using GestionAerolineas.src.Modules.RouteStops.Domain.Repositories;
using GestionAerolineas.src.Modules.RouteStops.Domain.ValueObject;
using GestionAerolineas.src.Modules.RouteStops.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.RouteStops.Infrastructure.Repository;

public class RouteStopRepository : IRouteStopRepository
{
    private readonly AppDbContext _context;

    public RouteStopRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RouteStop>> GetAllAsync()
    {
        var entities = await _context.RouteStops
            .AsNoTracking()
            .OrderBy(e => e.RouteId)
            .ThenBy(e => e.Order)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<RouteStop?> GetByIdAsync(RouteStopId id)
    {
        var entity = await _context.RouteStops
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<RouteStop>> GetByRouteIdAsync(RouteStopRouteId routeId)
    {
        var entities = await _context.RouteStops
            .AsNoTracking()
            .Where(e => e.RouteId == routeId.Value)
            .OrderBy(e => e.Order)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<RouteStop?> GetByRouteAndOrderAsync(RouteStopRouteId routeId, RouteStopOrder order)
    {
        var entity = await _context.RouteStops
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.RouteId == routeId.Value && e.Order == order.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(RouteStop routeStop)
    {
        await _context.RouteStops.AddAsync(MapToEntity(routeStop));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(RouteStop routeStop)
    {
        var existing = await _context.RouteStops
            .FirstOrDefaultAsync(e => e.Id == routeStop.Id.Value);

        if (existing is null)
            return;

        existing.RouteId = routeStop.RouteId.Value;
        existing.StopAirportId = routeStop.StopAirportId.Value;
        existing.Order = routeStop.Order.Value;
        existing.DurationMinutes = routeStop.DurationMinutes.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(RouteStop routeStop)
    {
        var entity = await _context.RouteStops.FindAsync(routeStop.Id.Value);
        if (entity is null)
            return;

        _context.RouteStops.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(RouteStopId id)
    {
        return _context.RouteStops.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByRouteAndOrderAsync(int routeId, int order, int? excludingId = null)
    {
        var query = _context.RouteStops
            .AsNoTracking()
            .Where(e => e.RouteId == routeId && e.Order == order);

        if (excludingId.HasValue)
            query = query.Where(e => e.Id != excludingId.Value);

        return query.AnyAsync();
    }

    public Task<bool> ExistsByRouteAndStopAirportAsync(int routeId, int stopAirportId, int? excludingId = null)
    {
        var query = _context.RouteStops
            .AsNoTracking()
            .Where(e => e.RouteId == routeId && e.StopAirportId == stopAirportId);

        if (excludingId.HasValue)
            query = query.Where(e => e.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static RouteStop MapToDomain(RouteStopEntity entity)
    {
        try
        {
            return RouteStop.Create(
                RouteStopId.Create(entity.Id),
                RouteStopRouteId.Create(entity.RouteId),
                RouteStopStopAirportId.Create(entity.StopAirportId),
                RouteStopOrder.Create(entity.Order),
                RouteStopDurationMinutes.Create(entity.DurationMinutes)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro routestops(id={entity.Id}) tiene datos invalidos. " +
                $"ruta_id={entity.RouteId}, aeropuerto_escala_id={entity.StopAirportId}, " +
                $"orden={entity.Order}, duracion_escala_min={entity.DurationMinutes}.",
                ex);
        }
    }

    private static RouteStopEntity MapToEntity(RouteStop routeStop)
    {
        return new RouteStopEntity
        {
            Id = routeStop.Id.Value,
            RouteId = routeStop.RouteId.Value,
            StopAirportId = routeStop.StopAirportId.Value,
            Order = routeStop.Order.Value,
            DurationMinutes = routeStop.DurationMinutes.Value
        };
    }
}

