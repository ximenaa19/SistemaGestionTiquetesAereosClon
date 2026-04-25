// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\Application\UseCases\GetAircraftByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Domain.Aggregate;
using GestionAerolineas.src.Modules.Aircraft.Domain.Repositories;
using GestionAerolineas.src.Modules.Aircraft.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Aircraft.Application.UseCases;

public class GetAircraftByIdUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAircraftByIdUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public Task<AircraftAggregate?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(AircraftId.Create(id));
    }
}

