// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\Domain\Aggregate\Airline.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Airlines.Domain.Aggregate;

public class Airline
{
    public AirlineId Id { get; private set; }
    public AirlineName Name { get; private set; }
    public AirlineIataCode IataCode { get; private set; }
    public AirlineOriginCountryId OriginCountryId { get; private set; }
    public AirlineIsActive IsActive { get; private set; }

    private Airline(
        AirlineId id,
        AirlineName name,
        AirlineIataCode iataCode,
        AirlineOriginCountryId originCountryId,
        AirlineIsActive isActive)
    {
        Id = id;
        Name = name;
        IataCode = iataCode;
        OriginCountryId = originCountryId;
        IsActive = isActive;
    }

    public static Airline Create(
        AirlineId id,
        AirlineName name,
        AirlineIataCode iataCode,
        AirlineOriginCountryId originCountryId,
        AirlineIsActive isActive)
    {
        return new Airline(id, name, iataCode, originCountryId, isActive);
    }

    public static Airline CreateNew(
        AirlineName name,
        AirlineIataCode iataCode,
        AirlineOriginCountryId originCountryId,
        AirlineIsActive? isActive = null)
    {
        return new Airline(AirlineId.CreateEmpty(), name, iataCode, originCountryId, isActive ?? AirlineIsActive.Create(true));
    }
}

