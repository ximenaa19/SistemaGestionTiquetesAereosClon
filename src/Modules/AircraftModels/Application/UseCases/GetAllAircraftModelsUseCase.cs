// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\Application\UseCases\GetAllAircraftModelsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftModels.Domain.Aggregate;
using GestionAerolineas.src.Modules.AircraftModels.Domain.Repositories;

namespace GestionAerolineas.src.Modules.AircraftModels.Application.UseCases;

public class GetAllAircraftModelsUseCase
{
    private readonly IAircraftModelRepository _repository;

    public GetAllAircraftModelsUseCase(IAircraftModelRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<AircraftModel>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

