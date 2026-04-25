// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Domain\Aggregate\InvoiceItem.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItems.Domain.Aggregate;

public class InvoiceItem
{
    public InvoiceItemId Id { get; private set; }
    public InvoiceItemInvoiceId InvoiceId { get; private set; }
    public InvoiceItemTypeId ItemTypeId { get; private set; }
    public InvoiceItemDescription Description { get; private set; }
    public InvoiceItemQuantity Quantity { get; private set; }
    public InvoiceItemUnitPrice UnitPrice { get; private set; }
    public InvoiceItemSubtotal Subtotal { get; private set; }
    public InvoiceItemReservationPassengerId ReservationPassengerId { get; private set; }

    private InvoiceItem(
        InvoiceItemId id,
        InvoiceItemInvoiceId invoiceId,
        InvoiceItemTypeId itemTypeId,
        InvoiceItemDescription description,
        InvoiceItemQuantity quantity,
        InvoiceItemUnitPrice unitPrice,
        InvoiceItemSubtotal subtotal,
        InvoiceItemReservationPassengerId reservationPassengerId)
    {
        Id = id;
        InvoiceId = invoiceId;
        ItemTypeId = itemTypeId;
        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Subtotal = subtotal;
        ReservationPassengerId = reservationPassengerId;
    }

    public static InvoiceItem Create(
        InvoiceItemId id,
        InvoiceItemInvoiceId invoiceId,
        InvoiceItemTypeId itemTypeId,
        InvoiceItemDescription description,
        InvoiceItemQuantity quantity,
        InvoiceItemUnitPrice unitPrice,
        InvoiceItemSubtotal subtotal,
        InvoiceItemReservationPassengerId reservationPassengerId)
    {
        return new InvoiceItem(id, invoiceId, itemTypeId, description, quantity, unitPrice, subtotal, reservationPassengerId);
    }

    public static InvoiceItem CreateNew(
        InvoiceItemInvoiceId invoiceId,
        InvoiceItemTypeId itemTypeId,
        InvoiceItemDescription description,
        InvoiceItemQuantity quantity,
        InvoiceItemUnitPrice unitPrice,
        InvoiceItemReservationPassengerId reservationPassengerId)
    {
        var subtotal = InvoiceItemSubtotal.Create(quantity.Value * unitPrice.Value);
        return new InvoiceItem(InvoiceItemId.CreateEmpty(), invoiceId, itemTypeId, description, quantity, unitPrice, subtotal, reservationPassengerId);
    }
}

