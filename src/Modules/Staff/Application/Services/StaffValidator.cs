// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Application\Services\StaffValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Airlines.Domain.ValueObject;
using GestionAerolineas.src.Modules.Airlines.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Airports.Domain.ValueObject;
using GestionAerolineas.src.Modules.Airports.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.Domain.ValueObject;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Staff.Application.Interfaces;
using GestionAerolineas.src.Modules.Staff.Domain.Repositories;
using GestionAerolineas.src.Modules.Staff.Domain.ValueObject;
using GestionAerolineas.src.Modules.StaffRoles.Infrastructure.Repository;
using StaffRolesStaffRoleId = GestionAerolineas.src.Modules.StaffRoles.Domain.ValueObject.StaffRoleId;

namespace GestionAerolineas.src.Modules.Staff.Application.Services;

public class StaffValidator : IStaffValidator
{
    private readonly IStaffRepository _repository;
    private readonly PersonRepository _personRepository;
    private readonly StaffRoleRepository _staffRoleRepository;
    private readonly AirlineRepository _airlineRepository;
    private readonly AirportRepository _airportRepository;

    public StaffValidator(
        IStaffRepository repository,
        PersonRepository personRepository,
        StaffRoleRepository staffRoleRepository,
        AirlineRepository airlineRepository,
        AirportRepository airportRepository)
    {
        _repository = repository;
        _personRepository = personRepository;
        _staffRoleRepository = staffRoleRepository;
        _airlineRepository = airlineRepository;
        _airportRepository = airportRepository;
    }

    public async Task ValidatePersonExistsAsync(StaffPersonId personId)
    {
        var exists = await _personRepository.ExistsAsync(PersonId.Create(personId.Value));
        if (!exists)
            throw new Exception("La persona no existe");
    }

    public async Task ValidateRoleExistsAsync(StaffRoleId roleId)
    {
        var exists = await _staffRoleRepository.ExistsAsync(StaffRolesStaffRoleId.Create(roleId.Value));
        if (!exists)
            throw new Exception("El cargo no existe");
    }

    public async Task ValidateOptionalAirlineExistsAsync(StaffAirlineId airlineId)
    {
        if (!airlineId.Value.HasValue)
            return;

        var exists = await _airlineRepository.ExistsAsync(AirlineId.Create(airlineId.Value.Value));
        if (!exists)
            throw new Exception("La aerolinea no existe");
    }

    public async Task ValidateOptionalAirportExistsAsync(StaffAirportId airportId)
    {
        if (!airportId.Value.HasValue)
            return;

        var exists = await _airportRepository.ExistsAsync(AirportId.Create(airportId.Value.Value));
        if (!exists)
            throw new Exception("El aeropuerto no existe");
    }

    public async Task ValidateUniquePersonAsync(StaffPersonId personId, StaffId? currentId = null)
    {
        var exists = await _repository.ExistsByPersonIdAsync(personId.Value, currentId?.Value);
        if (exists)
            throw new Exception("Esa persona ya existe como staff");
    }

    public void ValidateHasAirlineOrAirport(StaffAirlineId airlineId, StaffAirportId airportId)
    {
        if (!airlineId.Value.HasValue && !airportId.Value.HasValue)
            throw new Exception("Debe especificar aerolinea_id o aeropuerto_id (al menos uno)");
    }
}
