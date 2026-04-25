// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reports\UI\ReportsMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reports.Application.UseCases;
using GestionAerolineas.src.shared.Ui.RoleMenus;

namespace GestionAerolineas.src.Modules.Reports.UI;

/// <summary>
/// Menú de reportes para administración.
/// Expone consultas LINQ orientadas a métricas operativas y comerciales.
/// </summary>
public sealed class ReportsMenu
{
    private readonly GetFlightsWithHighestOccupancyUseCase _highestOccupancy;
    private readonly GetFlightsWithAvailableSeatsUseCase _availableSeats;
    private readonly GetTopCustomersByReservationsUseCase _topCustomers;
    private readonly GetMostRequestedDestinationsUseCase _topDestinations;
    private readonly GetReservationsByStatusUseCase _reservationsByStatus;
    private readonly GetEstimatedIncomeByAirlineUseCase _incomeByAirline;
    private readonly GetIssuedTicketsByDateRangeUseCase _issuedTicketsByDateRange;

    public ReportsMenu(
        GetFlightsWithHighestOccupancyUseCase highestOccupancy,
        GetFlightsWithAvailableSeatsUseCase availableSeats,
        GetTopCustomersByReservationsUseCase topCustomers,
        GetMostRequestedDestinationsUseCase topDestinations,
        GetReservationsByStatusUseCase reservationsByStatus,
        GetEstimatedIncomeByAirlineUseCase incomeByAirline,
        GetIssuedTicketsByDateRangeUseCase issuedTicketsByDateRange)
    {
        _highestOccupancy = highestOccupancy;
        _availableSeats = availableSeats;
        _topCustomers = topCustomers;
        _topDestinations = topDestinations;
        _reservationsByStatus = reservationsByStatus;
        _incomeByAirline = incomeByAirline;
        _issuedTicketsByDateRange = issuedTicketsByDateRange;
    }

    /// <summary>
    /// Muestra el submenú de reportes y delega cada opción al caso de uso correspondiente.
    /// </summary>
    public Task StartAsync()
    {
        var menu = new RoleMenu("REPORTES LINQ", new List<RoleMenuOption>
        {
            new("Vuelos con mayor ocupacion", ShowHighestOccupancyAsync),
            new("Vuelos con asientos disponibles", ShowFlightsWithAvailableSeatsAsync),
            new("Clientes con mas reservas", ShowTopCustomersAsync),
            new("Destinos mas solicitados", ShowTopDestinationsAsync),
            new("Reservas por estado", ShowReservationsByStatusAsync),
            new("Ingresos estimados por aerolinea", ShowIncomeByAirlineAsync),
            new("Tiquetes emitidos por rango", ShowTicketsByRangeAsync)
        }, "Volver");

        return menu.StartAsync();
    }

    /// <summary>
    /// Lista vuelos ordenados por porcentaje de ocupación.
    /// </summary>
    private async Task ShowHighestOccupancyAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Vuelos con mayor ocupacion ===\n");
        var list = await _highestOccupancy.ExecuteAsync();
        if (list.Count == 0)
        {
            Console.WriteLine("No hay datos.");
            Pause();
            return;
        }

        foreach (var item in list)
        {
            Console.WriteLine(
                $"Flight {item.FlightId} ({item.FlightCode}) | ocupados={item.OccupiedSeats}/{item.TotalCapacity} | disponibles={item.AvailableSeats} | ocupacion={item.OccupancyPercent}%");
        }

        Pause();
    }

    /// <summary>
    /// Lista vuelos que aún tienen asientos disponibles.
    /// </summary>
    private async Task ShowFlightsWithAvailableSeatsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Vuelos con asientos disponibles ===\n");
        var list = await _availableSeats.ExecuteAsync();
        if (list.Count == 0)
        {
            Console.WriteLine("No hay datos.");
            Pause();
            return;
        }

        foreach (var item in list)
            Console.WriteLine($"Flight {item.FlightId} ({item.FlightCode}) | capacidad={item.TotalCapacity} | disponibles={item.AvailableSeats}");

        Pause();
    }

    /// <summary>
    /// Muestra clientes con mayor número de reservas acumuladas.
    /// </summary>
    private async Task ShowTopCustomersAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Clientes con mas reservas ===\n");
        var list = await _topCustomers.ExecuteAsync();
        if (list.Count == 0)
        {
            Console.WriteLine("No hay datos.");
            Pause();
            return;
        }

        foreach (var item in list)
            Console.WriteLine($"Customer {item.CustomerId} - {item.CustomerName} | reservas={item.TotalReservations}");

        Pause();
    }

    /// <summary>
    /// Muestra destinos (aeropuertos) más solicitados según reservas.
    /// </summary>
    private async Task ShowTopDestinationsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Destinos mas solicitados ===\n");
        var list = await _topDestinations.ExecuteAsync();
        if (list.Count == 0)
        {
            Console.WriteLine("No hay datos.");
            Pause();
            return;
        }

        foreach (var item in list)
            Console.WriteLine($"Airport {item.AirportId} - {item.AirportName} ({item.AirportIataCode}) | reservas={item.TotalReservations}");

        Pause();
    }

    /// <summary>
    /// Agrupa reservas por estado y muestra su cantidad.
    /// </summary>
    private async Task ShowReservationsByStatusAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Reservas por estado ===\n");
        var list = await _reservationsByStatus.ExecuteAsync();
        if (list.Count == 0)
        {
            Console.WriteLine("No hay datos.");
            Pause();
            return;
        }

        foreach (var item in list)
            Console.WriteLine($"Status {item.StatusId} - {item.StatusName} | reservas={item.TotalReservations}");

        Pause();
    }

    /// <summary>
    /// Calcula ingresos estimados por aerolínea.
    /// </summary>
    private async Task ShowIncomeByAirlineAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Ingresos estimados por aerolinea ===\n");
        var list = await _incomeByAirline.ExecuteAsync();
        if (list.Count == 0)
        {
            Console.WriteLine("No hay datos.");
            Pause();
            return;
        }

        foreach (var item in list)
            Console.WriteLine($"Airline {item.AirlineId} - {item.AirlineName} ({item.AirlineIataCode}) | ingreso={item.EstimatedIncome:N2}");

        Pause();
    }

    /// <summary>
    /// Cuenta tiquetes emitidos por día dentro de un rango de fechas.
    /// </summary>
    private async Task ShowTicketsByRangeAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Tiquetes emitidos por rango de fechas ===\n");

        if (!TryReadDate("Fecha inicio (yyyy-MM-dd)", out var start))
            return;
        if (!TryReadDate("Fecha fin (yyyy-MM-dd)", out var end))
            return;

        if (end < start)
        {
            Console.WriteLine("La fecha fin no puede ser menor a la fecha inicio.");
            Pause();
            return;
        }

        var endInclusive = end.Date.AddDays(1).AddTicks(-1);
        var list = await _issuedTicketsByDateRange.ExecuteAsync(start.Date, endInclusive);

        if (list.Count == 0)
        {
            Console.WriteLine("No hay datos en el rango.");
            Pause();
            return;
        }

        var total = list.Sum(x => x.TotalTickets);
        foreach (var item in list)
            Console.WriteLine($"{item.Date:yyyy-MM-dd} | tiquetes={item.TotalTickets}");

        Console.WriteLine($"\nTOTAL TIQUETES: {total}");
        Pause();
    }

    /// <summary>
    /// Intenta leer una fecha estricta en formato yyyy-MM-dd.
    /// </summary>
    private static bool TryReadDate(string label, out DateTime value)
    {
        Console.Write($"{label}: ");
        var raw = Console.ReadLine();
        if (DateTime.TryParseExact(raw, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out value))
            return true;

        Console.WriteLine("Formato invalido. Debe ser yyyy-MM-dd.");
        Pause();
        return false;
    }

    /// <summary>
    /// Pausa estándar para mantener visible la salida del reporte.
    /// </summary>
    private static void Pause()
    {
        Console.WriteLine("\nPresiona una tecla para continuar...");
        Console.ReadKey();
    }
}
