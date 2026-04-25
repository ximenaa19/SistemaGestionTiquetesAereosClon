// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reports\Application\UseCases\GetEstimatedIncomeByAirlineUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reports.Application.Models;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Reports.Application.UseCases;

/// <summary>
/// Estima ingresos por aerolínea a partir de los montos asociados a reservas/pagos.
/// </summary>
public sealed class GetEstimatedIncomeByAirlineUseCase
{
    private readonly AppDbContext _context;

    public GetEstimatedIncomeByAirlineUseCase(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna ingresos estimados consolidados por aerolínea.
    /// </summary>
    public async Task<IReadOnlyList<AirlineIncomeRow>> ExecuteAsync()
    {
        var query =
            from rf in _context.ReservationFlights.AsNoTracking()
            join f in _context.Flights.AsNoTracking() on rf.FlightId equals f.Id
            join al in _context.Airlines.AsNoTracking() on f.AirlineId equals al.Id
            group new { rf, al } by new { al.Id, al.Name, al.IataCode } into g
            orderby g.Sum(x => x.rf.PartialAmount) descending, g.Key.Id
            select new AirlineIncomeRow(
                g.Key.Id,
                g.Key.Name ?? "N/A",
                g.Key.IataCode ?? "N/A",
                g.Sum(x => x.rf.PartialAmount)
            );

        return await query.ToListAsync();
    }
}
