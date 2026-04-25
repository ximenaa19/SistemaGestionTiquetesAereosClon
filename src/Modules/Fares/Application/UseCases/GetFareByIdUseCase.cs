// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Application\UseCases\GetFareByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Fares.Domain.Aggregate;
using GestionAerolineas.src.Modules.Fares.Domain.Repositories;
using GestionAerolineas.src.Modules.Fares.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Fares.Application.UseCases;

public class GetFareByIdUseCase
{
    private readonly IFareRepository _repository;

    public GetFareByIdUseCase(IFareRepository repository)
    {
        _repository = repository;
    }

    public Task<Fare?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(FareId.Create(id));
    }
}

