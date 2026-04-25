// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethodTypes\Application\UseCases\UpdatePaymentMethodTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethodTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentMethodTypes.Application.UseCases;

public class UpdatePaymentMethodTypeUseCase
{
    private readonly IPaymentMethodTypeRepository _repository;
    private readonly IPaymentMethodTypeValidator _validator;

    public UpdatePaymentMethodTypeUseCase(
        IPaymentMethodTypeRepository repository,
        IPaymentMethodTypeValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name)
    {
        var idVO = PaymentMethodTypeId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing == null)
            throw new Exception("El PaymentMethodType no existe");

        var nameVO = PaymentMethodTypeName.Create(name);

        await _validator.ValidateNameAsync(nameVO);

        var updated = PaymentMethodType.Create(idVO, nameVO);

        await _repository.UpdateAsync(updated);
    }
}
