// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonPhones\Application\UseCases\GetAllPersonPhonesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonPhones.Domain.Aggregate;
using GestionAerolineas.src.Modules.PersonPhones.Domain.Repositories;

namespace GestionAerolineas.src.Modules.PersonPhones.Application.UseCases;

public class GetAllPersonPhonesUseCase
{
    private readonly IPersonPhoneRepository _repository;

    public GetAllPersonPhonesUseCase(IPersonPhoneRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<PersonPhone>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

