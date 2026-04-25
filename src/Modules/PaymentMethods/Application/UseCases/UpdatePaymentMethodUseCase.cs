// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\Application\UseCases\UpdatePaymentMethodUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethods.Application.Interfaces;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentMethods.Application.UseCases;

public class UpdatePaymentMethodUseCase
{
    private readonly IPaymentMethodRepository _repository;
    private readonly IPaymentMethodValidator _validator;

    public UpdatePaymentMethodUseCase(IPaymentMethodRepository repository, IPaymentMethodValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, int paymentMethodTypeId, int? cardTypeId, int? cardIssuerId, string commercialName)
    {
        var idVO = PaymentMethodId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is null)
            throw new Exception("El método de pago no existe");

        var paymentMethodTypeIdVO = PaymentMethodTypeId.Create(paymentMethodTypeId);
        var cardTypeIdVO = cardTypeId is null ? null : CardTypeId.Create(cardTypeId.Value);
        var cardIssuerIdVO = cardIssuerId is null ? null : CardIssuerId.Create(cardIssuerId.Value);
        var commercialNameVO = PaymentMethodCommercialName.Create(commercialName);

        await _validator.ValidateCommercialNameAsync(commercialNameVO, idVO);

        var updated = PaymentMethod.Create(idVO, paymentMethodTypeIdVO, cardTypeIdVO, cardIssuerIdVO, commercialNameVO);

        await _repository.UpdateAsync(updated);
    }
}

