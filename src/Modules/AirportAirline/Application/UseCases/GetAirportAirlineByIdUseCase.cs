// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Application\UseCases\GetAirportAirlineByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AirportAirline.Domain.Aggregate;
using GestionAerolineas.src.Modules.AirportAirline.Domain.Repositories;
using GestionAerolineas.src.Modules.AirportAirline.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AirportAirline.Application.UseCases;

public class GetAirportAirlineByIdUseCase
{
    private readonly IAirportAirlineRepository _repository;

    public GetAirportAirlineByIdUseCase(IAirportAirlineRepository repository)
    {
        _repository = repository;
    }

    public Task<AirportAirlineRelation?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(AirportAirlineId.Create(id));
    }
}

