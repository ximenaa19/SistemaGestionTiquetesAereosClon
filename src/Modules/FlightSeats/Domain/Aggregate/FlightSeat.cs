// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightSeats\Domain\Aggregate\FlightSeat.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightSeats.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.FlightSeats.Domain.Aggregate;

public class FlightSeat
{
    public FlightSeatId Id { get; private set; }
    public FlightSeatFlightId FlightId { get; private set; }
    public FlightSeatCode Code { get; private set; }
    public FlightSeatCabinTypeId CabinTypeId { get; private set; }
    public FlightSeatLocationTypeId LocationTypeId { get; private set; }
    public FlightSeatIsOccupied IsOccupied { get; private set; }

    private FlightSeat(
        FlightSeatId id,
        FlightSeatFlightId flightId,
        FlightSeatCode code,
        FlightSeatCabinTypeId cabinTypeId,
        FlightSeatLocationTypeId locationTypeId,
        FlightSeatIsOccupied isOccupied)
    {
        Id = id;
        FlightId = flightId;
        Code = code;
        CabinTypeId = cabinTypeId;
        LocationTypeId = locationTypeId;
        IsOccupied = isOccupied;
    }

    public static FlightSeat Create(
        FlightSeatId id,
        FlightSeatFlightId flightId,
        FlightSeatCode code,
        FlightSeatCabinTypeId cabinTypeId,
        FlightSeatLocationTypeId locationTypeId,
        FlightSeatIsOccupied isOccupied)
    {
        return new FlightSeat(id, flightId, code, cabinTypeId, locationTypeId, isOccupied);
    }

    public static FlightSeat CreateNew(
        FlightSeatFlightId flightId,
        FlightSeatCode code,
        FlightSeatCabinTypeId cabinTypeId,
        FlightSeatLocationTypeId locationTypeId,
        FlightSeatIsOccupied? isOccupied = null)
    {
        return new FlightSeat(
            FlightSeatId.CreateEmpty(),
            flightId,
            code,
            cabinTypeId,
            locationTypeId,
            isOccupied ?? FlightSeatIsOccupied.Create(false));
    }
}

