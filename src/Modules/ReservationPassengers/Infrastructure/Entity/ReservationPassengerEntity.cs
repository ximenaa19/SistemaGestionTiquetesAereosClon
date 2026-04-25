// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Infrastructure\Entity\ReservationPassengerEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Entity;

public class ReservationPassengerEntity
{
    public int Id { get; set; }
    public int ReservationFlightId { get; set; }
    public int PassengerId { get; set; }
}

