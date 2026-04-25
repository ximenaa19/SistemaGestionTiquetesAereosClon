// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Infrastructure\Repository\InvoiceRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Invoices.Domain.Aggregate;
using GestionAerolineas.src.Modules.Invoices.Domain.Repositories;
using GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;
using GestionAerolineas.src.Modules.Invoices.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Invoices.Infrastructure.Repository;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _context;

    public InvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Invoice>> GetAllAsync()
    {
        var entities = await _context.Set<InvoiceEntity>()
            .AsNoTracking()
            .OrderByDescending(e => e.IssuedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Invoice?> GetByIdAsync(InvoiceId id)
    {
        var entity = await _context.Set<InvoiceEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Invoice?> GetByNumberAsync(InvoiceNumber number)
    {
        var normalized = InvoiceNumber.Normalize(number.Value);

        var entity = await _context.Set<InvoiceEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.InvoiceNumber != null && e.InvoiceNumber.Trim().ToUpper() == normalized);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Invoice?> GetByReservationIdAsync(InvoiceReservationId reservationId)
    {
        var entity = await _context.Set<InvoiceEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ReservationId == reservationId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Invoice>> GetByIssuedAtRangeAsync(DateTime fromInclusive, DateTime toInclusive)
    {
        var entities = await _context.Set<InvoiceEntity>()
            .AsNoTracking()
            .Where(e => e.IssuedAt >= fromInclusive && e.IssuedAt <= toInclusive)
            .OrderByDescending(e => e.IssuedAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(Invoice invoice)
    {
        await _context.Set<InvoiceEntity>().AddAsync(MapToEntity(invoice));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Invoice invoice)
    {
        var existing = await _context.Set<InvoiceEntity>()
            .FirstOrDefaultAsync(e => e.Id == invoice.Id.Value);

        if (existing is null)
            return;

        existing.ReservationId = invoice.ReservationId.Value;
        existing.InvoiceNumber = invoice.Number.Value;
        existing.IssuedAt = invoice.IssuedAt.Value;
        existing.Subtotal = invoice.Subtotal.Value;
        existing.Taxes = invoice.Taxes.Value;
        existing.Total = invoice.Total.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Invoice invoice)
    {
        var entity = await _context.Set<InvoiceEntity>().FindAsync(invoice.Id.Value);
        if (entity is null)
            return;

        _context.Set<InvoiceEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(InvoiceId id)
    {
        return _context.Set<InvoiceEntity>().AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByReservationIdAsync(int reservationId, int? excludingInvoiceId = null)
    {
        var query = _context.Set<InvoiceEntity>()
            .AsNoTracking()
            .Where(e => e.ReservationId == reservationId);

        if (excludingInvoiceId.HasValue)
            query = query.Where(e => e.Id != excludingInvoiceId.Value);

        return query.AnyAsync();
    }

    public Task<bool> ExistsByNormalizedNumberAsync(string normalizedNumber, int? excludingInvoiceId = null)
    {
        var query = _context.Set<InvoiceEntity>()
            .AsNoTracking()
            .Where(e => e.InvoiceNumber != null);

        if (excludingInvoiceId.HasValue)
            query = query.Where(e => e.Id != excludingInvoiceId.Value);

        return query.AnyAsync(e => e.InvoiceNumber!.Trim().ToUpper() == normalizedNumber);
    }

    private static Invoice MapToDomain(InvoiceEntity entity)
    {
        try
        {
            return Invoice.Create(
                InvoiceId.Create(entity.Id),
                InvoiceReservationId.Create(entity.ReservationId),
                InvoiceNumber.Create(entity.InvoiceNumber ?? string.Empty),
                InvoiceIssuedAt.Create(entity.IssuedAt),
                InvoiceSubtotal.Create(entity.Subtotal),
                InvoiceTaxes.Create(entity.Taxes),
                InvoiceTotal.Create(entity.Total),
                InvoiceCreatedAt.CreateOptional(entity.CreatedAt));
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro invoices(id={entity.Id}) tiene datos invalidos. " +
                $"reserva_id={entity.ReservationId}, numero_factura='{entity.InvoiceNumber}', subtotal={entity.Subtotal}, impuestos={entity.Taxes}, total={entity.Total}.",
                ex);
        }
    }

    private static InvoiceEntity MapToEntity(Invoice invoice)
    {
        return new InvoiceEntity
        {
            Id = invoice.Id.Value,
            ReservationId = invoice.ReservationId.Value,
            InvoiceNumber = invoice.Number.Value,
            IssuedAt = invoice.IssuedAt.Value,
            Subtotal = invoice.Subtotal.Value,
            Taxes = invoice.Taxes.Value,
            Total = invoice.Total.Value,
            CreatedAt = invoice.CreatedAt.Value
        };
    }
}
