// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Regions\Aplication\UseCases\GetAllRegionsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Regions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Regions.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Regions.Application.UseCases;

public class GetAllRegionsUseCase
{
    private readonly IRegionRepository _repository;

    public GetAllRegionsUseCase(IRegionRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Region>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

