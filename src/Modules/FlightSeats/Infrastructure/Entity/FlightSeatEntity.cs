// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Infrastructure\Entity\FlightSeatEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.FlightSeats.Infrastructure.Entity;

public class FlightSeatEntity
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public string? SeatCode { get; set; }
    public int CabinTypeId { get; set; }
    public int LocationTypeId { get; set; }
    public bool IsOccupied { get; set; }
}

