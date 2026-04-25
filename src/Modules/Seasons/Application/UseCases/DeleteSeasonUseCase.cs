// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Application\UseCases\DeleteSeasonUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Seasons.Domain.Repositories;
using GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Seasons.Application.UseCases;

public class DeleteSeasonUseCase
{
    private readonly ISeasonRepository _repository;

    public DeleteSeasonUseCase(ISeasonRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var seasonId = SeasonId.Create(id);
        var season = await _repository.GetByIdAsync(seasonId);

        if (season is null)
            throw new KeyNotFoundException($"Season con id '{seasonId.Value}' no existe.");

        await _repository.DeleteAsync(season);
    }
}
