// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightRoles\Application\UseCases\DeleteFlightRoleUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightRoles.Application.UseCases;

public class DeleteFlightRoleUseCase
{
    private readonly IFlightRoleRepository _repository;

    public DeleteFlightRoleUseCase(IFlightRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var flightRoleId = FlightRoleId.Create(id);
        var flightRole = await _repository.GetByIdAsync(flightRoleId);

        if (flightRole is null)
        {
            throw new KeyNotFoundException($"FlightRole con id '{flightRoleId.Value}' no existe.");
        }

        await _repository.DeleteAsync(flightRole);
    }
}

