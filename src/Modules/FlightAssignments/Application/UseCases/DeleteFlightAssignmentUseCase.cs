// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\Application\UseCases\DeleteFlightAssignmentUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightAssignments.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightAssignments.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightAssignments.Application.UseCases;

public class DeleteFlightAssignmentUseCase
{
    private readonly IFlightAssignmentRepository _repository;

    public DeleteFlightAssignmentUseCase(IFlightAssignmentRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(FlightAssignmentId.Create(id));
        if (entity is null)
            return;

        await _repository.DeleteAsync(entity);
    }
}

