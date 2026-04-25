// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\Application\UseCases\GetPaymentMethodByCommercialNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethods.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentMethods.Application.UseCases;

public class GetPaymentMethodByCommercialNameUseCase
{
    private readonly IPaymentMethodRepository _repository;

    public GetPaymentMethodByCommercialNameUseCase(IPaymentMethodRepository repository)
    {
        _repository = repository;
    }

    public Task<PaymentMethod?> ExecuteAsync(string commercialName)
    {
        var nameVO = PaymentMethodCommercialName.Create(commercialName);
        return _repository.GetByCommercialNameAsync(nameVO);
    }
}

