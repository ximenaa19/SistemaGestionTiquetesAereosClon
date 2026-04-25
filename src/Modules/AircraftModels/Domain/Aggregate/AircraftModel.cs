// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\Domain\Aggregate\AircraftModel.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftModels.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AircraftModels.Domain.Aggregate;

public class AircraftModel
{
    public AircraftModelId Id { get; private set; }
    public AircraftManufacturerId ManufacturerId { get; private set; }
    public AircraftModelName ModelName { get; private set; }
    public AircraftModelMaxCapacity MaxCapacity { get; private set; }
    public decimal? MaxTakeoffWeightKg { get; private set; }
    public decimal? FuelConsumptionKgPerHour { get; private set; }
    public int? CruiseSpeedKmh { get; private set; }
    public int? CruiseAltitudeFt { get; private set; }

    private AircraftModel(
        AircraftModelId id,
        AircraftManufacturerId manufacturerId,
        AircraftModelName modelName,
        AircraftModelMaxCapacity maxCapacity,
        decimal? maxTakeoffWeightKg,
        decimal? fuelConsumptionKgPerHour,
        int? cruiseSpeedKmh,
        int? cruiseAltitudeFt)
    {
        Id = id;
        ManufacturerId = manufacturerId;
        ModelName = modelName;
        MaxCapacity = maxCapacity;
        MaxTakeoffWeightKg = maxTakeoffWeightKg;
        FuelConsumptionKgPerHour = fuelConsumptionKgPerHour;
        CruiseSpeedKmh = cruiseSpeedKmh;
        CruiseAltitudeFt = cruiseAltitudeFt;
    }

    public static AircraftModel Create(
        AircraftModelId id,
        AircraftManufacturerId manufacturerId,
        AircraftModelName modelName,
        AircraftModelMaxCapacity maxCapacity,
        decimal? maxTakeoffWeightKg,
        decimal? fuelConsumptionKgPerHour,
        int? cruiseSpeedKmh,
        int? cruiseAltitudeFt)
    {
        return new AircraftModel(
            id,
            manufacturerId,
            modelName,
            maxCapacity,
            maxTakeoffWeightKg,
            fuelConsumptionKgPerHour,
            cruiseSpeedKmh,
            cruiseAltitudeFt
        );
    }

    public static AircraftModel CreateNew(
        AircraftManufacturerId manufacturerId,
        AircraftModelName modelName,
        AircraftModelMaxCapacity maxCapacity,
        decimal? maxTakeoffWeightKg,
        decimal? fuelConsumptionKgPerHour,
        int? cruiseSpeedKmh,
        int? cruiseAltitudeFt)
    {
        return new AircraftModel(
            AircraftModelId.CreateEmpty(),
            manufacturerId,
            modelName,
            maxCapacity,
            maxTakeoffWeightKg,
            fuelConsumptionKgPerHour,
            cruiseSpeedKmh,
            cruiseAltitudeFt
        );
    }
}

