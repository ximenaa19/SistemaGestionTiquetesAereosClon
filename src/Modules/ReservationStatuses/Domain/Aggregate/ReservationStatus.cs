// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatuses\Domain\Aggregate\ReservationStatus.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatuses.Domain.Aggregate;

public class ReservationStatus
{
    public ReservationStatusId Id { get; private set; }
    public ReservationStatusName Name { get; private set; }

    private ReservationStatus(ReservationStatusId id, ReservationStatusName name)
    {
        Id = id;
        Name = name;
    }

    public static ReservationStatus Create(ReservationStatusId id, ReservationStatusName name)
    {
        return new ReservationStatus(id, name);
    }

    public static ReservationStatus CreateNew(ReservationStatusName name)
    {
        return new ReservationStatus(ReservationStatusId.CreateEmpty(), name);
    }
}

