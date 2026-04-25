// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Application\Interfaces\IInvoiceItemValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItems.Application.Interfaces;

public interface IInvoiceItemValidator
{
    Task ValidateInvoiceExistsAsync(InvoiceItemInvoiceId invoiceId);
    Task ValidateItemTypeExistsAsync(InvoiceItemTypeId itemTypeId);
    Task ValidateReservationPassengerAsync(InvoiceItemInvoiceId invoiceId, InvoiceItemReservationPassengerId reservationPassengerId);
}

