// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Application\UseCases\GetAllPeopleUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Domain.Aggregate;
using GestionAerolineas.src.Modules.People.Domain.Repositories;

namespace GestionAerolineas.src.Modules.People.Application.UseCases;

public class GetAllPeopleUseCase
{
    private readonly IPersonRepository _repository;

    public GetAllPeopleUseCase(IPersonRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Person>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

