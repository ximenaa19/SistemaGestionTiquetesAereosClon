// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightRoles\Application\UseCases\GetAllFlightRolesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightRoles.Domain.Repositories;

namespace GestionAerolineas.src.Modules.FlightRoles.Application.UseCases;

public class GetAllFlightRolesUseCase
{
    private readonly IFlightRoleRepository _repository;

    public GetAllFlightRolesUseCase(IFlightRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<FlightRole>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}

