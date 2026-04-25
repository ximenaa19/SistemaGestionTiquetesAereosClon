// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reports\Application\UseCases\GetTopCustomersByReservationsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reports.Application.Models;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Reports.Application.UseCases;

/// <summary>
/// Identifica clientes con mayor número de reservas registradas.
/// </summary>
public sealed class GetTopCustomersByReservationsUseCase
{
    private readonly AppDbContext _context;

    public GetTopCustomersByReservationsUseCase(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna ranking de clientes por cantidad de reservas.
    /// </summary>
    public async Task<IReadOnlyList<CustomerReservationRow>> ExecuteAsync(int take = 20)
    {
        var grouped = _context.Reservations
            .AsNoTracking()
            .GroupBy(r => r.CustomerId)
            .Select(g => new
            {
                CustomerId = g.Key,
                TotalReservations = g.Count()
            });

        var query =
            from g in grouped
            join c in _context.Customers.AsNoTracking() on g.CustomerId equals c.Id
            join p in _context.People.AsNoTracking() on c.PersonId equals p.Id
            select new
            {
                c.Id,
                p.FirstNames,
                p.LastNames,
                g.TotalReservations
            };

        var rows = await query
            .OrderByDescending(x => x.TotalReservations)
            .ThenBy(x => x.Id)
            .Take(take)
            .ToListAsync();

        return rows
            .Select(x => new CustomerReservationRow(
                x.Id,
                $"{x.FirstNames ?? string.Empty} {x.LastNames ?? string.Empty}".Trim(),
                x.TotalReservations))
            .ToList();
    }
}
