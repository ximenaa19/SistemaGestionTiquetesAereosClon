// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Regions\Aplication\UseCases\GetRegionByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Regions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Regions.Domain.Repositories;
using GestionAerolineas.src.Modules.Regions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Regions.Application.UseCases;

public class GetRegionByNameUseCase
{
    private readonly IRegionRepository _repository;

    public GetRegionByNameUseCase(IRegionRepository repository)
    {
        _repository = repository;
    }

    public Task<Region?> ExecuteAsync(string name)
    {
        return _repository.GetByNameAsync(RegionName.Create(name));
    }
}

