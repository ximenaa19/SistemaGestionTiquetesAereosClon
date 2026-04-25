// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightRoles\Application\UseCases\GetFlightRoleByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightRoles.Application.UseCases;

public class GetFlightRoleByIdUseCase
{
    private readonly IFlightRoleRepository _repository;

    public GetFlightRoleByIdUseCase(IFlightRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<FlightRole?> ExecuteAsync(int id)
    {
        var idVO = FlightRoleId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}

