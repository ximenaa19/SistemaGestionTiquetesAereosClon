// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Application\UseCases\GetAllAirportAirlinesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AirportAirline.Domain.Aggregate;
using GestionAerolineas.src.Modules.AirportAirline.Domain.Repositories;

namespace GestionAerolineas.src.Modules.AirportAirline.Application.UseCases;

public class GetAllAirportAirlinesUseCase
{
    private readonly IAirportAirlineRepository _repository;

    public GetAllAirportAirlinesUseCase(IAirportAirlineRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<AirportAirlineRelation>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

