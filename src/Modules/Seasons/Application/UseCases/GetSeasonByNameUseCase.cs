// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Application\UseCases\GetSeasonByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Seasons.Domain.Aggregate;
using GestionAerolineas.src.Modules.Seasons.Domain.Repositories;
using GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Seasons.Application.UseCases;

public class GetSeasonByNameUseCase
{
    private readonly ISeasonRepository _repository;

    public GetSeasonByNameUseCase(ISeasonRepository repository)
    {
        _repository = repository;
    }

    public async Task<Season?> ExecuteAsync(string name)
    {
        var nameVO = SeasonName.Create(name);
        return await _repository.GetByNameAsync(nameVO);
    }
}
