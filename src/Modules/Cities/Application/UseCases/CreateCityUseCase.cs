// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Cities\Application\UseCases\CreateCityUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Cities.Application.Interfaces;
using GestionAerolineas.src.Modules.Cities.Domain.Aggregate;
using GestionAerolineas.src.Modules.Cities.Domain.Repositories;
using GestionAerolineas.src.Modules.Cities.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Cities.Application.UseCases;

public class CreateCityUseCase
{
    private readonly ICityRepository _repository;
    private readonly ICityValidator _validator;

    public CreateCityUseCase(ICityRepository repository, ICityValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(string name, int regionId)
    {
        var nameVO = CityName.Create(name);
        var regionVO = CityRegionId.Create(regionId);

        await _validator.ValidateRegionExistsAsync(regionVO);
        await _validator.ValidateNameAsync(nameVO, regionVO);

        var entity = City.CreateNew(nameVO, regionVO);

        await _repository.AddAsync(entity);
    }
}
