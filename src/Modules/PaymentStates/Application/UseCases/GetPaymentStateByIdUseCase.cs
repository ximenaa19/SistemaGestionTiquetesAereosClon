// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentStates\Application\UseCases\GetPaymentStateByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentStates.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentStates.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentStates.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentStates.Application.UseCases;

public class GetPaymentStateByIdUseCase
{
    private readonly IPaymentStateRepository _repository;

    public GetPaymentStateByIdUseCase(IPaymentStateRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentState?> ExecuteAsync(int id)
    {
        var idVO = PaymentStateId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}
