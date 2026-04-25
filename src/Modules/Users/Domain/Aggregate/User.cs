// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Domain\Aggregate\User.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Users.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Users.Domain.Aggregate;

public class User
{
    public UserId Id { get; private set; }
    public UserUsername Username { get; private set; }
    public UserPasswordHash PasswordHash { get; private set; }
    public UserPersonId PersonId { get; private set; }
    public UserRoleId RoleId { get; private set; }
    public UserIsActive IsActive { get; private set; }
    public UserLastAccess LastAccess { get; private set; }
    public UserCreatedAt CreatedAt { get; private set; }
    public UserUpdatedAt UpdatedAt { get; private set; }

    private User(
        UserId id,
        UserUsername username,
        UserPasswordHash passwordHash,
        UserPersonId personId,
        UserRoleId roleId,
        UserIsActive isActive,
        UserLastAccess lastAccess,
        UserCreatedAt createdAt,
        UserUpdatedAt updatedAt)
    {
        Id = id;
        Username = username;
        PasswordHash = passwordHash;
        PersonId = personId;
        RoleId = roleId;
        IsActive = isActive;
        LastAccess = lastAccess;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static User Create(
        UserId id,
        UserUsername username,
        UserPasswordHash passwordHash,
        UserPersonId personId,
        UserRoleId roleId,
        UserIsActive isActive,
        UserLastAccess lastAccess,
        UserCreatedAt createdAt,
        UserUpdatedAt updatedAt)
    {
        return new User(id, username, passwordHash, personId, roleId, isActive, lastAccess, createdAt, updatedAt);
    }

    public static User CreateNew(
        UserUsername username,
        UserPasswordHash passwordHash,
        UserPersonId personId,
        UserRoleId roleId,
        UserIsActive isActive,
        UserLastAccess lastAccess)
    {
        var now = DateTime.Now;

        return new User(
            UserId.CreateEmpty(),
            username,
            passwordHash,
            personId,
            roleId,
            isActive,
            lastAccess,
            UserCreatedAt.Create(now),
            UserUpdatedAt.Create(now));
    }
}
