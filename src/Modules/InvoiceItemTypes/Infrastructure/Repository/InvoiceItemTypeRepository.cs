// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\Infrastructure\Repository\InvoiceItemTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.ValueObject;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.InvoiceItemTypes.Infrastructure.Repository;

public class InvoiceItemTypeRepository : IInvoiceItemTypeRepository
{
    private readonly AppDbContext _context;

    public InvoiceItemTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InvoiceItemType>> GetAllAsync()
    {
        var entities = await _context.InvoiceItemTypes
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<InvoiceItemType?> GetByIdAsync(InvoiceItemTypeId id)
    {
        var entity = await _context.InvoiceItemTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<InvoiceItemType?> GetByNameAsync(InvoiceItemTypeName name)
    {
        var entity = await _context.InvoiceItemTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(InvoiceItemType invoiceItemType)
    {
        await _context.InvoiceItemTypes.AddAsync(MapToEntity(invoiceItemType));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(InvoiceItemType invoiceItemType)
    {
        var existing = await _context.InvoiceItemTypes
            .FirstOrDefaultAsync(e => e.Id == invoiceItemType.Id.Value);

        if (existing is null)
            return;

        existing.Name = invoiceItemType.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(InvoiceItemType invoiceItemType)
    {
        var entity = await _context.InvoiceItemTypes.FindAsync(invoiceItemType.Id.Value);

        if (entity is null)
            return;

        _context.InvoiceItemTypes.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(InvoiceItemTypeId id)
    {
        return _context.InvoiceItemTypes.AnyAsync(e => e.Id == id.Value);
    }

    private static InvoiceItemType MapToDomain(InvoiceItemTypeEntity entity)
    {
        return InvoiceItemType.Create(
            InvoiceItemTypeId.Create(entity.Id),
            InvoiceItemTypeName.FromPersistence(entity.Name ?? string.Empty)
        );
    }

    private static InvoiceItemTypeEntity MapToEntity(InvoiceItemType invoiceItemType)
    {
        return new InvoiceItemTypeEntity
        {
            Id = invoiceItemType.Id.Value,
            Name = invoiceItemType.Name.Value
        };
    }
}
