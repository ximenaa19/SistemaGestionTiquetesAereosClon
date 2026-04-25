// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Application\UseCases\GetPaymentsByMethodIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Payments.Domain.Aggregate;
using GestionAerolineas.src.Modules.Payments.Domain.Repositories;
using GestionAerolineas.src.Modules.Payments.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Payments.Application.UseCases;

public class GetPaymentsByMethodIdUseCase
{
    private readonly IPaymentRepository _repository;

    public GetPaymentsByMethodIdUseCase(IPaymentRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Payment>> ExecuteAsync(int methodId)
    {
        return _repository.GetByMethodIdAsync(PaymentMethodId.Create(methodId));
    }
}

