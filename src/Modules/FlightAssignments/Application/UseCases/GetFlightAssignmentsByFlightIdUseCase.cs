// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\Application\UseCases\GetFlightAssignmentsByFlightIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightAssignments.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightAssignments.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightAssignments.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightAssignments.Application.UseCases;

public class GetFlightAssignmentsByFlightIdUseCase
{
    private readonly IFlightAssignmentRepository _repository;

    public GetFlightAssignmentsByFlightIdUseCase(IFlightAssignmentRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<FlightAssignment>> ExecuteAsync(int flightId)
    {
        return _repository.GetByFlightIdAsync(FlightAssignmentFlightId.Create(flightId));
    }
}

