// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reports\Application\UseCases\GetFlightsWithAvailableSeatsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reports.Application.Models;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Reports.Application.UseCases;

/// <summary>
/// Consulta vuelos que aún tienen capacidad disponible para venta/asignación.
/// </summary>
public sealed class GetFlightsWithAvailableSeatsUseCase
{
    private readonly AppDbContext _context;

    public GetFlightsWithAvailableSeatsUseCase(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Devuelve vuelos con asientos disponibles, ordenados por disponibilidad descendente.
    /// </summary>
    public async Task<IReadOnlyList<FlightAvailabilityRow>> ExecuteAsync(int take = 50)
    {
        var rows = await _context.Flights
            .AsNoTracking()
            .Where(f => f.AvailableSeats > 0)
            .Select(f => new
            {
                f.Id,
                f.Code,
                f.TotalCapacity,
                f.AvailableSeats
            })
            .OrderByDescending(x => x.AvailableSeats)
            .ThenBy(x => x.Id)
            .Take(take)
            .ToListAsync();

        return rows
            .Select(x => new FlightAvailabilityRow(
                x.Id,
                x.Code ?? $"FL-{x.Id}",
                x.TotalCapacity,
                x.AvailableSeats))
            .ToList();
    }
}
