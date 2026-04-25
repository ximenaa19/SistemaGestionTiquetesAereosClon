// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Domain\Aggregate\PhoneCode.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PhoneCodes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PhoneCodes.Domain.Aggregate;

public class PhoneCode
{
    public PhoneCodeId Id { get; private set; }
    public PhoneCountryCode CountryCode { get; private set; }
    public CountryName CountryName { get; private set; }

    private PhoneCode(PhoneCodeId id, PhoneCountryCode countryCode, CountryName countryName)
    {
        Id = id;
        CountryCode = countryCode;
        CountryName = countryName;
    }

    public static PhoneCode Create(PhoneCodeId id, PhoneCountryCode countryCode, CountryName countryName)
    {
        return new PhoneCode(id, countryCode, countryName);
    }

    public static PhoneCode CreateNew(PhoneCountryCode countryCode, CountryName countryName)
    {
        return new PhoneCode(PhoneCodeId.CreateEmpty(), countryCode, countryName);
    }
}

