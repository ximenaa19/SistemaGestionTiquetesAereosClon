// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentStates\Application\UseCases\UpdatePaymentStateUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentStates.Application.Interfaces;
using GestionAerolineas.src.Modules.PaymentStates.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentStates.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentStates.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentStates.Application.UseCases;

public class UpdatePaymentStateUseCase
{
    private readonly IPaymentStateRepository _repository;
    private readonly IPaymentStateValidator _validator;

    public UpdatePaymentStateUseCase(
        IPaymentStateRepository repository,
        IPaymentStateValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name)
    {
        var idVO = PaymentStateId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing is null)
            throw new Exception("El estado de pago no existe");

        var nameVO = PaymentStateName.Create(name);

        await _validator.ValidateNameAsync(nameVO, idVO);

        var updated = PaymentState.Create(idVO, nameVO);

        await _repository.UpdateAsync(updated);
    }
}
