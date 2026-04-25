// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\Infrastructure\Entity\ReservationStatusTransitionEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.Infrastructure.Entity;

public class ReservationStatusTransitionEntity
{
    public int Id { get; set; }
    public int OriginStatusId { get; set; }
    public int DestinationStatusId { get; set; }
}
