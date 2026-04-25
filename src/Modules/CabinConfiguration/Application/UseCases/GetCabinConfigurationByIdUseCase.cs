// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Application\UseCases\GetCabinConfigurationByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Aggregate;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Repositories;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CabinConfiguration.Application.UseCases;

public class GetCabinConfigurationByIdUseCase
{
    private readonly ICabinConfigurationRepository _repository;

    public GetCabinConfigurationByIdUseCase(ICabinConfigurationRepository repository)
    {
        _repository = repository;
    }

    public Task<CabinConfigurationAggregate?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(CabinConfigurationId.Create(id));
    }
}
