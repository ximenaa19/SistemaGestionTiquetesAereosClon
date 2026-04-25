// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Application\UseCases\GetAllPaymentsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Payments.Domain.Aggregate;
using GestionAerolineas.src.Modules.Payments.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Payments.Application.UseCases;

public class GetAllPaymentsUseCase
{
    private readonly IPaymentRepository _repository;

    public GetAllPaymentsUseCase(IPaymentRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Payment>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

