// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Infrastructure\Repository\SeasonRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Seasons.Domain.Aggregate;
using GestionAerolineas.src.Modules.Seasons.Domain.Repositories;
using GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;
using GestionAerolineas.src.Modules.Seasons.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Seasons.Infrastructure.Repository;

public class SeasonRepository : ISeasonRepository
{
    private readonly AppDbContext _context;

    public SeasonRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Season>> GetAllAsync()
    {
        var entities = await _context.Seasons
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Season?> GetByIdAsync(SeasonId id)
    {
        var entity = await _context.Seasons
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Season?> GetByNameAsync(SeasonName name)
    {
        var entity = await _context.Seasons
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(Season season)
    {
        await _context.Seasons.AddAsync(MapToEntity(season));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Season season)
    {
        var existing = await _context.Seasons
            .FirstOrDefaultAsync(e => e.Id == season.Id.Value);

        if (existing is null)
            return;

        existing.Name = season.Name.Value;
        existing.Description = season.Description.Value;
        existing.PriceFactor = season.PriceFactor.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Season season)
    {
        var entity = await _context.Seasons.FindAsync(season.Id.Value);

        if (entity is null)
            return;

        _context.Seasons.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(SeasonId id)
    {
        return _context.Seasons.AnyAsync(e => e.Id == id.Value);
    }

    private static Season MapToDomain(SeasonEntity entity)
    {
        return Season.Create(
            SeasonId.Create(entity.Id),
            SeasonName.Create(entity.Name ?? string.Empty),
            SeasonDescription.Create(entity.Description),
            SeasonPriceFactor.Create(entity.PriceFactor)
        );
    }

    private static SeasonEntity MapToEntity(Season season)
    {
        return new SeasonEntity
        {
            Id = season.Id.Value,
            Name = season.Name.Value,
            Description = season.Description.Value,
            PriceFactor = season.PriceFactor.Value
        };
    }
}
