// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Domain\ValueObject\InvoiceItemReservationPassengerId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;

public sealed record InvoiceItemReservationPassengerId
{
    public int? Value { get; }

    private InvoiceItemReservationPassengerId(int? value)
    {
        Value = value;
    }

    public static InvoiceItemReservationPassengerId Create(int? value)
    {
        if (value.HasValue && value.Value <= 0)
            throw new ArgumentException("reserva_pasajero_id no es valido");
        return new InvoiceItemReservationPassengerId(value);
    }
}

