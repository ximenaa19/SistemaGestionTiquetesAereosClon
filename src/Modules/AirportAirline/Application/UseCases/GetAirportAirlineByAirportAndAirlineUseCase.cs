// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Application\UseCases\GetAirportAirlineByAirportAndAirlineUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AirportAirline.Domain.Aggregate;
using GestionAerolineas.src.Modules.AirportAirline.Domain.Repositories;
using GestionAerolineas.src.Modules.AirportAirline.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AirportAirline.Application.UseCases;

public class GetAirportAirlineByAirportAndAirlineUseCase
{
    private readonly IAirportAirlineRepository _repository;

    public GetAirportAirlineByAirportAndAirlineUseCase(IAirportAirlineRepository repository)
    {
        _repository = repository;
    }

    public Task<AirportAirlineRelation?> ExecuteAsync(int airportId, int airlineId)
    {
        return _repository.GetByAirportAndAirlineAsync(
            AirportAirlineAirportId.Create(airportId),
            AirportAirlineAirlineId.Create(airlineId)
        );
    }
}

