// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Infrastructure\Repository\CabinConfigurationRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Aggregate;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Repositories;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.ValueObject;
using GestionAerolineas.src.Modules.CabinConfiguration.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.CabinConfiguration.Infrastructure.Repository;

public class CabinConfigurationRepository : ICabinConfigurationRepository
{
    private readonly AppDbContext _context;

    public CabinConfigurationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CabinConfigurationAggregate>> GetAllAsync()
    {
        var entities = await _context.CabinConfigurations.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<CabinConfigurationAggregate?> GetByIdAsync(CabinConfigurationId id)
    {
        var entity = await _context.CabinConfigurations
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<CabinConfigurationAggregate>> GetByAircraftIdAsync(CabinConfigurationAircraftId aircraftId)
    {
        var entities = await _context.CabinConfigurations
            .AsNoTracking()
            .Where(e => e.AircraftId == aircraftId.Value)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<CabinConfigurationAggregate?> GetByAircraftAndCabinTypeAsync(CabinConfigurationAircraftId aircraftId, CabinConfigurationCabinTypeId cabinTypeId)
    {
        var entity = await _context.CabinConfigurations
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.AircraftId == aircraftId.Value && e.CabinTypeId == cabinTypeId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(CabinConfigurationAggregate cabinConfiguration)
    {
        await _context.CabinConfigurations.AddAsync(MapToEntity(cabinConfiguration));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CabinConfigurationAggregate cabinConfiguration)
    {
        var existing = await _context.CabinConfigurations
            .FirstOrDefaultAsync(e => e.Id == cabinConfiguration.Id.Value);

        if (existing is null)
            return;

        existing.AircraftId = cabinConfiguration.AircraftId.Value;
        existing.CabinTypeId = cabinConfiguration.CabinTypeId.Value;
        existing.StartRow = cabinConfiguration.StartRow.Value;
        existing.EndRow = cabinConfiguration.EndRow.Value;
        existing.SeatsPerRow = cabinConfiguration.SeatsPerRow.Value;
        existing.SeatLetters = cabinConfiguration.SeatLetters.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CabinConfigurationAggregate cabinConfiguration)
    {
        var entity = await _context.CabinConfigurations.FindAsync(cabinConfiguration.Id.Value);

        if (entity is null)
            return;

        _context.CabinConfigurations.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(CabinConfigurationId id)
    {
        return _context.CabinConfigurations.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByAircraftAndCabinTypeAsync(int aircraftId, int cabinTypeId, int? excludingId = null)
    {
        var query = _context.CabinConfigurations
            .AsNoTracking()
            .Where(e => e.AircraftId == aircraftId && e.CabinTypeId == cabinTypeId);

        if (excludingId.HasValue)
            query = query.Where(e => e.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static CabinConfigurationAggregate MapToDomain(CabinConfigurationEntity entity)
    {
        try
        {
            var seatsPerRow = CabinConfigurationSeatsPerRow.Create(entity.SeatsPerRow);

            return CabinConfigurationAggregate.Create(
                CabinConfigurationId.Create(entity.Id),
                CabinConfigurationAircraftId.Create(entity.AircraftId),
                CabinConfigurationCabinTypeId.Create(entity.CabinTypeId),
                CabinConfigurationStartRow.Create(entity.StartRow),
                CabinConfigurationEndRow.Create(entity.EndRow),
                seatsPerRow,
                CabinConfigurationSeatLetters.Create(entity.SeatLetters ?? string.Empty, seatsPerRow.Value)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro cabinconfiguration(id={entity.Id}) tiene datos invalidos. " +
                $"aeronave_id={entity.AircraftId}, tipo_cabina_id={entity.CabinTypeId}, " +
                $"fila_inicio={entity.StartRow}, fila_fin={entity.EndRow}, " +
                $"asientos_por_fila={entity.SeatsPerRow}, letras_asientos='{entity.SeatLetters}'.",
                ex);
        }
    }

    private static CabinConfigurationEntity MapToEntity(CabinConfigurationAggregate cabinConfiguration)
    {
        return new CabinConfigurationEntity
        {
            Id = cabinConfiguration.Id.Value,
            AircraftId = cabinConfiguration.AircraftId.Value,
            CabinTypeId = cabinConfiguration.CabinTypeId.Value,
            StartRow = cabinConfiguration.StartRow.Value,
            EndRow = cabinConfiguration.EndRow.Value,
            SeatsPerRow = cabinConfiguration.SeatsPerRow.Value,
            SeatLetters = cabinConfiguration.SeatLetters.Value
        };
    }
}
