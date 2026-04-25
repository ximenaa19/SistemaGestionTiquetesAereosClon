// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AvailabilityStatuses\Application\UseCases\GetAvailabilityStatusByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AvailabilityStatuses.Application.UseCases;

public class GetAvailabilityStatusByNameUseCase
{
    private readonly IAvailabilityStatusRepository _repository;

    public GetAvailabilityStatusByNameUseCase(IAvailabilityStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task<AvailabilityStatus?> ExecuteAsync(string name)
    {
        var nameVO = AvailabilityStatusName.Create(name);
        return await _repository.GetByNameAsync(nameVO);
    }
}
