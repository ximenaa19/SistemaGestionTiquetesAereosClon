// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Domain\ValueObject\InvoiceReservationId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;

public record InvoiceReservationId(int Value)
{
    public static InvoiceReservationId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("reserva_id no es valido");
        return new InvoiceReservationId(value);
    }
}

