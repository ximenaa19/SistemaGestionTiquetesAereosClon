// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Continents\Application\UseCases\GetContinentByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Continents.Domain.Aggregate;
using GestionAerolineas.src.Modules.Continents.Domain.Repositories;
using GestionAerolineas.src.Modules.Continents.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Continents.Application.UseCases;

public class GetContinentByNameUseCase
{
    private readonly IContinentRepository _repository;

    public GetContinentByNameUseCase(IContinentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Continent?> ExecuteAsync(string name)
    {
        var nameVO = ContinentName.Create(name);
        return await _repository.GetByNameAsync(nameVO);
    }
}