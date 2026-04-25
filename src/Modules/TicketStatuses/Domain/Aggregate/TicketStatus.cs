// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\TicketStatuses\Domain\Aggregate\TicketStatus.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.TicketStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.TicketStatuses.Domain.Aggregate;

public class TicketStatus
{
    public TicketStatusId Id { get; private set; }
    public TicketStatusName Name { get; private set; }

    private TicketStatus(TicketStatusId id, TicketStatusName name)
    {
        Id = id;
        Name = name;
    }

    public static TicketStatus Create(TicketStatusId id, TicketStatusName name)
    {
        return new TicketStatus(id, name);
    }

    public static TicketStatus CreateNew(TicketStatusName name)
    {
        return new TicketStatus(TicketStatusId.CreateEmpty(), name);
    }
}
