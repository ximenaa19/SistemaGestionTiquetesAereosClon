// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationFlights\Application\UseCases\GetReservationFlightsByReservationCodeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;

public class GetReservationFlightsByReservationCodeUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationFlightRepository _reservationFlightRepository;

    public GetReservationFlightsByReservationCodeUseCase(
        IReservationRepository reservationRepository,
        IReservationFlightRepository reservationFlightRepository)
    {
        _reservationRepository = reservationRepository;
        _reservationFlightRepository = reservationFlightRepository;
    }

    public async Task<IEnumerable<ReservationFlight>> ExecuteAsync(string reservationCode)
    {
        var reservation = await _reservationRepository.GetByCodeAsync(ReservationCode.Create(reservationCode));
        if (reservation is null)
            return Enumerable.Empty<ReservationFlight>();

        return await _reservationFlightRepository.GetByReservationIdAsync(
            ReservationFlightReservationId.Create(reservation.Id.Value));
    }
}

