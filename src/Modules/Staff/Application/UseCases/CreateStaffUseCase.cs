// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Staff\Application\UseCases\CreateStaffUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Staff.Application.Interfaces;
using GestionAerolineas.src.Modules.Staff.Domain.Aggregate;
using GestionAerolineas.src.Modules.Staff.Domain.Repositories;
using GestionAerolineas.src.Modules.Staff.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Staff.Application.UseCases;

public class CreateStaffUseCase
{
    private readonly IStaffRepository _repository;
    private readonly IStaffValidator _validator;

    public CreateStaffUseCase(IStaffRepository repository, IStaffValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(
        int personId,
        int roleId,
        int? airlineId,
        int? airportId,
        DateTime hireDate,
        bool isActive)
    {
        var personIdVO = StaffPersonId.Create(personId);
        var roleIdVO = StaffRoleId.Create(roleId);
        var airlineIdVO = StaffAirlineId.Create(airlineId);
        var airportIdVO = StaffAirportId.Create(airportId);
        var hireDateVO = StaffHireDate.Create(hireDate);
        var isActiveVO = StaffIsActive.Create(isActive);

        await _validator.ValidatePersonExistsAsync(personIdVO);
        await _validator.ValidateRoleExistsAsync(roleIdVO);
        _validator.ValidateHasAirlineOrAirport(airlineIdVO, airportIdVO);
        await _validator.ValidateOptionalAirlineExistsAsync(airlineIdVO);
        await _validator.ValidateOptionalAirportExistsAsync(airportIdVO);
        await _validator.ValidateUniquePersonAsync(personIdVO);

        var entity = StaffMember.CreateNew(personIdVO, roleIdVO, airlineIdVO, airportIdVO, hireDateVO, isActiveVO);
        await _repository.AddAsync(entity);
    }
}

