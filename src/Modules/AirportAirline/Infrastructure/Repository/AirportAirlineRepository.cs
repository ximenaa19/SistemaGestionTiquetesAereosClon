// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Infrastructure\Repository\AirportAirlineRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AirportAirline.Domain.Aggregate;
using GestionAerolineas.src.Modules.AirportAirline.Domain.Repositories;
using GestionAerolineas.src.Modules.AirportAirline.Domain.ValueObject;
using GestionAerolineas.src.Modules.AirportAirline.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.AirportAirline.Infrastructure.Repository;

public class AirportAirlineRepository : IAirportAirlineRepository
{
    private readonly AppDbContext _context;

    public AirportAirlineRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AirportAirlineRelation>> GetAllAsync()
    {
        var entities = await _context.AirportAirlines.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<AirportAirlineRelation?> GetByIdAsync(AirportAirlineId id)
    {
        var entity = await _context.AirportAirlines
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<AirportAirlineRelation?> GetByAirportAndAirlineAsync(AirportAirlineAirportId airportId, AirportAirlineAirlineId airlineId)
    {
        var entity = await _context.AirportAirlines
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.AirportId == airportId.Value && e.AirlineId == airlineId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(AirportAirlineRelation relation)
    {
        await _context.AirportAirlines.AddAsync(MapToEntity(relation));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AirportAirlineRelation relation)
    {
        var existing = await _context.AirportAirlines
            .FirstOrDefaultAsync(e => e.Id == relation.Id.Value);

        if (existing is null)
            return;

        existing.AirportId = relation.AirportId.Value;
        existing.AirlineId = relation.AirlineId.Value;
        existing.Terminal = relation.Terminal.Value;
        existing.StartDate = relation.StartDate.Value;
        existing.EndDate = relation.EndDate.Value;
        existing.IsActive = relation.IsActive.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(AirportAirlineRelation relation)
    {
        var entity = await _context.AirportAirlines.FindAsync(relation.Id.Value);

        if (entity is null)
            return;

        _context.AirportAirlines.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(AirportAirlineId id)
    {
        return _context.AirportAirlines.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByAirportAndAirlineAsync(int airportId, int airlineId, int? excludingId = null)
    {
        var query = _context.AirportAirlines
            .AsNoTracking()
            .Where(x => x.AirportId == airportId && x.AirlineId == airlineId);

        if (excludingId.HasValue)
            query = query.Where(x => x.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static AirportAirlineRelation MapToDomain(AirportAirlineEntity entity)
    {
        try
        {
            return AirportAirlineRelation.Create(
                AirportAirlineId.Create(entity.Id),
                AirportAirlineAirportId.Create(entity.AirportId),
                AirportAirlineAirlineId.Create(entity.AirlineId),
                AirportAirlineTerminal.Create(entity.Terminal),
                AirportAirlineStartDate.Create(entity.StartDate),
                AirportAirlineEndDate.Create(entity.EndDate),
                AirportAirlineIsActive.Create(entity.IsActive)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro airportairline(id={entity.Id}) tiene datos invalidos. " +
                $"aeropuerto_id={entity.AirportId}, aerolinea_id={entity.AirlineId}, terminal='{entity.Terminal}', " +
                $"fecha_inicio='{entity.StartDate}', fecha_fin='{entity.EndDate}', activa={entity.IsActive}.",
                ex);
        }
    }

    private static AirportAirlineEntity MapToEntity(AirportAirlineRelation relation)
    {
        return new AirportAirlineEntity
        {
            Id = relation.Id.Value,
            AirportId = relation.AirportId.Value,
            AirlineId = relation.AirlineId.Value,
            Terminal = relation.Terminal.Value,
            StartDate = relation.StartDate.Value,
            EndDate = relation.EndDate.Value,
            IsActive = relation.IsActive.Value
        };
    }
}

