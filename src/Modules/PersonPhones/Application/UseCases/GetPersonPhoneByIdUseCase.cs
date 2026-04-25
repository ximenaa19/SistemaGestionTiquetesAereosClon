// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonPhones\Application\UseCases\GetPersonPhoneByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonPhones.Domain.Aggregate;
using GestionAerolineas.src.Modules.PersonPhones.Domain.Repositories;
using GestionAerolineas.src.Modules.PersonPhones.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PersonPhones.Application.UseCases;

public class GetPersonPhoneByIdUseCase
{
    private readonly IPersonPhoneRepository _repository;

    public GetPersonPhoneByIdUseCase(IPersonPhoneRepository repository)
    {
        _repository = repository;
    }

    public Task<PersonPhone?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(PersonPhoneId.Create(id));
    }
}

