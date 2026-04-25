// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffRoles\Infrastructure\Repository\StaffRoleRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.StaffRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.StaffRoles.Domain.ValueObject;
using GestionAerolineas.src.Modules.StaffRoles.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.StaffRoles.Infrastructure.Repository;

public class StaffRoleRepository : IStaffRoleRepository
{
    private readonly AppDbContext _context;

    public StaffRoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StaffRole>> GetAllAsync()
    {
        var entities = await _context.StaffRoles
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<StaffRole?> GetByIdAsync(StaffRoleId id)
    {
        var entity = await _context.StaffRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<StaffRole?> GetByNameAsync(StaffRoleName name)
    {
        var entity = await _context.StaffRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(StaffRole staffRole)
    {
        await _context.StaffRoles.AddAsync(MapToEntity(staffRole));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StaffRole staffRole)
    {
        var existing = await _context.StaffRoles
            .FirstOrDefaultAsync(e => e.Id == staffRole.Id.Value);

        if (existing is null)
            return;

        existing.Name = staffRole.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(StaffRole staffRole)
    {
        var entity = await _context.StaffRoles.FindAsync(staffRole.Id.Value);

        if (entity is null)
            return;

        _context.StaffRoles.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(StaffRoleId id)
    {
        return _context.StaffRoles.AnyAsync(e => e.Id == id.Value);
    }

    private static StaffRole MapToDomain(StaffRoleEntity entity)
    {
        return StaffRole.Create(
            StaffRoleId.Create(entity.Id),
            StaffRoleName.Create(entity.Name ?? string.Empty)
        );
    }

    private static StaffRoleEntity MapToEntity(StaffRole staffRole)
    {
        return new StaffRoleEntity
        {
            Id = staffRole.Id.Value,
            Name = staffRole.Name.Value
        };
    }
}
