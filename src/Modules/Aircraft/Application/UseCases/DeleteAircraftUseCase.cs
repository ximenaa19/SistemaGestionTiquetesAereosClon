// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\Application\UseCases\DeleteAircraftUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Domain.Repositories;
using GestionAerolineas.src.Modules.Aircraft.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Aircraft.Application.UseCases;

public class DeleteAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public DeleteAircraftUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(AircraftId.Create(id));
        if (entity is null)
            return;

        await _repository.DeleteAsync(entity);
    }
}

