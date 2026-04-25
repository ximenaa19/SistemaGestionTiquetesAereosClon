// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reports\Application\UseCases\GetFlightsWithHighestOccupancyUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reports.Application.Models;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Reports.Application.UseCases;

/// <summary>
/// Consulta vuelos ordenados por ocupación para identificar los de mayor demanda.
/// </summary>
public sealed class GetFlightsWithHighestOccupancyUseCase
{
    private readonly AppDbContext _context;

    public GetFlightsWithHighestOccupancyUseCase(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Ejecuta el reporte de ocupación y retorna los primeros <paramref name="take"/> resultados.
    /// </summary>
    public async Task<IReadOnlyList<FlightOccupancyRow>> ExecuteAsync(int take = 20)
    {
        var rows = await _context.Flights
            .AsNoTracking()
            .Select(f => new
            {
                f.Id,
                f.Code,
                f.TotalCapacity,
                f.AvailableSeats,
                OccupancyPercent = f.TotalCapacity == 0
                    ? 0m
                    : ((decimal)(f.TotalCapacity - f.AvailableSeats) * 100m) / f.TotalCapacity
            })
            .OrderByDescending(x => x.OccupancyPercent)
            .ThenByDescending(x => x.TotalCapacity - x.AvailableSeats)
            .ThenBy(x => x.Id)
            .Take(take)
            .ToListAsync();

        return rows
            .Select(x => new FlightOccupancyRow(
                x.Id,
                x.Code ?? $"FL-{x.Id}",
                x.TotalCapacity,
                x.TotalCapacity - x.AvailableSeats,
                x.AvailableSeats,
                Math.Round(x.OccupancyPercent, 2)))
            .ToList();
    }
}
