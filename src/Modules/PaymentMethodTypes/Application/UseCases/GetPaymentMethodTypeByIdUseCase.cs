// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethodTypes\Application\UseCases\GetPaymentMethodTypeByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentMethodTypes.Application.UseCases;

public class GetPaymentMethodTypeByIdUseCase
{
    private readonly IPaymentMethodTypeRepository _repository;

    public GetPaymentMethodTypeByIdUseCase(IPaymentMethodTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentMethodType?> ExecuteAsync(int id)
    {
        var idVO = PaymentMethodTypeId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}
