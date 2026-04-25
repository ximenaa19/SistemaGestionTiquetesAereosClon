// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reports\Application\UseCases\GetMostRequestedDestinationsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reports.Application.Models;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Reports.Application.UseCases;

/// <summary>
/// Calcula destinos más solicitados con base en reservas por aeropuerto destino.
/// </summary>
public sealed class GetMostRequestedDestinationsUseCase
{
    private readonly AppDbContext _context;

    public GetMostRequestedDestinationsUseCase(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna ranking de destinos más demandados.
    /// </summary>
    public async Task<IReadOnlyList<DestinationDemandRow>> ExecuteAsync(int take = 20)
    {
        var query =
            from rf in _context.ReservationFlights.AsNoTracking()
            join f in _context.Flights.AsNoTracking() on rf.FlightId equals f.Id
            join r in _context.Routes.AsNoTracking() on f.RouteId equals r.Id
            join a in _context.Airports.AsNoTracking() on r.DestinationAirportId equals a.Id
            group new { rf, a } by new { a.Id, a.Name, a.IataCode } into g
            orderby g.Count() descending, g.Key.Id
            select new DestinationDemandRow(
                g.Key.Id,
                g.Key.Name ?? "N/A",
                g.Key.IataCode ?? "N/A",
                g.Count()
            );

        return await query.Take(take).ToListAsync();
    }
}
