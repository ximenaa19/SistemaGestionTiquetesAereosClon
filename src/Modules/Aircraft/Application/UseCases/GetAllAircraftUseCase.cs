// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\Application\UseCases\GetAllAircraftUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Domain.Aggregate;
using GestionAerolineas.src.Modules.Aircraft.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Aircraft.Application.UseCases;

public class GetAllAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAllAircraftUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<AircraftAggregate>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

