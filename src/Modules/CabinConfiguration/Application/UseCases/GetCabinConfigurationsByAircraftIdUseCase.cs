// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Application\UseCases\GetCabinConfigurationsByAircraftIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Aggregate;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Repositories;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CabinConfiguration.Application.UseCases;

public class GetCabinConfigurationsByAircraftIdUseCase
{
    private readonly ICabinConfigurationRepository _repository;

    public GetCabinConfigurationsByAircraftIdUseCase(ICabinConfigurationRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<CabinConfigurationAggregate>> ExecuteAsync(int aircraftId)
    {
        return _repository.GetByAircraftIdAsync(CabinConfigurationAircraftId.Create(aircraftId));
    }
}
