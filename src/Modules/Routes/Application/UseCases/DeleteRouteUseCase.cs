// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Application\UseCases\DeleteRouteUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Routes.Domain.Repositories;
using GestionAerolineas.src.Modules.Routes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Routes.Application.UseCases;

public class DeleteRouteUseCase
{
    private readonly IRouteRepository _repository;

    public DeleteRouteUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(RouteId.Create(id));
        if (entity is null)
            return;

        await _repository.DeleteAsync(entity);
    }
}

