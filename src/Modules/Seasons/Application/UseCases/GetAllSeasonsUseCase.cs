// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Application\UseCases\GetAllSeasonsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Seasons.Domain.Aggregate;
using GestionAerolineas.src.Modules.Seasons.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Seasons.Application.UseCases;

public class GetAllSeasonsUseCase
{
    private readonly ISeasonRepository _repository;

    public GetAllSeasonsUseCase(ISeasonRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Season>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
