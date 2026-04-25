// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Application\Interfaces\IInvoiceValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Invoices.Application.Interfaces;

public interface IInvoiceValidator
{
    Task ValidateReservationExistsAsync(InvoiceReservationId reservationId);
    Task ValidateReservationAllowsInvoiceAsync(InvoiceReservationId reservationId);
    Task ValidateReservationIsUniqueAsync(InvoiceReservationId reservationId, InvoiceId? excludingInvoiceId = null);
    Task ValidateInvoiceNumberUniqueAsync(InvoiceNumber number, InvoiceId? excludingInvoiceId = null);
    Task ValidateDeletableAsync(InvoiceId invoiceId);
}

