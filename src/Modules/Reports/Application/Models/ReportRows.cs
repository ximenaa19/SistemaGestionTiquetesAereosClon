// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reports\Application\Models\ReportRows.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Reports.Application.Models;

public sealed record FlightOccupancyRow(
    int FlightId,
    string FlightCode,
    int TotalCapacity,
    int OccupiedSeats,
    int AvailableSeats,
    decimal OccupancyPercent
);

public sealed record FlightAvailabilityRow(
    int FlightId,
    string FlightCode,
    int TotalCapacity,
    int AvailableSeats
);

public sealed record CustomerReservationRow(
    int CustomerId,
    string CustomerName,
    int TotalReservations
);

public sealed record DestinationDemandRow(
    int AirportId,
    string AirportName,
    string AirportIataCode,
    int TotalReservations
);

public sealed record ReservationStatusCountRow(
    int StatusId,
    string StatusName,
    int TotalReservations
);

public sealed record AirlineIncomeRow(
    int AirlineId,
    string AirlineName,
    string AirlineIataCode,
    decimal EstimatedIncome
);

public sealed record IssuedTicketsByDateRow(
    DateOnly Date,
    int TotalTickets
);
