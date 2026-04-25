// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\Application\UseCases\CreateFlightAssignmentUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightAssignments.Application.Interfaces;
using GestionAerolineas.src.Modules.FlightAssignments.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightAssignments.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightAssignments.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightAssignments.Application.UseCases;

public class CreateFlightAssignmentUseCase
{
    private readonly IFlightAssignmentRepository _repository;
    private readonly IFlightAssignmentValidator _validator;

    public CreateFlightAssignmentUseCase(IFlightAssignmentRepository repository, IFlightAssignmentValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int flightId, int staffId, int flightRoleId)
    {
        var flightIdVO = FlightAssignmentFlightId.Create(flightId);
        var staffIdVO = FlightAssignmentStaffId.Create(staffId);
        var flightRoleIdVO = FlightAssignmentFlightRoleId.Create(flightRoleId);

        await _validator.ValidateFlightExistsAsync(flightIdVO);
        await _validator.ValidateFlightNotInFinalStateAsync(flightIdVO);
        await _validator.ValidateStaffExistsAndActiveAsync(staffIdVO);
        await _validator.ValidateFlightRoleExistsAsync(flightRoleIdVO);
        await _validator.ValidateUniqueFlightAndStaffAsync(flightIdVO, staffIdVO);
        await _validator.ValidateNoStaffOverlapAsync(staffIdVO, flightIdVO);
        await _validator.ValidateStaffAirlineConsistencyAsync(staffIdVO, flightIdVO);
        await _validator.ValidateAirportStaffMatchesRouteAsync(staffIdVO, flightIdVO);

        var entity = FlightAssignment.CreateNew(flightIdVO, staffIdVO, flightRoleIdVO);
        await _repository.AddAsync(entity);
    }
}

