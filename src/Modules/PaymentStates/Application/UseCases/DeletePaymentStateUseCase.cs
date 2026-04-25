// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentStates\Application\UseCases\DeletePaymentStateUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentStates.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentStates.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentStates.Application.UseCases;

public class DeletePaymentStateUseCase
{
    private readonly IPaymentStateRepository _repository;

    public DeletePaymentStateUseCase(IPaymentStateRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var paymentStateId = PaymentStateId.Create(id);
        var paymentState = await _repository.GetByIdAsync(paymentStateId);

        if (paymentState is null)
            throw new KeyNotFoundException($"PaymentState con id '{paymentStateId.Value}' no existe.");

        await _repository.DeleteAsync(paymentState);
    }
}
