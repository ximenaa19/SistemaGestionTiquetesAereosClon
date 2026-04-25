// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Domain\Repositories\IReservationPassengerRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;

public interface IReservationPassengerRepository
{
    Task<IEnumerable<ReservationPassenger>> GetAllAsync();
    Task<ReservationPassenger?> GetByIdAsync(ReservationPassengerId id);
    Task<IEnumerable<ReservationPassenger>> GetByReservationFlightIdAsync(ReservationPassengerReservationFlightId reservationFlightId);
    Task<IEnumerable<ReservationPassenger>> GetByPassengerIdAsync(ReservationPassengerPassengerId passengerId);
    Task<ReservationPassenger?> GetByReservationFlightAndPassengerAsync(ReservationPassengerReservationFlightId reservationFlightId, ReservationPassengerPassengerId passengerId);
    Task AddAsync(ReservationPassenger reservationPassenger);
    Task UpdateAsync(ReservationPassenger reservationPassenger);
    Task DeleteAsync(ReservationPassenger reservationPassenger);
    Task<bool> ExistsAsync(ReservationPassengerId id);
    Task<bool> ExistsByReservationFlightAndPassengerAsync(int reservationFlightId, int passengerId, int? excludingId = null);
}

