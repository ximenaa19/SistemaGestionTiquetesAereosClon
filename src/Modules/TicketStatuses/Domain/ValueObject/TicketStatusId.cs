// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\TicketStatuses\Domain\ValueObject\TicketStatusId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.TicketStatuses.Domain.ValueObject;

public sealed record TicketStatusId
{
    public int Value { get; }

    private TicketStatusId(int value)
    {
        Value = value;
    }

    public static TicketStatusId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new TicketStatusId(value);
    }

    public static TicketStatusId CreateEmpty()
    {
        return new TicketStatusId(0);
    }
}
