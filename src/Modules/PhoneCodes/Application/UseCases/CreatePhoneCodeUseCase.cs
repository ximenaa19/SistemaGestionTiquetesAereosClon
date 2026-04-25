// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Application\UseCases\CreatePhoneCodeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PhoneCodes.Application.Interfaces;
using GestionAerolineas.src.Modules.PhoneCodes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PhoneCodes.Domain.Repositories;
using GestionAerolineas.src.Modules.PhoneCodes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PhoneCodes.Application.UseCases;

public class CreatePhoneCodeUseCase
{
    private readonly IPhoneCodeRepository _repository;
    private readonly IPhoneCodeValidator _validator;

    public CreatePhoneCodeUseCase(
        IPhoneCodeRepository repository,
        IPhoneCodeValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(string countryCode, string countryName)
    {
        var codeVO = PhoneCountryCode.Create(countryCode);
        var nameVO = CountryName.Create(countryName);

        await _validator.ValidateCountryCodeAsync(codeVO);

        var entity = PhoneCode.CreateNew(codeVO, nameVO);

        await _repository.AddAsync(entity);
    }
}

