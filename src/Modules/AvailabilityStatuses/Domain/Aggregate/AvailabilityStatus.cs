// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AvailabilityStatuses\Domain\Aggregate\AvailabilityStatus.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.Aggregate;

public class AvailabilityStatus
{
    public AvailabilityStatusId Id { get; private set; }
    public AvailabilityStatusName Name { get; private set; }

    private AvailabilityStatus(AvailabilityStatusId id, AvailabilityStatusName name)
    {
        Id = id;
        Name = name;
    }

    public static AvailabilityStatus Create(AvailabilityStatusId id, AvailabilityStatusName name)
    {
        return new AvailabilityStatus(id, name);
    }

    public static AvailabilityStatus CreateNew(AvailabilityStatusName name)
    {
        return new AvailabilityStatus(AvailabilityStatusId.CreateEmpty(), name);
    }
}
