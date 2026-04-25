// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\Application\UseCases\GetAllFlightAssignmentsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightAssignments.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightAssignments.Domain.Repositories;

namespace GestionAerolineas.src.Modules.FlightAssignments.Application.UseCases;

public class GetAllFlightAssignmentsUseCase
{
    private readonly IFlightAssignmentRepository _repository;

    public GetAllFlightAssignmentsUseCase(IFlightAssignmentRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<FlightAssignment>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

