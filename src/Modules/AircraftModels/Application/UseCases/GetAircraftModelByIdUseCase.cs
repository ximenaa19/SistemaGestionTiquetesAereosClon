// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\Application\UseCases\GetAircraftModelByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftModels.Domain.Aggregate;
using GestionAerolineas.src.Modules.AircraftModels.Domain.Repositories;
using GestionAerolineas.src.Modules.AircraftModels.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AircraftModels.Application.UseCases;

public class GetAircraftModelByIdUseCase
{
    private readonly IAircraftModelRepository _repository;

    public GetAircraftModelByIdUseCase(IAircraftModelRepository repository)
    {
        _repository = repository;
    }

    public Task<AircraftModel?> ExecuteAsync(int id)
    {
        var idVO = AircraftModelId.Create(id);
        return _repository.GetByIdAsync(idVO);
    }
}

