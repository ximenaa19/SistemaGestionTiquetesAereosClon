// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\Application\UseCases\CreateAircraftModelUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftModels.Application.Interfaces;
using GestionAerolineas.src.Modules.AircraftModels.Domain.Aggregate;
using GestionAerolineas.src.Modules.AircraftModels.Domain.Repositories;
using GestionAerolineas.src.Modules.AircraftModels.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AircraftModels.Application.UseCases;

public class CreateAircraftModelUseCase
{
    private readonly IAircraftModelRepository _repository;
    private readonly IAircraftModelValidator _validator;

    public CreateAircraftModelUseCase(IAircraftModelRepository repository, IAircraftModelValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(
        int manufacturerId,
        string modelName,
        int maxCapacity,
        decimal? maxTakeoffWeightKg,
        decimal? fuelConsumptionKgPerHour,
        int? cruiseSpeedKmh,
        int? cruiseAltitudeFt)
    {
        var manufacturerIdVO = AircraftManufacturerId.Create(manufacturerId);
        var modelNameVO = AircraftModelName.Create(modelName);
        var maxCapacityVO = AircraftModelMaxCapacity.Create(maxCapacity);

        ValidateOptionalNumbers(maxTakeoffWeightKg, fuelConsumptionKgPerHour, cruiseSpeedKmh, cruiseAltitudeFt);

        await _validator.ValidateManufacturerExistsAsync(manufacturerIdVO);
        await _validator.ValidateModelNameAsync(modelNameVO);

        var entity = AircraftModel.CreateNew(
            manufacturerIdVO,
            modelNameVO,
            maxCapacityVO,
            maxTakeoffWeightKg,
            fuelConsumptionKgPerHour,
            cruiseSpeedKmh,
            cruiseAltitudeFt
        );

        await _repository.AddAsync(entity);
    }

    public static void ValidateOptionalNumbers(
        decimal? maxTakeoffWeightKg,
        decimal? fuelConsumptionKgPerHour,
        int? cruiseSpeedKmh,
        int? cruiseAltitudeFt)
    {
        if (maxTakeoffWeightKg != null && maxTakeoffWeightKg < 0)
            throw new Exception("peso_max_despegue_kg no puede ser negativo");

        if (fuelConsumptionKgPerHour != null && fuelConsumptionKgPerHour < 0)
            throw new Exception("consumo_combustible_kg_h no puede ser negativo");

        if (cruiseSpeedKmh != null && cruiseSpeedKmh < 0)
            throw new Exception("velocidad_crucero_kmh no puede ser negativo");

        if (cruiseAltitudeFt != null && cruiseAltitudeFt < 0)
            throw new Exception("altitud_crucero_ft no puede ser negativo");
    }
}
