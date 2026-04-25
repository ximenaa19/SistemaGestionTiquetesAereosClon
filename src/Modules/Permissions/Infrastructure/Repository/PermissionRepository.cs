// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Permissions\Infrastructure\Repository\PermissionRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Permissions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Permissions.Domain.Repositories;
using GestionAerolineas.src.Modules.Permissions.Domain.ValueObject;
using GestionAerolineas.src.Modules.Permissions.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Permissions.Infrastructure.Repository;

public class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _context;

    public PermissionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Permission>> GetAllAsync()
    {
        var entities = await _context.Permissions
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Permission?> GetByIdAsync(PermissionId id)
    {
        var entity = await _context.Permissions
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Permission?> GetByNameAsync(PermissionName name)
    {
        var entity = await _context.Permissions
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(Permission permission)
    {
        await _context.Permissions.AddAsync(MapToEntity(permission));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Permission permission)
    {
        var existing = await _context.Permissions
            .FirstOrDefaultAsync(e => e.Id == permission.Id.Value);

        if (existing is null)
            return;

        existing.Name = permission.Name.Value;
        existing.Description = permission.Description.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Permission permission)
    {
        var entity = await _context.Permissions.FindAsync(permission.Id.Value);

        if (entity is null)
            return;

        _context.Permissions.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(PermissionId id)
    {
        return _context.Permissions.AnyAsync(e => e.Id == id.Value);
    }

    private static Permission MapToDomain(PermissionEntity entity)
    {
        return Permission.Create(
            PermissionId.Create(entity.Id),
            PermissionName.Create(entity.Name ?? string.Empty),
            PermissionDescription.Create(entity.Description)
        );
    }

    private static PermissionEntity MapToEntity(Permission permission)
    {
        return new PermissionEntity
        {
            Id = permission.Id.Value,
            Name = permission.Name.Value,
            Description = permission.Description.Value
        };
    }
}
