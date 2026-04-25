// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethodTypes\Application\UseCases\GetPaymentMethodTypeByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentMethodTypes.Application.UseCases;

public class GetPaymentMethodTypeByNameUseCase
{
    private readonly IPaymentMethodTypeRepository _repository;

    public GetPaymentMethodTypeByNameUseCase(IPaymentMethodTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentMethodType?> ExecuteAsync(string name)
    {
        var nameVO = PaymentMethodTypeName.Create(name);
        return await _repository.GetByNameAsync(nameVO);
    }
}
