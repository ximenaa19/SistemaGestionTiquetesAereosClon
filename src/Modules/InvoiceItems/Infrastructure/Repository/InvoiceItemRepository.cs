// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Infrastructure\Repository\InvoiceItemRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Repositories;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;
using GestionAerolineas.src.Modules.InvoiceItems.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Invoices.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.InvoiceItems.Infrastructure.Repository;

public class InvoiceItemRepository : IInvoiceItemRepository
{
    private readonly AppDbContext _context;

    public InvoiceItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InvoiceItem>> GetAllAsync()
    {
        var entities = await _context.Set<InvoiceItemEntity>()
            .AsNoTracking()
            .OrderByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<InvoiceItem?> GetByIdAsync(InvoiceItemId id)
    {
        var entity = await _context.Set<InvoiceItemEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<InvoiceItem>> GetByInvoiceIdAsync(int invoiceId)
    {
        var entities = await _context.Set<InvoiceItemEntity>()
            .AsNoTracking()
            .Where(e => e.InvoiceId == invoiceId)
            .OrderByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<InvoiceItem>> GetByItemTypeIdAsync(InvoiceItemTypeId itemTypeId)
    {
        var entities = await _context.Set<InvoiceItemEntity>()
            .AsNoTracking()
            .Where(e => e.ItemTypeId == itemTypeId.Value)
            .OrderByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<InvoiceItem>> GetByReservationPassengerIdAsync(int reservationPassengerId)
    {
        var entities = await _context.Set<InvoiceItemEntity>()
            .AsNoTracking()
            .Where(e => e.ReservationPassengerId == reservationPassengerId)
            .OrderByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(InvoiceItem item)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();

        await _context.Set<InvoiceItemEntity>().AddAsync(MapToEntity(item));
        await _context.SaveChangesAsync();

        await RecalculateInvoiceTotalsAsync(item.InvoiceId.Value);
        await tx.CommitAsync();
    }

    public async Task UpdateAsync(InvoiceItem item)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();

        var existing = await _context.Set<InvoiceItemEntity>()
            .FirstOrDefaultAsync(e => e.Id == item.Id.Value);

        if (existing is null)
            return;

        var previousInvoiceId = existing.InvoiceId;

        existing.InvoiceId = item.InvoiceId.Value;
        existing.ItemTypeId = item.ItemTypeId.Value;
        existing.Description = item.Description.Value;
        existing.Quantity = item.Quantity.Value;
        existing.UnitPrice = item.UnitPrice.Value;
        existing.Subtotal = item.Subtotal.Value;
        existing.ReservationPassengerId = item.ReservationPassengerId.Value;

        await _context.SaveChangesAsync();

        await RecalculateInvoiceTotalsAsync(previousInvoiceId);
        if (previousInvoiceId != item.InvoiceId.Value)
            await RecalculateInvoiceTotalsAsync(item.InvoiceId.Value);

        await tx.CommitAsync();
    }

    public async Task DeleteAsync(InvoiceItem item)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();

        var entity = await _context.Set<InvoiceItemEntity>().FindAsync(item.Id.Value);
        if (entity is null)
            return;

        var invoiceId = entity.InvoiceId;

        _context.Set<InvoiceItemEntity>().Remove(entity);
        await _context.SaveChangesAsync();

        await RecalculateInvoiceTotalsAsync(invoiceId);
        await tx.CommitAsync();
    }

    public Task<bool> ExistsAsync(InvoiceItemId id)
    {
        return _context.Set<InvoiceItemEntity>().AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> AnyByInvoiceIdAsync(int invoiceId)
    {
        return _context.Set<InvoiceItemEntity>()
            .AsNoTracking()
            .AnyAsync(e => e.InvoiceId == invoiceId);
    }

    private async Task RecalculateInvoiceTotalsAsync(int invoiceId)
    {
        var invoice = await _context.Set<InvoiceEntity>().FirstOrDefaultAsync(i => i.Id == invoiceId);
        if (invoice is null)
            return;

        var subtotal = await _context.Set<InvoiceItemEntity>()
            .AsNoTracking()
            .Where(i => i.InvoiceId == invoiceId)
            .SumAsync(i => (decimal?)i.Subtotal) ?? 0m;

        invoice.Subtotal = subtotal;
        invoice.Taxes = 0m;
        invoice.Total = subtotal;

        await _context.SaveChangesAsync();
    }

    private static InvoiceItem MapToDomain(InvoiceItemEntity entity)
    {
        try
        {
            return InvoiceItem.Create(
                InvoiceItemId.Create(entity.Id),
                InvoiceItemInvoiceId.Create(entity.InvoiceId),
                InvoiceItemTypeId.Create(entity.ItemTypeId),
                InvoiceItemDescription.Create(entity.Description ?? string.Empty),
                InvoiceItemQuantity.Create(entity.Quantity),
                InvoiceItemUnitPrice.Create(entity.UnitPrice),
                InvoiceItemSubtotal.Create(entity.Subtotal),
                InvoiceItemReservationPassengerId.Create(entity.ReservationPassengerId));
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro invoiceitems(id={entity.Id}) tiene datos invalidos. " +
                $"factura_id={entity.InvoiceId}, tipo_item_id={entity.ItemTypeId}, cantidad={entity.Quantity}, precio_unitario={entity.UnitPrice}, subtotal={entity.Subtotal}, reserva_pasajero_id={entity.ReservationPassengerId}.",
                ex);
        }
    }

    private static InvoiceItemEntity MapToEntity(InvoiceItem item)
    {
        return new InvoiceItemEntity
        {
            Id = item.Id.Value,
            InvoiceId = item.InvoiceId.Value,
            ItemTypeId = item.ItemTypeId.Value,
            Description = item.Description.Value,
            Quantity = item.Quantity.Value,
            UnitPrice = item.UnitPrice.Value,
            Subtotal = item.Subtotal.Value,
            ReservationPassengerId = item.ReservationPassengerId.Value
        };
    }
}

