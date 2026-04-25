// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStatusTransitions\Application\UseCases\DeleteFlightStatusTransitionUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightStatusTransitions.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightStatusTransitions.Application.UseCases;

public class DeleteFlightStatusTransitionUseCase
{
    private readonly IFlightStatusTransitionRepository _repository;

    public DeleteFlightStatusTransitionUseCase(IFlightStatusTransitionRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var idVO = FlightStatusTransitionId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing is null)
            throw new Exception("La transición no existe");

        await _repository.DeleteAsync(existing);
    }
}

