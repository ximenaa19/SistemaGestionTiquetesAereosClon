// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\Infrastructure\Entity\FlightAssignmentEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.FlightAssignments.Infrastructure.Entity;

public class FlightAssignmentEntity
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public int StaffId { get; set; }
    public int FlightRoleId { get; set; }
}

