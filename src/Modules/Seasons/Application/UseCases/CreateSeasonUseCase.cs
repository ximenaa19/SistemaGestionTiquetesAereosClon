// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Application\UseCases\CreateSeasonUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Seasons.Application.Interfaces;
using GestionAerolineas.src.Modules.Seasons.Domain.Aggregate;
using GestionAerolineas.src.Modules.Seasons.Domain.Repositories;
using GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Seasons.Application.UseCases;

public class CreateSeasonUseCase
{
    private readonly ISeasonRepository _repository;
    private readonly ISeasonValidator _validator;

    public CreateSeasonUseCase(
        ISeasonRepository repository,
        ISeasonValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(string name, string? description, decimal priceFactor)
    {
        var nameVO = SeasonName.Create(name);
        var descriptionVO = SeasonDescription.Create(description);
        var priceFactorVO = SeasonPriceFactor.Create(priceFactor);

        await _validator.ValidateNameAsync(nameVO);

        var entity = Season.CreateNew(nameVO, descriptionVO, priceFactorVO);

        await _repository.AddAsync(entity);
    }
}
