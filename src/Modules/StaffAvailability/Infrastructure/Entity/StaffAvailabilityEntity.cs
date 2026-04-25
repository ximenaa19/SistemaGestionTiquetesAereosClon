// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Infrastructure\Entity\StaffAvailabilityEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.StaffAvailability.Infrastructure.Entity;

public class StaffAvailabilityEntity
{
    public int Id { get; set; }
    public int StaffId { get; set; }
    public int AvailabilityStatusId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Observation { get; set; }
}

