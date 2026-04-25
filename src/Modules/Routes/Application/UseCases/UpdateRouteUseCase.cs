// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Routes\Application\UseCases\UpdateRouteUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Routes.Application.Interfaces;
using GestionAerolineas.src.Modules.Routes.Domain.Aggregate;
using GestionAerolineas.src.Modules.Routes.Domain.Repositories;
using GestionAerolineas.src.Modules.Routes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Routes.Application.UseCases;

public class UpdateRouteUseCase
{
    private readonly IRouteRepository _repository;
    private readonly IRouteValidator _validator;

    public UpdateRouteUseCase(IRouteRepository repository, IRouteValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, int originAirportId, int destinationAirportId, int? distanceKm, int? estimatedDurationMin)
    {
        var idVO = RouteId.Create(id);
        var originVO = RouteAirportId.Create(originAirportId);
        var destinationVO = RouteAirportId.Create(destinationAirportId);
        var distanceVO = RouteDistanceKm.Create(distanceKm);
        var durationVO = RouteEstimatedDurationMinutes.Create(estimatedDurationMin);

        await _validator.ValidateAirportExistsAsync(originVO);
        await _validator.ValidateAirportExistsAsync(destinationVO);
        await _validator.ValidateDifferentAirportsAsync(originVO, destinationVO);
        await _validator.ValidateUniquePairAsync(originVO, destinationVO, idVO);

        var entity = Route.Create(idVO, originVO, destinationVO, distanceVO, durationVO);
        await _repository.UpdateAsync(entity);
    }
}

