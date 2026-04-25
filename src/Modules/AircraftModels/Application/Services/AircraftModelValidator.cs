// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\Application\Services\AircraftModelValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftModels.Application.Interfaces;
using GestionAerolineas.src.Modules.AircraftModels.Domain.Repositories;
using GestionAerolineas.src.Modules.AircraftModels.Domain.ValueObject;
using GestionAerolineas.src.Modules.AircraftManufacturers.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.AircraftModels.Application.Services;

public class AircraftModelValidator : IAircraftModelValidator
{
    private readonly IAircraftModelRepository _repository;
    private readonly AircraftManufacturerRepository _manufacturerRepository;

    public AircraftModelValidator(
        IAircraftModelRepository repository,
        AircraftManufacturerRepository manufacturerRepository)
    {
        _repository = repository;
        _manufacturerRepository = manufacturerRepository;
    }

    public async Task ValidateManufacturerExistsAsync(AircraftManufacturerId manufacturerId)
    {
        var exists = await _manufacturerRepository.ExistsAsync(
            GestionAerolineas.src.Modules.AircraftManufacturers.Domain.ValueObject.AircraftManufacturerId.Create(manufacturerId.Value));

        if (!exists)
            throw new Exception("El fabricante no existe");
    }

    public async Task ValidateModelNameAsync(AircraftModelName modelName, AircraftModelId? currentId = null)
    {
        var normalizedCandidate = AircraftModelName.Normalize(modelName.Value);
        var all = await _repository.GetAllAsync();

        foreach (var item in all)
        {
            if (currentId != null && item.Id.Value == currentId.Value)
                continue;

            if (AircraftModelName.Normalize(item.ModelName.Value) == normalizedCandidate)
                throw new Exception("Ya existe un modelo con ese nombre");
        }
    }
}
