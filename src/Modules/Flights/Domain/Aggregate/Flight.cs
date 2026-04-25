// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Flights\Domain\Aggregate\Flight.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Flights.Domain.Aggregate;

public class Flight
{
    public FlightId Id { get; private set; }
    public FlightCode Code { get; private set; }
    public FlightAirlineId AirlineId { get; private set; }
    public FlightRouteId RouteId { get; private set; }
    public FlightAircraftId AircraftId { get; private set; }
    public FlightDepartureDateTime DepartureDateTime { get; private set; }
    public FlightEstimatedArrivalDateTime EstimatedArrivalDateTime { get; private set; }
    public FlightTotalCapacity TotalCapacity { get; private set; }
    public FlightAvailableSeats AvailableSeats { get; private set; }
    public FlightStateId StateId { get; private set; }
    public FlightRescheduledAt RescheduledAt { get; private set; }

    private Flight(
        FlightId id,
        FlightCode code,
        FlightAirlineId airlineId,
        FlightRouteId routeId,
        FlightAircraftId aircraftId,
        FlightDepartureDateTime departureDateTime,
        FlightEstimatedArrivalDateTime estimatedArrivalDateTime,
        FlightTotalCapacity totalCapacity,
        FlightAvailableSeats availableSeats,
        FlightStateId stateId,
        FlightRescheduledAt rescheduledAt)
    {
        Id = id;
        Code = code;
        AirlineId = airlineId;
        RouteId = routeId;
        AircraftId = aircraftId;
        DepartureDateTime = departureDateTime;
        EstimatedArrivalDateTime = estimatedArrivalDateTime;
        TotalCapacity = totalCapacity;
        AvailableSeats = availableSeats;
        StateId = stateId;
        RescheduledAt = rescheduledAt;
    }

    public static Flight Create(
        FlightId id,
        FlightCode code,
        FlightAirlineId airlineId,
        FlightRouteId routeId,
        FlightAircraftId aircraftId,
        FlightDepartureDateTime departureDateTime,
        FlightEstimatedArrivalDateTime estimatedArrivalDateTime,
        FlightTotalCapacity totalCapacity,
        FlightAvailableSeats availableSeats,
        FlightStateId stateId,
        FlightRescheduledAt rescheduledAt)
    {
        return new Flight(
            id,
            code,
            airlineId,
            routeId,
            aircraftId,
            departureDateTime,
            estimatedArrivalDateTime,
            totalCapacity,
            availableSeats,
            stateId,
            rescheduledAt);
    }

    public static Flight CreateNew(
        FlightCode code,
        FlightAirlineId airlineId,
        FlightRouteId routeId,
        FlightAircraftId aircraftId,
        FlightDepartureDateTime departureDateTime,
        FlightEstimatedArrivalDateTime estimatedArrivalDateTime,
        FlightTotalCapacity totalCapacity,
        FlightAvailableSeats availableSeats,
        FlightStateId stateId,
        FlightRescheduledAt rescheduledAt)
    {
        return new Flight(
            FlightId.CreateEmpty(),
            code,
            airlineId,
            routeId,
            aircraftId,
            departureDateTime,
            estimatedArrivalDateTime,
            totalCapacity,
            availableSeats,
            stateId,
            rescheduledAt);
    }
}

