// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Cities\Domain\Aggregate\City.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Cities.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Cities.Domain.Aggregate;

public class City
{
    public CityId Id { get; private set; }
    public CityName Name { get; private set; }
    public CityRegionId RegionId { get; private set; }

    private City(CityId id, CityName name, CityRegionId regionId)
    {
        Id = id;
        Name = name;
        RegionId = regionId;
    }

    public static City Create(CityId id, CityName name, CityRegionId regionId)
    {
        return new City(id, name, regionId);
    }

    public static City CreateNew(CityName name, CityRegionId regionId)
    {
        return new City(CityId.CreateEmpty(), name, regionId);
    }
}


