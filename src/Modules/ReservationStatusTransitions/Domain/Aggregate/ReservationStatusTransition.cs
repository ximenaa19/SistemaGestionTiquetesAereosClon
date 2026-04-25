// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\Domain\Aggregate\ReservationStatusTransition.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.Aggregate;

public class ReservationStatusTransition
{
    public ReservationStatusTransitionId Id { get; private set; }
    public ReservationStatusOriginId OriginStatusId { get; private set; }
    public ReservationStatusDestinationId DestinationStatusId { get; private set; }

    private ReservationStatusTransition(
        ReservationStatusTransitionId id,
        ReservationStatusOriginId originStatusId,
        ReservationStatusDestinationId destinationStatusId)
    {
        Id = id;
        OriginStatusId = originStatusId;
        DestinationStatusId = destinationStatusId;
    }

    public static ReservationStatusTransition Create(
        ReservationStatusTransitionId id,
        ReservationStatusOriginId originStatusId,
        ReservationStatusDestinationId destinationStatusId)
    {
        return new ReservationStatusTransition(id, originStatusId, destinationStatusId);
    }

    public static ReservationStatusTransition CreateNew(
        ReservationStatusOriginId originStatusId,
        ReservationStatusDestinationId destinationStatusId)
    {
        return new ReservationStatusTransition(
            ReservationStatusTransitionId.CreateEmpty(),
            originStatusId,
            destinationStatusId
        );
    }
}
