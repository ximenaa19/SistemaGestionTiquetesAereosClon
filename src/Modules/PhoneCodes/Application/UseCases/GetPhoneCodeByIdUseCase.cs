// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Application\UseCases\GetPhoneCodeByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PhoneCodes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PhoneCodes.Domain.Repositories;
using GestionAerolineas.src.Modules.PhoneCodes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PhoneCodes.Application.UseCases;

public class GetPhoneCodeByIdUseCase
{
    private readonly IPhoneCodeRepository _repository;

    public GetPhoneCodeByIdUseCase(IPhoneCodeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PhoneCode?> ExecuteAsync(int id)
    {
        var idVO = PhoneCodeId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}

