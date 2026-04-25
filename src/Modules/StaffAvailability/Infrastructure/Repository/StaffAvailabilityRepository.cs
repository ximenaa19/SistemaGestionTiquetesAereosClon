// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Infrastructure\Repository\StaffAvailabilityRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffAvailability.Domain.Aggregate;
using GestionAerolineas.src.Modules.StaffAvailability.Domain.Repositories;
using GestionAerolineas.src.Modules.StaffAvailability.Domain.ValueObject;
using GestionAerolineas.src.Modules.StaffAvailability.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.StaffAvailability.Infrastructure.Repository;

public class StaffAvailabilityRepository : IStaffAvailabilityRepository
{
    private readonly AppDbContext _context;

    public StaffAvailabilityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StaffAvailabilityBlock>> GetAllAsync()
    {
        var entities = await _context.StaffAvailability
            .AsNoTracking()
            .OrderBy(e => e.StaffId)
            .ThenBy(e => e.StartDateTime)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<StaffAvailabilityBlock?> GetByIdAsync(StaffAvailabilityId id)
    {
        var entity = await _context.StaffAvailability
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<StaffAvailabilityBlock>> GetByStaffIdAsync(StaffAvailabilityStaffId staffId)
    {
        var entities = await _context.StaffAvailability
            .AsNoTracking()
            .Where(e => e.StaffId == staffId.Value)
            .OrderBy(e => e.StartDateTime)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<StaffAvailabilityBlock>> GetByStatusIdAsync(StaffAvailabilityStatusId statusId)
    {
        var entities = await _context.StaffAvailability
            .AsNoTracking()
            .Where(e => e.AvailabilityStatusId == statusId.Value)
            .OrderBy(e => e.StaffId)
            .ThenBy(e => e.StartDateTime)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<StaffAvailabilityBlock?> GetActiveNowByStaffIdAsync(StaffAvailabilityStaffId staffId, DateTime now)
    {
        var entity = await _context.StaffAvailability
            .AsNoTracking()
            .Where(e => e.StaffId == staffId.Value && e.StartDateTime <= now && now < e.EndDateTime)
            .OrderByDescending(e => e.StartDateTime)
            .FirstOrDefaultAsync();

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(StaffAvailabilityBlock block)
    {
        await _context.StaffAvailability.AddAsync(MapToEntity(block));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StaffAvailabilityBlock block)
    {
        var existing = await _context.StaffAvailability
            .FirstOrDefaultAsync(e => e.Id == block.Id.Value);

        if (existing is null)
            return;

        existing.StaffId = block.StaffId.Value;
        existing.AvailabilityStatusId = block.StatusId.Value;
        existing.StartDateTime = block.StartDateTime.Value;
        existing.EndDateTime = block.EndDateTime.Value;
        existing.Observation = block.Observation.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(StaffAvailabilityBlock block)
    {
        var entity = await _context.StaffAvailability.FindAsync(block.Id.Value);
        if (entity is null)
            return;

        _context.StaffAvailability.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(StaffAvailabilityId id)
    {
        return _context.StaffAvailability.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsOverlapAsync(int staffId, DateTime start, DateTime end, int? excludingId = null)
    {
        var query = _context.StaffAvailability
            .AsNoTracking()
            .Where(e => e.StaffId == staffId && e.StartDateTime < end && e.EndDateTime > start);

        if (excludingId.HasValue)
            query = query.Where(e => e.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static StaffAvailabilityBlock MapToDomain(StaffAvailabilityEntity entity)
    {
        try
        {
            return StaffAvailabilityBlock.Create(
                StaffAvailabilityId.Create(entity.Id),
                StaffAvailabilityStaffId.Create(entity.StaffId),
                StaffAvailabilityStatusId.Create(entity.AvailabilityStatusId),
                StaffAvailabilityStartDateTime.Create(entity.StartDateTime),
                StaffAvailabilityEndDateTime.Create(entity.EndDateTime),
                StaffAvailabilityObservation.Create(entity.Observation)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro staffavailability(id={entity.Id}) tiene datos invalidos. " +
                $"personal_id={entity.StaffId}, estado_disponibilidad_id={entity.AvailabilityStatusId}, " +
                $"fecha_inicio={entity.StartDateTime:o}, fecha_fin={entity.EndDateTime:o}, observacion='{entity.Observation}'.",
                ex);
        }
    }

    private static StaffAvailabilityEntity MapToEntity(StaffAvailabilityBlock block)
    {
        return new StaffAvailabilityEntity
        {
            Id = block.Id.Value,
            StaffId = block.StaffId.Value,
            AvailabilityStatusId = block.StatusId.Value,
            StartDateTime = block.StartDateTime.Value,
            EndDateTime = block.EndDateTime.Value,
            Observation = block.Observation.Value
        };
    }
}
