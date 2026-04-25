// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\Domain\Aggregate\Airport.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airports.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Airports.Domain.Aggregate;

public class Airport
{
    public AirportId Id { get; private set; }
    public AirportName Name { get; private set; }
    public AirportIataCode IataCode { get; private set; }
    public AirportIcaoCode? IcaoCode { get; private set; }
    public AirportCityId CityId { get; private set; }

    private Airport(
        AirportId id,
        AirportName name,
        AirportIataCode iataCode,
        AirportIcaoCode? icaoCode,
        AirportCityId cityId)
    {
        Id = id;
        Name = name;
        IataCode = iataCode;
        IcaoCode = icaoCode;
        CityId = cityId;
    }

    public static Airport Create(
        AirportId id,
        AirportName name,
        AirportIataCode iataCode,
        AirportIcaoCode? icaoCode,
        AirportCityId cityId)
    {
        return new Airport(id, name, iataCode, icaoCode, cityId);
    }

    public static Airport CreateNew(
        AirportName name,
        AirportIataCode iataCode,
        AirportIcaoCode? icaoCode,
        AirportCityId cityId)
    {
        return new Airport(AirportId.CreateEmpty(), name, iataCode, icaoCode, cityId);
    }
}
