// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Infrastructure\Entity\StaffEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Staff.Infrastructure.Entity;

public class StaffEntity
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public int RoleId { get; set; }
    public int? AirlineId { get; set; }
    public int? AirportId { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

