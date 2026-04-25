// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Application\UseCases\DeletePaymentUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Payments.Application.Interfaces;
using GestionAerolineas.src.Modules.Payments.Domain.Repositories;
using GestionAerolineas.src.Modules.Payments.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Payments.Application.UseCases;

public class DeletePaymentUseCase
{
    private readonly IPaymentRepository _repository;
    private readonly IPaymentValidator _validator;

    public DeletePaymentUseCase(IPaymentRepository repository, IPaymentValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var paymentId = PaymentId.Create(id);

        var entity = await _repository.GetByIdAsync(paymentId);
        if (entity is null)
            return false;

        await _validator.ValidateDeletableAsync(paymentId);
        await _repository.DeleteAsync(entity);
        return true;
    }
}

