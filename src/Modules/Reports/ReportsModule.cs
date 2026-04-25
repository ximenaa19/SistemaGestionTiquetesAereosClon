// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reports\ReportsModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reports.Application.UseCases;
using GestionAerolineas.src.Modules.Reports.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Reports;

/// <summary>
/// Punto de composición del módulo de reportes.
/// Construye casos de uso LINQ y los inyecta en el menú de consola.
/// </summary>
public static class ReportsModule
{
    /// <summary>
    /// Crea una instancia lista del menú de reportes usando el contexto compartido.
    /// </summary>
    public static ReportsMenu Build(AppDbContext context)
    {
        var highestOccupancy = new GetFlightsWithHighestOccupancyUseCase(context);
        var availableSeats = new GetFlightsWithAvailableSeatsUseCase(context);
        var topCustomers = new GetTopCustomersByReservationsUseCase(context);
        var topDestinations = new GetMostRequestedDestinationsUseCase(context);
        var reservationsByStatus = new GetReservationsByStatusUseCase(context);
        var incomeByAirline = new GetEstimatedIncomeByAirlineUseCase(context);
        var issuedTicketsByDateRange = new GetIssuedTicketsByDateRangeUseCase(context);

        return new ReportsMenu(
            highestOccupancy,
            availableSeats,
            topCustomers,
            topDestinations,
            reservationsByStatus,
            incomeByAirline,
            issuedTicketsByDateRange
        );
    }
}
