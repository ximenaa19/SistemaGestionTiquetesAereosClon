// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Application\UseCases\UpdateSeasonUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Seasons.Application.Interfaces;
using GestionAerolineas.src.Modules.Seasons.Domain.Aggregate;
using GestionAerolineas.src.Modules.Seasons.Domain.Repositories;
using GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Seasons.Application.UseCases;

public class UpdateSeasonUseCase
{
    private readonly ISeasonRepository _repository;
    private readonly ISeasonValidator _validator;

    public UpdateSeasonUseCase(
        ISeasonRepository repository,
        ISeasonValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name, string? description, decimal priceFactor)
    {
        var idVO = SeasonId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing is null)
            throw new Exception("La temporada no existe");

        var nameVO = SeasonName.Create(name);
        var descriptionVO = SeasonDescription.Create(description);
        var priceFactorVO = SeasonPriceFactor.Create(priceFactor);

        await _validator.ValidateNameAsync(nameVO, idVO);

        var updated = Season.Create(idVO, nameVO, descriptionVO, priceFactorVO);

        await _repository.UpdateAsync(updated);
    }
}
