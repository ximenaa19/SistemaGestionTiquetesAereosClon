// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethodTypes\Application\Services\PaymentMethodTypeValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethodTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentMethodTypes.Application.Services;

public class PaymentMethodTypeValidator : IPaymentMethodTypeValidator
{
    private readonly IPaymentMethodTypeRepository _repository;

    public PaymentMethodTypeValidator(IPaymentMethodTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateNameAsync(PaymentMethodTypeName name)
    {
        var existing = await _repository.GetByNameAsync(name);

        if (existing != null)
            throw new Exception("Ya existe un PaymentMethodType con ese nombre");
    }
}
