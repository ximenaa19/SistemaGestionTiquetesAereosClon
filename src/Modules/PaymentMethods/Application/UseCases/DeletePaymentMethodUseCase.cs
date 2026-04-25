// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\Application\UseCases\DeletePaymentMethodUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethods.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentMethods.Application.UseCases;

public class DeletePaymentMethodUseCase
{
    private readonly IPaymentMethodRepository _repository;

    public DeletePaymentMethodUseCase(IPaymentMethodRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var idVO = PaymentMethodId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is null)
            throw new Exception("El método de pago no existe");

        await _repository.DeleteAsync(existing);
    }
}

