// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentStates\Application\UseCases\GetAllPaymentStatesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentStates.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentStates.Domain.Repositories;

namespace GestionAerolineas.src.Modules.PaymentStates.Application.UseCases;

public class GetAllPaymentStatesUseCase
{
    private readonly IPaymentStateRepository _repository;

    public GetAllPaymentStatesUseCase(IPaymentStateRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PaymentState>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
