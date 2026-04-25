// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Application\UseCases\GetAllReservationPassengersUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;

namespace GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;

public class GetAllReservationPassengersUseCase
{
    private readonly IReservationPassengerRepository _repository;

    public GetAllReservationPassengersUseCase(IReservationPassengerRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<ReservationPassenger>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

