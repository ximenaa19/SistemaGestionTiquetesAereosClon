// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\Application\UseCases\GetAircraftByRegistrationUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Domain.Aggregate;
using GestionAerolineas.src.Modules.Aircraft.Domain.Repositories;
using GestionAerolineas.src.Modules.Aircraft.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Aircraft.Application.UseCases;

public class GetAircraftByRegistrationUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAircraftByRegistrationUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public Task<AircraftAggregate?> ExecuteAsync(string registration)
    {
        return _repository.GetByRegistrationAsync(AircraftRegistration.Create(registration));
    }
}

