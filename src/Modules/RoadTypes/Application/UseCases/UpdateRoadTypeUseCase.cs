// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RoadTypes\Application\UseCases\UpdateRoadTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using System.Threading.Tasks;
using GestionAerolineas.src.Modules.RoadTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.RoadTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.RoadTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.RoadTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RoadTypes.Application.UseCases;

public class UpdateRoadTypeUseCase
{
    private readonly IRoadTypeRepository _repository;
    private readonly IRoadTypeValidator _validator;

    public UpdateRoadTypeUseCase(
        IRoadTypeRepository repository,
        IRoadTypeValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name)
    {
        var idVO = RoadTypeId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing == null)
            throw new Exception("El RoadType no existe");

        var nameVO = RoadTypeName.Create(name);

        await _validator.ValidateNameAsync(nameVO);

        var updated = RoadType.Create(idVO, nameVO);

        await _repository.UpdateAsync(updated);
    }
}