// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightRoles\Application\Services\FlightRoleValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightRoles.Application.Interfaces;
using GestionAerolineas.src.Modules.FlightRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightRoles.Application.Services;

public class FlightRoleValidator : IFlightRoleValidator
{
    private readonly IFlightRoleRepository _repository;

    public FlightRoleValidator(IFlightRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateNameAsync(FlightRoleName name)
    {
        var existing = await _repository.GetByNameAsync(name);

        if (existing != null)
            throw new Exception("Ya existe un FlightRole con ese nombre");
    }
}

