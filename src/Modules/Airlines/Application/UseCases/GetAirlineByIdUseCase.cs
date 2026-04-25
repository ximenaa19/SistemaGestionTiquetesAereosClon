// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\Application\UseCases\GetAirlineByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Domain.Aggregate;
using GestionAerolineas.src.Modules.Airlines.Domain.Repositories;
using GestionAerolineas.src.Modules.Airlines.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Airlines.Application.UseCases;

public class GetAirlineByIdUseCase
{
    private readonly IAirlineRepository _repository;

    public GetAirlineByIdUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public Task<Airline?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(AirlineId.Create(id));
    }
}

