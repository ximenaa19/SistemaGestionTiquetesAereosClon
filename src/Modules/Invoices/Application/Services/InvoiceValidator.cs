// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Application\Services\InvoiceValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Repositories;
using GestionAerolineas.src.Modules.Invoices.Application.Interfaces;
using GestionAerolineas.src.Modules.Invoices.Domain.Repositories;
using GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.Invoices.Application.Services;

public class InvoiceValidator : IInvoiceValidator
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ReservationRepository _reservationRepository;
    private readonly ReservationStatusRepository _reservationStatusRepository;
    private readonly IInvoiceItemRepository _invoiceItemRepository;

    public InvoiceValidator(
        IInvoiceRepository invoiceRepository,
        ReservationRepository reservationRepository,
        ReservationStatusRepository reservationStatusRepository,
        IInvoiceItemRepository invoiceItemRepository)
    {
        _invoiceRepository = invoiceRepository;
        _reservationRepository = reservationRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _invoiceItemRepository = invoiceItemRepository;
    }

    public async Task ValidateReservationExistsAsync(InvoiceReservationId reservationId)
    {
        var exists = await _reservationRepository.ExistsAsync(ReservationId.Create(reservationId.Value));
        if (!exists)
            throw new Exception("La reserva no existe");
    }

    public async Task ValidateReservationAllowsInvoiceAsync(InvoiceReservationId reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationId.Value));
        if (reservation is null)
            throw new Exception("La reserva no existe");

        var status = await _reservationStatusRepository.GetByIdAsync(
            GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject.ReservationStatusId.Create(reservation.StatusId.Value));

        var name = (status?.Name.Value ?? string.Empty).Trim().ToUpperInvariant();
        if (name == "CANCELADA" || name == "VENCIDA")
            throw new Exception($"No se puede facturar una reserva '{status!.Name.Value}'");
    }

    public async Task ValidateReservationIsUniqueAsync(InvoiceReservationId reservationId, InvoiceId? excludingInvoiceId = null)
    {
        var exists = await _invoiceRepository.ExistsByReservationIdAsync(reservationId.Value, excludingInvoiceId?.Value);
        if (exists)
            throw new Exception("Ya existe una factura para esta reserva");
    }

    public async Task ValidateInvoiceNumberUniqueAsync(InvoiceNumber number, InvoiceId? excludingInvoiceId = null)
    {
        var normalized = InvoiceNumber.Normalize(number.Value);
        var exists = await _invoiceRepository.ExistsByNormalizedNumberAsync(normalized, excludingInvoiceId?.Value);
        if (exists)
            throw new Exception("Ya existe una factura con ese numero");
    }

    public async Task ValidateDeletableAsync(InvoiceId invoiceId)
    {
        var anyItems = await _invoiceItemRepository.AnyByInvoiceIdAsync(invoiceId.Value);
        if (anyItems)
            throw new Exception("No se puede eliminar una factura con items");
    }
}

