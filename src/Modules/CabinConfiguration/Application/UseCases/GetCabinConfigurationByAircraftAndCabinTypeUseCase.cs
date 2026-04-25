// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Application\UseCases\GetCabinConfigurationByAircraftAndCabinTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Aggregate;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Repositories;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CabinConfiguration.Application.UseCases;

public class GetCabinConfigurationByAircraftAndCabinTypeUseCase
{
    private readonly ICabinConfigurationRepository _repository;

    public GetCabinConfigurationByAircraftAndCabinTypeUseCase(ICabinConfigurationRepository repository)
    {
        _repository = repository;
    }

    public Task<CabinConfigurationAggregate?> ExecuteAsync(int aircraftId, int cabinTypeId)
    {
        return _repository.GetByAircraftAndCabinTypeAsync(
            CabinConfigurationAircraftId.Create(aircraftId),
            CabinConfigurationCabinTypeId.Create(cabinTypeId)
        );
    }
}
