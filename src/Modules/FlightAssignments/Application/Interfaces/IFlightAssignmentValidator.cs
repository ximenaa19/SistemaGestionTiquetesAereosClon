// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\Application\Interfaces\IFlightAssignmentValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightAssignments.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightAssignments.Application.Interfaces;

public interface IFlightAssignmentValidator
{
    Task ValidateFlightExistsAsync(FlightAssignmentFlightId flightId);
    Task ValidateStaffExistsAndActiveAsync(FlightAssignmentStaffId staffId);
    Task ValidateFlightRoleExistsAsync(FlightAssignmentFlightRoleId flightRoleId);
    Task ValidateUniqueFlightAndStaffAsync(FlightAssignmentFlightId flightId, FlightAssignmentStaffId staffId, FlightAssignmentId? currentId = null);
    Task ValidateNoStaffOverlapAsync(FlightAssignmentStaffId staffId, FlightAssignmentFlightId flightId, FlightAssignmentId? currentId = null);
    Task ValidateStaffAirlineConsistencyAsync(FlightAssignmentStaffId staffId, FlightAssignmentFlightId flightId);
    Task ValidateAirportStaffMatchesRouteAsync(FlightAssignmentStaffId staffId, FlightAssignmentFlightId flightId);
    Task ValidateFlightNotInFinalStateAsync(FlightAssignmentFlightId flightId);
}

