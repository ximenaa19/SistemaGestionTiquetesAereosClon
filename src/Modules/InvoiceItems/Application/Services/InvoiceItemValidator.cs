// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Application\Services\InvoiceItemValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.InvoiceItems.Application.Interfaces;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;
using GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;
using GestionAerolineas.src.Modules.Invoices.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.InvoiceItems.Application.Services;

public class InvoiceItemValidator : IInvoiceItemValidator
{
    private readonly InvoiceRepository _invoiceRepository;
    private readonly InvoiceItemTypeRepository _invoiceItemTypeRepository;
    private readonly ReservationPassengerRepository _reservationPassengerRepository;
    private readonly ReservationFlightRepository _reservationFlightRepository;

    public InvoiceItemValidator(
        InvoiceRepository invoiceRepository,
        InvoiceItemTypeRepository invoiceItemTypeRepository,
        ReservationPassengerRepository reservationPassengerRepository,
        ReservationFlightRepository reservationFlightRepository)
    {
        _invoiceRepository = invoiceRepository;
        _invoiceItemTypeRepository = invoiceItemTypeRepository;
        _reservationPassengerRepository = reservationPassengerRepository;
        _reservationFlightRepository = reservationFlightRepository;
    }

    public async Task ValidateInvoiceExistsAsync(InvoiceItemInvoiceId invoiceId)
    {
        var exists = await _invoiceRepository.ExistsAsync(InvoiceId.Create(invoiceId.Value));
        if (!exists)
            throw new Exception("La factura no existe");
    }

    public async Task ValidateItemTypeExistsAsync(InvoiceItemTypeId itemTypeId)
    {
        var exists = await _invoiceItemTypeRepository.ExistsAsync(
            GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.ValueObject.InvoiceItemTypeId.Create(itemTypeId.Value));
        if (!exists)
            throw new Exception("El tipo de item no existe");
    }

    public async Task ValidateReservationPassengerAsync(InvoiceItemInvoiceId invoiceId, InvoiceItemReservationPassengerId reservationPassengerId)
    {
        if (!reservationPassengerId.Value.HasValue)
            return;

        var invoice = await _invoiceRepository.GetByIdAsync(InvoiceId.Create(invoiceId.Value));
        if (invoice is null)
            throw new Exception("La factura no existe");

        var rpId = reservationPassengerId.Value.Value;
        var rp = await _reservationPassengerRepository.GetByIdAsync(ReservationPassengerId.Create(rpId));
        if (rp is null)
            throw new Exception("El reserva_pasajero_id no existe");

        var rf = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(rp.ReservationFlightId.Value));
        if (rf is null)
            throw new Exception("El reserva_vuelo del pasajero no existe");

        if (rf.ReservationId.Value != invoice.ReservationId.Value)
            throw new Exception("El pasajero no pertenece a la misma reserva de la factura");
    }
}

