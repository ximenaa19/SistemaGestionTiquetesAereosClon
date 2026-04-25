// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Application\UseCases\DeleteFareUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Fares.Domain.Repositories;
using GestionAerolineas.src.Modules.Fares.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Fares.Application.UseCases;

public class DeleteFareUseCase
{
    private readonly IFareRepository _repository;

    public DeleteFareUseCase(IFareRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(FareId.Create(id));
        if (entity is null)
            return;

        await _repository.DeleteAsync(entity);
    }
}

