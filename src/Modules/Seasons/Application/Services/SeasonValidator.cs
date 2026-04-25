// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Application\Services\SeasonValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Seasons.Application.Interfaces;
using GestionAerolineas.src.Modules.Seasons.Domain.Repositories;
using GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Seasons.Application.Services;

public class SeasonValidator : ISeasonValidator
{
    private readonly ISeasonRepository _repository;

    public SeasonValidator(ISeasonRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateNameAsync(SeasonName name, SeasonId? currentId = null)
    {
        var existingByName = await _repository.GetByNameAsync(name);

        if (existingByName is not null && existingByName.Id != currentId)
            throw new Exception("Ya existe una temporada con ese nombre");
    }
}
