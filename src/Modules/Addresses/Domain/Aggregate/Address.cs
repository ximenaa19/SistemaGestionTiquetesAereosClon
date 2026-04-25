// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Addresses\Domain\Aggregate\Address.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Addresses.Domain.Aggregate;

public sealed record Address
{
    public AddressId Id { get; private set; }
    public AddressRoadTypeId RoadTypeId { get; private set; }
    public AddressRoadName RoadName { get; private set; }
    public AddressNumber Number { get; private set; }
    public AddressComplement Complement { get; private set; }
    public AddressCityId CityId { get; private set; }
    public AddressPostalCode PostalCode { get; private set; }

    private Address(
        AddressId id,
        AddressRoadTypeId roadTypeId,
        AddressRoadName roadName,
        AddressNumber number,
        AddressComplement complement,
        AddressCityId cityId,
        AddressPostalCode postalCode)
    {
        Id = id;
        RoadTypeId = roadTypeId;
        RoadName = roadName;
        Number = number;
        Complement = complement;
        CityId = cityId;
        PostalCode = postalCode;
    }

    public static Address Create(
        AddressId id,
        AddressRoadTypeId roadTypeId,
        AddressRoadName roadName,
        AddressNumber number,
        AddressComplement complement,
        AddressCityId cityId,
        AddressPostalCode postalCode)
    {
        return new Address(id, roadTypeId, roadName, number, complement, cityId, postalCode);
    }

    public static Address CreateNew(
        AddressRoadTypeId roadTypeId,
        AddressRoadName roadName,
        AddressNumber number,
        AddressComplement complement,
        AddressCityId cityId,
        AddressPostalCode postalCode)
    {
        return new Address(AddressId.CreateEmpty(), roadTypeId, roadName, number, complement, cityId, postalCode);
    }
}

