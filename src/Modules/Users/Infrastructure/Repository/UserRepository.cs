// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Infrastructure\Repository\UserRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Users.Domain.Aggregate;
using GestionAerolineas.src.Modules.Users.Domain.Repositories;
using GestionAerolineas.src.Modules.Users.Domain.ValueObject;
using GestionAerolineas.src.Modules.Users.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Users.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var entities = await _context.Users
            .AsNoTracking()
            .OrderBy(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<User?> GetByIdAsync(UserId id)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<User?> GetByUsernameAsync(UserUsername username)
    {
        var normalized = UserUsername.Normalize(username.Value);

        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Username != null && e.Username.Trim().ToUpper() == normalized);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<User>> GetByRoleIdAsync(UserRoleId roleId)
    {
        var entities = await _context.Users
            .AsNoTracking()
            .Where(e => e.RoleId == roleId.Value)
            .OrderBy(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(MapToEntity(user));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        var existing = await _context.Users
            .FirstOrDefaultAsync(e => e.Id == user.Id.Value);

        if (existing is null)
            return;

        existing.Username = user.Username.Value;
        existing.PasswordHash = user.PasswordHash.Value;
        existing.PersonId = user.PersonId.Value;
        existing.RoleId = user.RoleId.Value;
        existing.IsActive = user.IsActive.Value;
        existing.LastAccess = user.LastAccess.Value;
        existing.UpdatedAt = user.UpdatedAt.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        var entity = await _context.Users.FindAsync(user.Id.Value);
        if (entity is null)
            return;

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(UserId id)
    {
        return _context.Users.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByNormalizedUsernameAsync(string normalizedUsername, int? excludingId = null)
    {
        var query = _context.Users
            .AsNoTracking()
            .Where(u => u.Username != null);

        if (excludingId.HasValue)
            query = query.Where(u => u.Id != excludingId.Value);

        return query.AnyAsync(u => u.Username!.Trim().ToUpper() == normalizedUsername);
    }

    public Task<bool> ExistsByPersonIdAsync(int personId, int? excludingId = null)
    {
        var query = _context.Users
            .AsNoTracking()
            .Where(u => u.PersonId == personId);

        if (excludingId.HasValue)
            query = query.Where(u => u.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static User MapToDomain(UserEntity entity)
    {
        try
        {
            return User.Create(
                UserId.Create(entity.Id),
                UserUsername.Create(entity.Username ?? string.Empty),
                UserPasswordHash.Create(entity.PasswordHash ?? string.Empty),
                UserPersonId.Create(entity.PersonId),
                UserRoleId.Create(entity.RoleId),
                UserIsActive.Create(entity.IsActive),
                UserLastAccess.Create(entity.LastAccess),
                UserCreatedAt.Create(entity.CreatedAt),
                UserUpdatedAt.Create(entity.UpdatedAt));
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro users(id={entity.Id}) tiene datos invalidos. " +
                $"username='{entity.Username}', persona_id={entity.PersonId}, rol_id={entity.RoleId}, activo={entity.IsActive}, " +
                $"ultimo_acceso='{entity.LastAccess}', creado_en='{entity.CreatedAt}', actualizado_en='{entity.UpdatedAt}'.",
                ex);
        }
    }

    private static UserEntity MapToEntity(User user)
    {
        return new UserEntity
        {
            Id = user.Id.Value,
            Username = user.Username.Value,
            PasswordHash = user.PasswordHash.Value,
            PersonId = user.PersonId.Value,
            RoleId = user.RoleId.Value,
            IsActive = user.IsActive.Value,
            LastAccess = user.LastAccess.Value,
            CreatedAt = user.CreatedAt.Value,
            UpdatedAt = user.UpdatedAt.Value
        };
    }


}
