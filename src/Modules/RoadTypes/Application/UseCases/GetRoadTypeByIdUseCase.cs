// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RoadTypes\Application\UseCases\GetRoadTypeByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using GestionAerolineas.src.Modules.RoadTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.RoadTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.RoadTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RoadTypes.Application.UseCases;

public class GetRoadTypeByIdUseCase
{
    private readonly IRoadTypeRepository _repository;

    public GetRoadTypeByIdUseCase(IRoadTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<RoadType?> ExecuteAsync(int id)
    {
        return await _repository.GetByIdAsync(RoadTypeId.Create(id));
    }
}
