// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Application\UseCases\GetAllPhoneCodesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PhoneCodes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PhoneCodes.Domain.Repositories;

namespace GestionAerolineas.src.Modules.PhoneCodes.Application.UseCases;

public class GetAllPhoneCodesUseCase
{
    private readonly IPhoneCodeRepository _repository;

    public GetAllPhoneCodesUseCase(IPhoneCodeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PhoneCode>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}

