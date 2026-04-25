// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Application\UseCases\GetReservationPassengerByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;

public class GetReservationPassengerByIdUseCase
{
    private readonly IReservationPassengerRepository _repository;

    public GetReservationPassengerByIdUseCase(IReservationPassengerRepository repository)
    {
        _repository = repository;
    }

    public Task<ReservationPassenger?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(ReservationPassengerId.Create(id));
    }
}

