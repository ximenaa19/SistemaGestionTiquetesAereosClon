// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Continents\Application\UseCases\GetAllContinentsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Continents.Domain.Aggregate;
using GestionAerolineas.src.Modules.Continents.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Continents.Application.UseCases;

public class GetAllContinentsUseCase
{
    private readonly IContinentRepository _repository;

    public GetAllContinentsUseCase(IContinentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Continent>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
