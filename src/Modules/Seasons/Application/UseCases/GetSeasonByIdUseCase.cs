// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Application\UseCases\GetSeasonByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Seasons.Domain.Aggregate;
using GestionAerolineas.src.Modules.Seasons.Domain.Repositories;
using GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Seasons.Application.UseCases;

public class GetSeasonByIdUseCase
{
    private readonly ISeasonRepository _repository;

    public GetSeasonByIdUseCase(ISeasonRepository repository)
    {
        _repository = repository;
    }

    public async Task<Season?> ExecuteAsync(int id)
    {
        var idVO = SeasonId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}
