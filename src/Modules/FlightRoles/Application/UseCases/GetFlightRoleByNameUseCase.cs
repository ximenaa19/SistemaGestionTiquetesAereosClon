// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightRoles\Application\UseCases\GetFlightRoleByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightRoles.Application.UseCases;

public class GetFlightRoleByNameUseCase
{
    private readonly IFlightRoleRepository _repository;

    public GetFlightRoleByNameUseCase(IFlightRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<FlightRole?> ExecuteAsync(string name)
    {
        var nameVO = FlightRoleName.Create(name);
        return await _repository.GetByNameAsync(nameVO);
    }
}

