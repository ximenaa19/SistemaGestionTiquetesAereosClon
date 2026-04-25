// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Infrastructure\Repository\RouteRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Routes.Domain.Aggregate;
using GestionAerolineas.src.Modules.Routes.Domain.Repositories;
using GestionAerolineas.src.Modules.Routes.Domain.ValueObject;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Routes.Infrastructure.Repository;

public class RouteRepository : IRouteRepository
{
    private readonly AppDbContext _context;

    public RouteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Route>> GetAllAsync()
    {
        var entities = await _context.Routes.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Route?> GetByIdAsync(RouteId id)
    {
        var entity = await _context.Routes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Route?> GetByOriginAndDestinationAsync(RouteAirportId originAirportId, RouteAirportId destinationAirportId)
    {
        var entity = await _context.Routes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.OriginAirportId == originAirportId.Value && e.DestinationAirportId == destinationAirportId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(Route route)
    {
        await _context.Routes.AddAsync(MapToEntity(route));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Route route)
    {
        var existing = await _context.Routes
            .FirstOrDefaultAsync(e => e.Id == route.Id.Value);

        if (existing is null)
            return;

        existing.OriginAirportId = route.OriginAirportId.Value;
        existing.DestinationAirportId = route.DestinationAirportId.Value;
        existing.DistanceKm = route.DistanceKm.Value;
        existing.EstimatedDurationMin = route.EstimatedDurationMin.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Route route)
    {
        var entity = await _context.Routes.FindAsync(route.Id.Value);

        if (entity is null)
            return;

        _context.Routes.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(RouteId id)
    {
        return _context.Routes.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByOriginAndDestinationAsync(int originAirportId, int destinationAirportId, int? excludingId = null)
    {
        var query = _context.Routes
            .AsNoTracking()
            .Where(r => r.OriginAirportId == originAirportId && r.DestinationAirportId == destinationAirportId);

        if (excludingId.HasValue)
            query = query.Where(r => r.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static Route MapToDomain(RouteEntity entity)
    {
        try
        {
            return Route.Create(
                RouteId.Create(entity.Id),
                RouteAirportId.Create(entity.OriginAirportId),
                RouteAirportId.Create(entity.DestinationAirportId),
                RouteDistanceKm.Create(entity.DistanceKm),
                RouteEstimatedDurationMinutes.Create(entity.EstimatedDurationMin)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro routes(id={entity.Id}) tiene datos invalidos. " +
                $"aeropuerto_origen_id={entity.OriginAirportId}, aeropuerto_destino_id={entity.DestinationAirportId}, " +
                $"distancia_km={entity.DistanceKm}, duracion_estimada_min={entity.EstimatedDurationMin}.",
                ex);
        }
    }

    private static RouteEntity MapToEntity(Route route)
    {
        return new RouteEntity
        {
            Id = route.Id.Value,
            OriginAirportId = route.OriginAirportId.Value,
            DestinationAirportId = route.DestinationAirportId.Value,
            DistanceKm = route.DistanceKm.Value,
            EstimatedDurationMin = route.EstimatedDurationMin.Value
        };
    }
}

