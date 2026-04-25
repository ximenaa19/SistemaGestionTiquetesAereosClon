// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Application\Services\PhoneCodeValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PhoneCodes.Application.Interfaces;
using GestionAerolineas.src.Modules.PhoneCodes.Domain.Repositories;
using GestionAerolineas.src.Modules.PhoneCodes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PhoneCodes.Application.Services;

public class PhoneCodeValidator : IPhoneCodeValidator
{
    private readonly IPhoneCodeRepository _repository;

    public PhoneCodeValidator(IPhoneCodeRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateCountryCodeAsync(PhoneCountryCode countryCode, PhoneCodeId? currentId = null)
    {
        var existing = await _repository.GetByCountryCodeAsync(countryCode);

        if (existing != null && (currentId is null || existing.Id.Value != currentId.Value))
            throw new Exception("Ya existe un PhoneCode con ese código país");
    }
}
