// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SystemRoles\Infrastructure\Repository\SystemRoleRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SystemRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.SystemRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.SystemRoles.Domain.ValueObject;
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Repository;

public class SystemRoleRepository : ISystemRoleRepository
{
    private readonly AppDbContext _context;

    public SystemRoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SystemRole>> GetAllAsync()
    {
        var entities = await _context.SystemRoles
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<SystemRole?> GetByIdAsync(SystemRoleId id)
    {
        var entity = await _context.SystemRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<SystemRole?> GetByNameAsync(SystemRoleName name)
    {
        var entity = await _context.SystemRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(SystemRole systemRole)
    {
        await _context.SystemRoles.AddAsync(MapToEntity(systemRole));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SystemRole systemRole)
    {
        var existing = await _context.SystemRoles
            .FirstOrDefaultAsync(e => e.Id == systemRole.Id.Value);

        if (existing is null)
            return;

        existing.Name = systemRole.Name.Value;
        existing.Description = systemRole.Description.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(SystemRole systemRole)
    {
        var entity = await _context.SystemRoles.FindAsync(systemRole.Id.Value);

        if (entity is null)
            return;

        _context.SystemRoles.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(SystemRoleId id)
    {
        return _context.SystemRoles.AnyAsync(e => e.Id == id.Value);
    }

    private static SystemRole MapToDomain(SystemRoleEntity entity)
    {
        return SystemRole.Create(
            SystemRoleId.Create(entity.Id),
            SystemRoleName.Create(entity.Name ?? string.Empty),
            SystemRoleDescription.Create(entity.Description)
        );
    }

    private static SystemRoleEntity MapToEntity(SystemRole systemRole)
    {
        return new SystemRoleEntity
        {
            Id = systemRole.Id.Value,
            Name = systemRole.Name.Value,
            Description = systemRole.Description.Value
        };
    }
}
