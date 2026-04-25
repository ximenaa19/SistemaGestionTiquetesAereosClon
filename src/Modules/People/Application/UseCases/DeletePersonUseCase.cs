// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Application\UseCases\DeletePersonUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Domain.Repositories;
using GestionAerolineas.src.Modules.People.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.People.Application.UseCases;

public class DeletePersonUseCase
{
    private readonly IPersonRepository _repository;

    public DeletePersonUseCase(IPersonRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(PersonId.Create(id));
        if (entity is null)
            return;

        await _repository.DeleteAsync(entity);
    }
}

