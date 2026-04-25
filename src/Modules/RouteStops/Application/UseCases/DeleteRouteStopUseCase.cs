// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RouteStops\Application\UseCases\DeleteRouteStopUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RouteStops.Domain.Repositories;
using GestionAerolineas.src.Modules.RouteStops.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RouteStops.Application.UseCases;

public class DeleteRouteStopUseCase
{
    private readonly IRouteStopRepository _repository;

    public DeleteRouteStopUseCase(IRouteStopRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(RouteStopId.Create(id));
        if (entity is null)
            return;

        await _repository.DeleteAsync(entity);
    }
}

