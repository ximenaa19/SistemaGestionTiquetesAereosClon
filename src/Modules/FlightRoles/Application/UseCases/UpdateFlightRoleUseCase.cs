// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightRoles\Application\UseCases\UpdateFlightRoleUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightRoles.Application.Interfaces;
using GestionAerolineas.src.Modules.FlightRoles.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightRoles.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightRoles.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightRoles.Application.UseCases;

public class UpdateFlightRoleUseCase
{
    private readonly IFlightRoleRepository _repository;
    private readonly IFlightRoleValidator _validator;

    public UpdateFlightRoleUseCase(
        IFlightRoleRepository repository,
        IFlightRoleValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name)
    {
        var idVO = FlightRoleId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing == null)
            throw new Exception("El FlightRole no existe");

        var nameVO = FlightRoleName.Create(name);

        await _validator.ValidateNameAsync(nameVO);

        var updated = FlightRole.Create(idVO, nameVO);

        await _repository.UpdateAsync(updated);
    }
}

