// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Application\UseCases\DeletePhoneCodeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PhoneCodes.Domain.Repositories;
using GestionAerolineas.src.Modules.PhoneCodes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PhoneCodes.Application.UseCases;

public class DeletePhoneCodeUseCase
{
    private readonly IPhoneCodeRepository _repository;

    public DeletePhoneCodeUseCase(IPhoneCodeRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var phoneCodeId = PhoneCodeId.Create(id);
        var phoneCode = await _repository.GetByIdAsync(phoneCodeId);

        if (phoneCode is null)
        {
            throw new KeyNotFoundException($"PhoneCode con id '{phoneCodeId.Value}' no existe.");
        }

        await _repository.DeleteAsync(phoneCode);
    }
}

