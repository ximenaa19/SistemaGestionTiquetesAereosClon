// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\Application\UseCases\GetAllAirlinesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Domain.Aggregate;
using GestionAerolineas.src.Modules.Airlines.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Airlines.Application.UseCases;

public class GetAllAirlinesUseCase
{
    private readonly IAirlineRepository _repository;

    public GetAllAirlinesUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Airline>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

