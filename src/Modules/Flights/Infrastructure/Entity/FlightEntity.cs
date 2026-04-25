// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Infrastructure\Entity\FlightEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;

public class FlightEntity
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public int AirlineId { get; set; }
    public int RouteId { get; set; }
    public int AircraftId { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public DateTime EstimatedArrivalDateTime { get; set; }
    public int TotalCapacity { get; set; }
    public int AvailableSeats { get; set; }
    public int StateId { get; set; }
    public DateTime? RescheduledAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

