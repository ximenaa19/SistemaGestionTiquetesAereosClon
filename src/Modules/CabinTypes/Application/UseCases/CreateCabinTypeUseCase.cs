// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinTypes\Application\UseCases\CreateCabinTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using GestionAerolineas.src.Modules.CabinTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.CabinTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.CabinTypes.Domain.Repository;
using GestionAerolineas.src.Modules.CabinTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CabinTypes.Application.UseCases;

public class CreateCabinTypeUseCase
{
    private readonly ICabinTypeValidator _validator;
    private readonly ICabinTypeRepository _repository;

    public CreateCabinTypeUseCase(ICabinTypeValidator validator, ICabinTypeRepository repository)
    {
        _validator = validator;
        _repository = repository;
    }
    public async Task ExecuteAsync(int id, string name)
    {
        var nameVO = CabinTypesName.Create(name);

        await _validator.ValidateNameAsync(nameVO);

        var cabinType = CabinType.Create(
            CabinTypesId.Create(id),
            nameVO
        );

        await _repository.AddAsync(cabinType);
    }

}
