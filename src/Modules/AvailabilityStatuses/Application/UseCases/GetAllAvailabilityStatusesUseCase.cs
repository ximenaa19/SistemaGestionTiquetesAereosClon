// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AvailabilityStatuses\Application\UseCases\GetAllAvailabilityStatusesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.Repositories;

namespace GestionAerolineas.src.Modules.AvailabilityStatuses.Application.UseCases;

public class GetAllAvailabilityStatusesUseCase
{
    private readonly IAvailabilityStatusRepository _repository;

    public GetAllAvailabilityStatusesUseCase(IAvailabilityStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AvailabilityStatus>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
