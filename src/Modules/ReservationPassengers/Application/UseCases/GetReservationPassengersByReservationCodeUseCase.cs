// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationPassengers\Application\UseCases\GetReservationPassengersByReservationCodeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;

public class GetReservationPassengersByReservationCodeUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationFlightRepository _reservationFlightRepository;
    private readonly IReservationPassengerRepository _reservationPassengerRepository;

    public GetReservationPassengersByReservationCodeUseCase(
        IReservationRepository reservationRepository,
        IReservationFlightRepository reservationFlightRepository,
        IReservationPassengerRepository reservationPassengerRepository)
    {
        _reservationRepository = reservationRepository;
        _reservationFlightRepository = reservationFlightRepository;
        _reservationPassengerRepository = reservationPassengerRepository;
    }

    public async Task<IEnumerable<ReservationPassenger>> ExecuteAsync(string reservationCode)
    {
        var reservation = await _reservationRepository.GetByCodeAsync(ReservationCode.Create(reservationCode));
        if (reservation is null)
            return Enumerable.Empty<ReservationPassenger>();

        var reservationFlights = await _reservationFlightRepository.GetByReservationIdAsync(
            ReservationFlightReservationId.Create(reservation.Id.Value));

        var result = new List<ReservationPassenger>();
        foreach (var rf in reservationFlights)
        {
            var passengers = await _reservationPassengerRepository.GetByReservationFlightIdAsync(
                ReservationPassengers.Domain.ValueObject.ReservationPassengerReservationFlightId.Create(rf.Id.Value));
            result.AddRange(passengers);
        }

        return result;
    }
}

