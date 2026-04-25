// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\Infrastructure\Entity\ReservationFlightEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Entity;

public class ReservationFlightEntity
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public int FlightId { get; set; }
    public decimal PartialAmount { get; set; }
}

