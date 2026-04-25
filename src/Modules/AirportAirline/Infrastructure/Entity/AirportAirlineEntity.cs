// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Infrastructure\Entity\AirportAirlineEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.AirportAirline.Infrastructure.Entity;

public class AirportAirlineEntity
{
    public int Id { get; set; }
    public int AirportId { get; set; }
    public int AirlineId { get; set; }
    public string? Terminal { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}

