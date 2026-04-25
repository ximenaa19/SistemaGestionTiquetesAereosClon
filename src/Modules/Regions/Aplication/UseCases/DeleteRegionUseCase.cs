// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Regions\Aplication\UseCases\DeleteRegionUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Regions.Domain.Repositories;
using GestionAerolineas.src.Modules.Regions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Regions.Application.UseCases;

public class DeleteRegionUseCase
{
    private readonly IRegionRepository _repository;

    public DeleteRegionUseCase(IRegionRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(RegionId.Create(id));
        if (entity is null)
            return;

        await _repository.DeleteAsync(entity);
    }
}

