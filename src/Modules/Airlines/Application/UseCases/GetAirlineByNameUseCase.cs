// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\Application\UseCases\GetAirlineByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Domain.Aggregate;
using GestionAerolineas.src.Modules.Airlines.Domain.Repositories;
using GestionAerolineas.src.Modules.Airlines.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Airlines.Application.UseCases;

public class GetAirlineByNameUseCase
{
    private readonly IAirlineRepository _repository;

    public GetAirlineByNameUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public Task<Airline?> ExecuteAsync(string name)
    {
        return _repository.GetByNameAsync(AirlineName.Create(name));
    }
}

