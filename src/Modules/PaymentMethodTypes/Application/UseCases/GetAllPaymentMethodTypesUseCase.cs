// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethodTypes\Application\UseCases\GetAllPaymentMethodTypesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Repositories;

namespace GestionAerolineas.src.Modules.PaymentMethodTypes.Application.UseCases;

public class GetAllPaymentMethodTypesUseCase
{
    private readonly IPaymentMethodTypeRepository _repository;

    public GetAllPaymentMethodTypesUseCase(IPaymentMethodTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PaymentMethodType>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
