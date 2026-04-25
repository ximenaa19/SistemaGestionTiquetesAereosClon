// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Domain\Aggregate\StaffAvailabilityBlock.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffAvailability.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.StaffAvailability.Domain.Aggregate;

public class StaffAvailabilityBlock
{
    public StaffAvailabilityId Id { get; private set; }
    public StaffAvailabilityStaffId StaffId { get; private set; }
    public StaffAvailabilityStatusId StatusId { get; private set; }
    public StaffAvailabilityStartDateTime StartDateTime { get; private set; }
    public StaffAvailabilityEndDateTime EndDateTime { get; private set; }
    public StaffAvailabilityObservation Observation { get; private set; }

    private StaffAvailabilityBlock(
        StaffAvailabilityId id,
        StaffAvailabilityStaffId staffId,
        StaffAvailabilityStatusId statusId,
        StaffAvailabilityStartDateTime startDateTime,
        StaffAvailabilityEndDateTime endDateTime,
        StaffAvailabilityObservation observation)
    {
        Id = id;
        StaffId = staffId;
        StatusId = statusId;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        Observation = observation;
    }

    public static StaffAvailabilityBlock Create(
        StaffAvailabilityId id,
        StaffAvailabilityStaffId staffId,
        StaffAvailabilityStatusId statusId,
        StaffAvailabilityStartDateTime startDateTime,
        StaffAvailabilityEndDateTime endDateTime,
        StaffAvailabilityObservation observation)
    {
        return new StaffAvailabilityBlock(id, staffId, statusId, startDateTime, endDateTime, observation);
    }

    public static StaffAvailabilityBlock CreateNew(
        StaffAvailabilityStaffId staffId,
        StaffAvailabilityStatusId statusId,
        StaffAvailabilityStartDateTime startDateTime,
        StaffAvailabilityEndDateTime endDateTime,
        StaffAvailabilityObservation observation)
    {
        return new StaffAvailabilityBlock(StaffAvailabilityId.CreateEmpty(), staffId, statusId, startDateTime, endDateTime, observation);
    }
}
