// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftManufacturers\Infrastructure\Repository\AircraftManufacturerRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.Aggregate;
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.Repositories;
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.ValueObject;
using GestionAerolineas.src.Modules.AircraftManufacturers.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.AircraftManufacturers.Infrastructure.Repository;

public class AircraftManufacturerRepository : IAircraftManufacturerRepository
{
    private readonly AppDbContext _context;

    public AircraftManufacturerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AircraftManufacturer>> GetAllAsync()
    {
        var entities = await _context.AircraftManufacturers.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<AircraftManufacturer?> GetByIdAsync(AircraftManufacturerId id)
    {
        var entity = await _context.AircraftManufacturers
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<AircraftManufacturer?> GetByNameAsync(AircraftManufacturerName name)
    {
        var entity = await _context.AircraftManufacturers
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(AircraftManufacturer manufacturer)
    {
        await _context.AircraftManufacturers.AddAsync(MapToEntity(manufacturer));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AircraftManufacturer manufacturer)
    {
        var existing = await _context.AircraftManufacturers
            .FirstOrDefaultAsync(e => e.Id == manufacturer.Id.Value);

        if (existing is null)
            return;

        existing.Name = manufacturer.Name.Value;
        existing.CountryId = manufacturer.CountryId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(AircraftManufacturer manufacturer)
    {
        var entity = await _context.AircraftManufacturers.FindAsync(manufacturer.Id.Value);

        if (entity is null)
            return;

        _context.AircraftManufacturers.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(AircraftManufacturerId id)
    {
        return _context.AircraftManufacturers.AnyAsync(e => e.Id == id.Value);
    }

    private static AircraftManufacturer MapToDomain(AircraftManufacturerEntity entity)
    {
        return AircraftManufacturer.Create(
            AircraftManufacturerId.Create(entity.Id),
            AircraftManufacturerName.Create(entity.Name ?? string.Empty),
            AircraftManufacturerCountryId.Create(entity.CountryId)
        );
    }

    private static AircraftManufacturerEntity MapToEntity(AircraftManufacturer manufacturer)
    {
        return new AircraftManufacturerEntity
        {
            Id = manufacturer.Id.Value,
            Name = manufacturer.Name.Value,
            CountryId = manufacturer.CountryId.Value
        };
    }
}

