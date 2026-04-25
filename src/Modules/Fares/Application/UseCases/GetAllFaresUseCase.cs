// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Application\UseCases\GetAllFaresUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Fares.Domain.Aggregate;
using GestionAerolineas.src.Modules.Fares.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Fares.Application.UseCases;

public class GetAllFaresUseCase
{
    private readonly IFareRepository _repository;

    public GetAllFaresUseCase(IFareRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Fare>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

