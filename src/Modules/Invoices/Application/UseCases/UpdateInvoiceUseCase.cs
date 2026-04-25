// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Application\UseCases\UpdateInvoiceUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Invoices.Application.Interfaces;
using GestionAerolineas.src.Modules.Invoices.Domain.Aggregate;
using GestionAerolineas.src.Modules.Invoices.Domain.Repositories;
using GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Invoices.Application.UseCases;

public class UpdateInvoiceUseCase
{
    private readonly IInvoiceRepository _repository;
    private readonly IInvoiceValidator _validator;

    public UpdateInvoiceUseCase(IInvoiceRepository repository, IInvoiceValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, int reservationId, string number, DateTime issuedAt)
    {
        var idVO = InvoiceId.Create(id);
        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is null)
            throw new Exception("La factura no existe");

        var reservationIdVO = InvoiceReservationId.Create(reservationId);
        var numberVO = InvoiceNumber.Create(number);
        var issuedAtVO = InvoiceIssuedAt.Create(issuedAt);

        await _validator.ValidateReservationExistsAsync(reservationIdVO);
        await _validator.ValidateReservationIsUniqueAsync(reservationIdVO, idVO);
        await _validator.ValidateInvoiceNumberUniqueAsync(numberVO, idVO);

        var subtotal = existing.Subtotal;
        var taxes = InvoiceTaxes.Create(0);
        var total = InvoiceTotal.Create(subtotal.Value + taxes.Value);

        var updated = Invoice.Create(
            idVO,
            reservationIdVO,
            numberVO,
            issuedAtVO,
            subtotal,
            taxes,
            total,
            existing.CreatedAt);

        await _repository.UpdateAsync(updated);
    }
}

