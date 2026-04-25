// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Application\UseCases\GetAllCabinConfigurationsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Aggregate;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Repositories;

namespace GestionAerolineas.src.Modules.CabinConfiguration.Application.UseCases;

public class GetAllCabinConfigurationsUseCase
{
    private readonly ICabinConfigurationRepository _repository;

    public GetAllCabinConfigurationsUseCase(ICabinConfigurationRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<CabinConfigurationAggregate>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}
