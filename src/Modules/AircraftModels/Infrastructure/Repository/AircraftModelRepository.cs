// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\Infrastructure\Repository\AircraftModelRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftModels.Domain.Aggregate;
using GestionAerolineas.src.Modules.AircraftModels.Domain.Repositories;
using GestionAerolineas.src.Modules.AircraftModels.Domain.ValueObject;
using GestionAerolineas.src.Modules.AircraftModels.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.AircraftModels.Infrastructure.Repository;

public class AircraftModelRepository : IAircraftModelRepository
{
    private readonly AppDbContext _context;

    public AircraftModelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AircraftModel>> GetAllAsync()
    {
        var entities = await _context.AircraftModels.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<AircraftModel?> GetByIdAsync(AircraftModelId id)
    {
        var entity = await _context.AircraftModels
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<AircraftModel?> GetByNameAsync(AircraftModelName modelName)
    {
        var entity = await _context.AircraftModels
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ModelName == modelName.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(AircraftModel aircraftModel)
    {
        await _context.AircraftModels.AddAsync(MapToEntity(aircraftModel));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AircraftModel aircraftModel)
    {
        var existing = await _context.AircraftModels
            .FirstOrDefaultAsync(e => e.Id == aircraftModel.Id.Value);

        if (existing is null)
            return;

        existing.ManufacturerId = aircraftModel.ManufacturerId.Value;
        existing.ModelName = aircraftModel.ModelName.Value;
        existing.MaxCapacity = aircraftModel.MaxCapacity.Value;
        existing.MaxTakeoffWeightKg = aircraftModel.MaxTakeoffWeightKg;
        existing.FuelConsumptionKgPerHour = aircraftModel.FuelConsumptionKgPerHour;
        existing.CruiseSpeedKmh = aircraftModel.CruiseSpeedKmh;
        existing.CruiseAltitudeFt = aircraftModel.CruiseAltitudeFt;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(AircraftModel aircraftModel)
    {
        var entity = await _context.AircraftModels.FindAsync(aircraftModel.Id.Value);

        if (entity is null)
            return;

        _context.AircraftModels.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(AircraftModelId id)
    {
        return _context.AircraftModels.AnyAsync(e => e.Id == id.Value);
    }

    private static AircraftModel MapToDomain(AircraftModelEntity entity)
    {
        return AircraftModel.Create(
            AircraftModelId.Create(entity.Id),
            AircraftManufacturerId.Create(entity.ManufacturerId),
            AircraftModelName.Create(entity.ModelName ?? string.Empty),
            AircraftModelMaxCapacity.Create(entity.MaxCapacity),
            entity.MaxTakeoffWeightKg,
            entity.FuelConsumptionKgPerHour,
            entity.CruiseSpeedKmh,
            entity.CruiseAltitudeFt
        );
    }

    private static AircraftModelEntity MapToEntity(AircraftModel aircraftModel)
    {
        return new AircraftModelEntity
        {
            Id = aircraftModel.Id.Value,
            ManufacturerId = aircraftModel.ManufacturerId.Value,
            ModelName = aircraftModel.ModelName.Value,
            MaxCapacity = aircraftModel.MaxCapacity.Value,
            MaxTakeoffWeightKg = aircraftModel.MaxTakeoffWeightKg,
            FuelConsumptionKgPerHour = aircraftModel.FuelConsumptionKgPerHour,
            CruiseSpeedKmh = aircraftModel.CruiseSpeedKmh,
            CruiseAltitudeFt = aircraftModel.CruiseAltitudeFt
        };
    }
}

