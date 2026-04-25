// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CheckinStatuses\Domain\Aggregate\CheckinStatus.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CheckinStatuses.Domain.Aggregate;

public class CheckinStatus
{
    public CheckinStatusId Id { get; private set; }
    public CheckinStatusName Name { get; private set; }

    private CheckinStatus(CheckinStatusId id, CheckinStatusName name)
    {
        Id = id;
        Name = name;
    }

    public static CheckinStatus Create(CheckinStatusId id, CheckinStatusName name)
    {
        return new CheckinStatus(id, name);
    }

    public static CheckinStatus CreateNew(CheckinStatusName name)
    {
        return new CheckinStatus(CheckinStatusId.CreateEmpty(), name);
    }
}
