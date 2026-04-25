// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Sessions\Infrastructure\Entity\SessionEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Sessions.Infrastructure.Entity;

public class SessionEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string? IpAddress { get; set; }
    public bool IsActive { get; set; }
}
