// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\Application\Services\PaymentMethodValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethods.Application.Interfaces;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentMethods.Application.Services;

public class PaymentMethodValidator : IPaymentMethodValidator
{
    private readonly IPaymentMethodRepository _repository;

    public PaymentMethodValidator(IPaymentMethodRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateCommercialNameAsync(PaymentMethodCommercialName commercialName, PaymentMethodId? currentId = null)
    {
        var normalizedCandidate = PaymentMethodCommercialName.Normalize(commercialName.Value);
        var all = await _repository.GetAllAsync();

        foreach (var item in all)
        {
            if (currentId != null && item.Id.Value == currentId.Value)
                continue;

            if (PaymentMethodCommercialName.Normalize(item.CommercialName.Value) == normalizedCandidate)
                throw new Exception("Ya existe un método de pago con ese nombre comercial");
        }
    }
}

