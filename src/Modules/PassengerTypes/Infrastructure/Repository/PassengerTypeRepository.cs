// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PassengerTypes\Infrastructure\Repository\PassengerTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PassengerTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.ValueObject;
using GestionAerolineas.src.Modules.PassengerTypes.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.PassengerTypes.Infrastructure.Repository;

public class PassengerTypeRepository : IPassengerTypeRepository
{
    private readonly AppDbContext _context;

    public PassengerTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PassengerType>> GetAllAsync()
    {
        var entities = await _context.PassengerTypes
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<PassengerType?> GetByIdAsync(PassengerTypeId id)
    {
        var entity = await _context.PassengerTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PassengerType?> GetByNameAsync(PassengerTypeName name)
    {
        var entity = await _context.PassengerTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(PassengerType passengerType)
    {
        await _context.PassengerTypes.AddAsync(MapToEntity(passengerType));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PassengerType passengerType)
    {
        var existing = await _context.PassengerTypes
            .FirstOrDefaultAsync(e => e.Id == passengerType.Id.Value);

        if (existing is null)
            return;

        existing.Name = passengerType.Name.Value;
        existing.AgeMin = passengerType.AgeMin;
        existing.AgeMax = passengerType.AgeMax;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PassengerType passengerType)
    {
        var entity = await _context.PassengerTypes.FindAsync(passengerType.Id.Value);

        if (entity is null)
            return;

        _context.PassengerTypes.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(PassengerTypeId id)
    {
        return _context.PassengerTypes.AnyAsync(e => e.Id == id.Value);
    }

    private static PassengerType MapToDomain(PassengerTypeEntity entity)
    {
        return PassengerType.Create(
            PassengerTypeId.Create(entity.Id),
            PassengerTypeName.Create(entity.Name ?? string.Empty),
            entity.AgeMin,
            entity.AgeMax
        );
    }

    private static PassengerTypeEntity MapToEntity(PassengerType passengerType)
    {
        return new PassengerTypeEntity
        {
            Id = passengerType.Id.Value,
            Name = passengerType.Name.Value,
            AgeMin = passengerType.AgeMin,
            AgeMax = passengerType.AgeMax
        };
    }
}
