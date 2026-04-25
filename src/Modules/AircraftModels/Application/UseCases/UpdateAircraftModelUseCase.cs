// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\Application\UseCases\UpdateAircraftModelUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftModels.Application.Interfaces;
using GestionAerolineas.src.Modules.AircraftModels.Domain.Aggregate;
using GestionAerolineas.src.Modules.AircraftModels.Domain.Repositories;
using GestionAerolineas.src.Modules.AircraftModels.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AircraftModels.Application.UseCases;

public class UpdateAircraftModelUseCase
{
    private readonly IAircraftModelRepository _repository;
    private readonly IAircraftModelValidator _validator;

    public UpdateAircraftModelUseCase(IAircraftModelRepository repository, IAircraftModelValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(
        int id,
        int manufacturerId,
        string modelName,
        int maxCapacity,
        decimal? maxTakeoffWeightKg,
        decimal? fuelConsumptionKgPerHour,
        int? cruiseSpeedKmh,
        int? cruiseAltitudeFt)
    {
        var idVO = AircraftModelId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is null)
            throw new Exception("El modelo no existe");

        var manufacturerIdVO = AircraftManufacturerId.Create(manufacturerId);
        var modelNameVO = AircraftModelName.Create(modelName);
        var maxCapacityVO = AircraftModelMaxCapacity.Create(maxCapacity);

        CreateAircraftModelUseCase.ValidateOptionalNumbers(maxTakeoffWeightKg, fuelConsumptionKgPerHour, cruiseSpeedKmh, cruiseAltitudeFt);

        await _validator.ValidateManufacturerExistsAsync(manufacturerIdVO);
        await _validator.ValidateModelNameAsync(modelNameVO, idVO);

        var updated = AircraftModel.Create(
            idVO,
            manufacturerIdVO,
            modelNameVO,
            maxCapacityVO,
            maxTakeoffWeightKg,
            fuelConsumptionKgPerHour,
            cruiseSpeedKmh,
            cruiseAltitudeFt
        );

        await _repository.UpdateAsync(updated);
    }
}
