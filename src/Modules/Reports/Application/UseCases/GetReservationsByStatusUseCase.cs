// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reports\Application\UseCases\GetReservationsByStatusUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reports.Application.Models;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Reports.Application.UseCases;

/// <summary>
/// Agrupa reservas por estado para medir distribución operativa.
/// </summary>
public sealed class GetReservationsByStatusUseCase
{
    private readonly AppDbContext _context;

    public GetReservationsByStatusUseCase(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna cantidad de reservas por estado, ordenadas de mayor a menor.
    /// </summary>
    public async Task<IReadOnlyList<ReservationStatusCountRow>> ExecuteAsync()
    {
        var grouped = _context.Reservations
            .AsNoTracking()
            .GroupBy(r => r.StatusId)
            .Select(g => new { StatusId = g.Key, Total = g.Count() });

        var query =
            from g in grouped
            join s in _context.ReservationStatuses.AsNoTracking() on g.StatusId equals s.Id
            orderby g.Total descending, s.Id
            select new ReservationStatusCountRow(
                s.Id,
                s.Name ?? $"Status {s.Id}",
                g.Total
            );

        return await query.ToListAsync();
    }
}
