// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\Application\UseCases\GetAllPaymentMethodsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethods.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.Repositories;

namespace GestionAerolineas.src.Modules.PaymentMethods.Application.UseCases;

public class GetAllPaymentMethodsUseCase
{
    private readonly IPaymentMethodRepository _repository;

    public GetAllPaymentMethodsUseCase(IPaymentMethodRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<PaymentMethod>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

