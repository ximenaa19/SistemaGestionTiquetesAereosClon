// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\Infrastructure\Entity\AircraftModelEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.AircraftModels.Infrastructure.Entity;

public class AircraftModelEntity
{
    public int Id { get; set; }
    public int ManufacturerId { get; set; }
    public string? ModelName { get; set; }
    public int MaxCapacity { get; set; }
    public decimal? MaxTakeoffWeightKg { get; set; }
    public decimal? FuelConsumptionKgPerHour { get; set; }
    public int? CruiseSpeedKmh { get; set; }
    public int? CruiseAltitudeFt { get; set; }
}

