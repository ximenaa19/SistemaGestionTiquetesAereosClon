// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Infrastructure\Entity\RouteEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Routes.Infrastructure.Entity;

public class RouteEntity
{
    public int Id { get; set; }
    public int OriginAirportId { get; set; }
    public int DestinationAirportId { get; set; }
    public int? DistanceKm { get; set; }
    public int? EstimatedDurationMin { get; set; }
}

