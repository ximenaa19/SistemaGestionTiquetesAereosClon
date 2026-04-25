// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Application\Interfaces\IUserValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Users.Domain.Aggregate;
using GestionAerolineas.src.Modules.Users.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Users.Application.Interfaces;

public interface IUserValidator
{
    Task ValidateUsernameAsync(UserUsername username, UserId? currentId = null);
    Task ValidatePersonExistsAsync(UserPersonId personId);
    Task ValidatePersonIsUniqueAsync(UserPersonId personId, UserId? currentId = null);
    Task ValidateRoleExistsAsync(UserRoleId roleId);
    Task ValidateCanDeactivateAsync(User existingUser, UserIsActive newIsActive, string? actingUsername);
}
