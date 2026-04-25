// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Application\UseCases\DeleteCabinConfigurationUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Repositories;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CabinConfiguration.Application.UseCases;

public class DeleteCabinConfigurationUseCase
{
    private readonly ICabinConfigurationRepository _repository;

    public DeleteCabinConfigurationUseCase(ICabinConfigurationRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(CabinConfigurationId.Create(id));
        if (entity is null)
            return;

        await _repository.DeleteAsync(entity);
    }
}
