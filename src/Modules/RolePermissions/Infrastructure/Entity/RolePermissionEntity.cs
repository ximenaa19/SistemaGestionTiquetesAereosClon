// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RolePermissions\Infrastructure\Entity\RolePermissionEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.RolePermissions.Infrastructure.Entity;

public class RolePermissionEntity
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
}

