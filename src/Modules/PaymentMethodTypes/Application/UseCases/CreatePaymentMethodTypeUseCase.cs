// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethodTypes\Application\UseCases\CreatePaymentMethodTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethodTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentMethodTypes.Application.UseCases;

public class CreatePaymentMethodTypeUseCase
{
    private readonly IPaymentMethodTypeRepository _repository;
    private readonly IPaymentMethodTypeValidator _validator;

    public CreatePaymentMethodTypeUseCase(
        IPaymentMethodTypeRepository repository,
        IPaymentMethodTypeValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(string name)
    {
        var nameVO = PaymentMethodTypeName.Create(name);

        await _validator.ValidateNameAsync(nameVO);

        var entity = PaymentMethodType.CreateNew(nameVO);

        await _repository.AddAsync(entity);
    }
}
