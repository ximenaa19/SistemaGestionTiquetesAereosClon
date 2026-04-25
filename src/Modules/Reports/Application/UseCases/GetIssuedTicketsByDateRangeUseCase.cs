// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reports\Application\UseCases\GetIssuedTicketsByDateRangeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reports.Application.Models;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Reports.Application.UseCases;

/// <summary>
/// Consolida tiquetes emitidos por fecha en un rango consultado por el usuario.
/// </summary>
public sealed class GetIssuedTicketsByDateRangeUseCase
{
    private readonly AppDbContext _context;

    public GetIssuedTicketsByDateRangeUseCase(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna conteo diario de tiquetes emitidos entre <paramref name="start"/> y <paramref name="end"/>.
    /// </summary>
    public async Task<IReadOnlyList<IssuedTicketsByDateRow>> ExecuteAsync(DateTime start, DateTime end)
    {
        var rows = await _context.Tickets
            .AsNoTracking()
            .Where(t => t.IssuedAt >= start && t.IssuedAt <= end)
            .GroupBy(t => t.IssuedAt.Date)
            .Select(g => new
            {
                Date = g.Key,
                TotalTickets = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return rows
            .Select(x => new IssuedTicketsByDateRow(
                DateOnly.FromDateTime(x.Date),
                x.TotalTickets))
            .ToList();
    }
}
