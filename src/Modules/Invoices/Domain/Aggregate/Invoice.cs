// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Domain\Aggregate\Invoice.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Invoices.Domain.Aggregate;

public class Invoice
{
    public InvoiceId Id { get; private set; }
    public InvoiceReservationId ReservationId { get; private set; }
    public InvoiceNumber Number { get; private set; }
    public InvoiceIssuedAt IssuedAt { get; private set; }
    public InvoiceSubtotal Subtotal { get; private set; }
    public InvoiceTaxes Taxes { get; private set; }
    public InvoiceTotal Total { get; private set; }
    public InvoiceCreatedAt CreatedAt { get; private set; }

    private Invoice(
        InvoiceId id,
        InvoiceReservationId reservationId,
        InvoiceNumber number,
        InvoiceIssuedAt issuedAt,
        InvoiceSubtotal subtotal,
        InvoiceTaxes taxes,
        InvoiceTotal total,
        InvoiceCreatedAt createdAt)
    {
        Id = id;
        ReservationId = reservationId;
        Number = number;
        IssuedAt = issuedAt;
        Subtotal = subtotal;
        Taxes = taxes;
        Total = total;
        CreatedAt = createdAt;
    }

    public static Invoice Create(
        InvoiceId id,
        InvoiceReservationId reservationId,
        InvoiceNumber number,
        InvoiceIssuedAt issuedAt,
        InvoiceSubtotal subtotal,
        InvoiceTaxes taxes,
        InvoiceTotal total,
        InvoiceCreatedAt createdAt)
    {
        return new Invoice(id, reservationId, number, issuedAt, subtotal, taxes, total, createdAt);
    }

    public static Invoice CreateNew(
        InvoiceReservationId reservationId,
        InvoiceNumber number,
        InvoiceIssuedAt issuedAt)
    {
        var subtotal = InvoiceSubtotal.Create(0);
        var taxes = InvoiceTaxes.Create(0);
        var total = InvoiceTotal.Create(0);

        return new Invoice(
            InvoiceId.CreateEmpty(),
            reservationId,
            number,
            issuedAt,
            subtotal,
            taxes,
            total,
            InvoiceCreatedAt.CreateOptional(null));
    }
}
