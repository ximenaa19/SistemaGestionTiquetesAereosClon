// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Infrastructure\Entity\UserEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Users.Infrastructure.Entity;

public class UserEntity
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
    public int? PersonId { get; set; }
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastAccess { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
