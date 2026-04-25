// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftModels\Application\UseCases\DeleteAircraftModelUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftModels.Domain.Repositories;
using GestionAerolineas.src.Modules.AircraftModels.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AircraftModels.Application.UseCases;

public class DeleteAircraftModelUseCase
{
    private readonly IAircraftModelRepository _repository;

    public DeleteAircraftModelUseCase(IAircraftModelRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var idVO = AircraftModelId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);
        if (existing is null)
            throw new Exception("El modelo no existe");

        await _repository.DeleteAsync(existing);
    }
}

