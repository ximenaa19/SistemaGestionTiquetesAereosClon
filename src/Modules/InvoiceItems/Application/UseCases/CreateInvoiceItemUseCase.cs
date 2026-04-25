// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Application\UseCases\CreateInvoiceItemUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItems.Application.Interfaces;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Repositories;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItems.Application.UseCases;

public class CreateInvoiceItemUseCase
{
    private readonly IInvoiceItemRepository _repository;
    private readonly IInvoiceItemValidator _validator;

    public CreateInvoiceItemUseCase(IInvoiceItemRepository repository, IInvoiceItemValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(
        int invoiceId,
        int itemTypeId,
        string description,
        int quantity,
        decimal unitPrice,
        int? reservationPassengerId)
    {
        var invoiceIdVO = InvoiceItemInvoiceId.Create(invoiceId);
        var itemTypeIdVO = InvoiceItemTypeId.Create(itemTypeId);
        var descriptionVO = InvoiceItemDescription.Create(description);
        var quantityVO = InvoiceItemQuantity.Create(quantity);
        var unitPriceVO = InvoiceItemUnitPrice.Create(unitPrice);
        var reservationPassengerVO = InvoiceItemReservationPassengerId.Create(reservationPassengerId);

        await _validator.ValidateInvoiceExistsAsync(invoiceIdVO);
        await _validator.ValidateItemTypeExistsAsync(itemTypeIdVO);
        await _validator.ValidateReservationPassengerAsync(invoiceIdVO, reservationPassengerVO);

        var entity = InvoiceItem.CreateNew(invoiceIdVO, itemTypeIdVO, descriptionVO, quantityVO, unitPriceVO, reservationPassengerVO);
        await _repository.AddAsync(entity);
    }
}

