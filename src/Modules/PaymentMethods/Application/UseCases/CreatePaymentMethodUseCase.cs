// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\Application\UseCases\CreatePaymentMethodUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethods.Application.Interfaces;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentMethods.Application.UseCases;

public class CreatePaymentMethodUseCase
{
    private readonly IPaymentMethodRepository _repository;
    private readonly IPaymentMethodValidator _validator;

    public CreatePaymentMethodUseCase(IPaymentMethodRepository repository, IPaymentMethodValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int paymentMethodTypeId, int? cardTypeId, int? cardIssuerId, string commercialName)
    {
        var paymentMethodTypeIdVO = PaymentMethodTypeId.Create(paymentMethodTypeId);
        var cardTypeIdVO = cardTypeId is null ? null : CardTypeId.Create(cardTypeId.Value);
        var cardIssuerIdVO = cardIssuerId is null ? null : CardIssuerId.Create(cardIssuerId.Value);
        var commercialNameVO = PaymentMethodCommercialName.Create(commercialName);

        await _validator.ValidateCommercialNameAsync(commercialNameVO);

        var entity = PaymentMethod.CreateNew(paymentMethodTypeIdVO, cardTypeIdVO, cardIssuerIdVO, commercialNameVO);

        await _repository.AddAsync(entity);
    }
}

