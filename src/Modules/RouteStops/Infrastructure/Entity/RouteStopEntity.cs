// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\Infrastructure\Entity\RouteStopEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.RouteStops.Infrastructure.Entity;

public class RouteStopEntity
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int StopAirportId { get; set; }
    public int Order { get; set; }
    public int DurationMinutes { get; set; }
}

