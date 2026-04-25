// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Regions\Aplication\UseCases\UpdateRegionUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Regions.Application.Interfaces;
using GestionAerolineas.src.Modules.Regions.Domain.Aggregate;
using GestionAerolineas.src.Modules.Regions.Domain.Repositories;
using GestionAerolineas.src.Modules.Regions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Regions.Application.UseCases;

public class UpdateRegionUseCase
{
    private readonly IRegionRepository _repository;
    private readonly IRegionValidator _validator;

    public UpdateRegionUseCase(IRegionRepository repository, IRegionValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name, string type, int countryId)
    {
        var idVO = RegionId.Create(id);
        var nameVO = RegionName.Create(name);
        var typeVO = RegionType.Create(type);
        var countryVO = RegionCountryId.Create(countryId);

        await _validator.ValidateCountryExistsAsync(countryVO);
        await _validator.ValidateNameAsync(nameVO, countryVO, idVO);

        var entity = Region.Create(idVO, nameVO, typeVO, countryVO);
        await _repository.UpdateAsync(entity);
    }
}

