// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Domain\Repositories\IPhoneCodeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PhoneCodes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PhoneCodes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PhoneCodes.Domain.Repositories;

public interface IPhoneCodeRepository
{
    Task<IEnumerable<PhoneCode>> GetAllAsync();
    Task<PhoneCode?> GetByIdAsync(PhoneCodeId id);
    Task<PhoneCode?> GetByCountryCodeAsync(PhoneCountryCode countryCode);
    Task<PhoneCode?> GetByCountryNameAsync(CountryName countryName);
    Task AddAsync(PhoneCode phoneCode);
    Task UpdateAsync(PhoneCode phoneCode);
    Task DeleteAsync(PhoneCode phoneCode);
    Task<bool> ExistsAsync(PhoneCodeId id);
}
