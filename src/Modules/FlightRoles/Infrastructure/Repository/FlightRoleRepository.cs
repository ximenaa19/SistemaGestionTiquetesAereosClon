// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightRoles\Infrastructure\Repository\FlightRoleRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightRoles.Domain.ValueObject;
using GestionAerolineas.src.Modules.FlightRoles.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.FlightRoles.Infrastructure.Repository;

public class FlightRoleRepository : IFlightRoleRepository
{
    private readonly AppDbContext _context;

    public FlightRoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FlightRole>> GetAllAsync()
    {
        var entities = await _context.FlightRoles.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<FlightRole?> GetByIdAsync(FlightRoleId id)
    {
        var entity = await _context.FlightRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<FlightRole?> GetByNameAsync(FlightRoleName name)
    {
        var entity = await _context.FlightRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(FlightRole flightRole)
    {
        await _context.FlightRoles.AddAsync(MapToEntity(flightRole));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(FlightRole flightRole)
    {
        var existing = await _context.FlightRoles
            .FirstOrDefaultAsync(e => e.Id == flightRole.Id.Value);

        if (existing is null)
            return;

        existing.Name = flightRole.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(FlightRole flightRole)
    {
        var entity = await _context.FlightRoles.FindAsync(flightRole.Id.Value);

        if (entity is null)
            return;

        _context.FlightRoles.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(FlightRoleId id)
    {
        return await _context.FlightRoles.AnyAsync(e => e.Id == id.Value);
    }

    private static FlightRole MapToDomain(FlightRoleEntity entity)
    {
        return FlightRole.Create(
            FlightRoleId.Create(entity.Id),
            FlightRoleName.Create(entity.Name ?? string.Empty)
        );
    }

    private static FlightRoleEntity MapToEntity(FlightRole flightRole)
    {
        return new FlightRoleEntity
        {
            Id = flightRole.Id.Value,
            Name = flightRole.Name.Value
        };
    }
}

