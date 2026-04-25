// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RolePermissions\Infrastructure\Repository\RolePermissionRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RolePermissions.Domain.Aggregate;
using GestionAerolineas.src.Modules.RolePermissions.Domain.Repositories;
using GestionAerolineas.src.Modules.RolePermissions.Domain.ValueObject;
using GestionAerolineas.src.Modules.RolePermissions.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.RolePermissions.Infrastructure.Repository;

public class RolePermissionRepository : IRolePermissionRepository
{
    private readonly AppDbContext _context;

    public RolePermissionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RolePermission>> GetAllAsync()
    {
        var entities = await _context.Set<RolePermissionEntity>()
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<RolePermission?> GetByIdAsync(RolePermissionId id)
    {
        var entity = await _context.Set<RolePermissionEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<RolePermission?> GetByPairAsync(SystemRoleId roleId, PermissionId permissionId)
    {
        var entity = await _context.Set<RolePermissionEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.RoleId == roleId.Value && e.PermissionId == permissionId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(RolePermission rolePermission)
    {
        await _context.Set<RolePermissionEntity>().AddAsync(MapToEntity(rolePermission));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(RolePermission rolePermission)
    {
        var existing = await _context.Set<RolePermissionEntity>()
            .FirstOrDefaultAsync(e => e.Id == rolePermission.Id.Value);

        if (existing is null)
            return;

        existing.RoleId = rolePermission.RoleId.Value;
        existing.PermissionId = rolePermission.PermissionId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(RolePermission rolePermission)
    {
        var entity = await _context.Set<RolePermissionEntity>().FindAsync(rolePermission.Id.Value);

        if (entity is null)
            return;

        _context.Set<RolePermissionEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(RolePermissionId id)
    {
        return _context.Set<RolePermissionEntity>().AnyAsync(e => e.Id == id.Value);
    }

    private static RolePermission MapToDomain(RolePermissionEntity entity)
    {
        return RolePermission.Create(
            RolePermissionId.Create(entity.Id),
            SystemRoleId.Create(entity.RoleId),
            PermissionId.Create(entity.PermissionId)
        );
    }

    private static RolePermissionEntity MapToEntity(RolePermission rolePermission)
    {
        return new RolePermissionEntity
        {
            Id = rolePermission.Id.Value,
            RoleId = rolePermission.RoleId.Value,
            PermissionId = rolePermission.PermissionId.Value
        };
    }
}

