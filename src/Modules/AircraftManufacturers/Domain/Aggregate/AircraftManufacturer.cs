// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftManufacturers\Domain\Aggregate\AircraftManufacturer.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AircraftManufacturers.Domain.Aggregate;

public class AircraftManufacturer
{
    public AircraftManufacturerId Id { get; private set; }
    public AircraftManufacturerName Name { get; private set; }
    public AircraftManufacturerCountryId CountryId { get; private set; }

    private AircraftManufacturer(AircraftManufacturerId id, AircraftManufacturerName name, AircraftManufacturerCountryId countryId)
    {
        Id = id;
        Name = name;
        CountryId = countryId;
    }

    public static AircraftManufacturer Create(
        AircraftManufacturerId id,
        AircraftManufacturerName name,
        AircraftManufacturerCountryId countryId)
    {
        return new AircraftManufacturer(id, name, countryId);
    }

    public static AircraftManufacturer CreateNew(AircraftManufacturerName name, AircraftManufacturerCountryId countryId)
    {
        return new AircraftManufacturer(AircraftManufacturerId.CreateEmpty(), name, countryId);
    }
}

