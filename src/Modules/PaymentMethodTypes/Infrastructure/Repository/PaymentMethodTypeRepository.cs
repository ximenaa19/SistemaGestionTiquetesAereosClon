// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethodTypes\Infrastructure\Repository\PaymentMethodTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.ValueObject;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.PaymentMethodTypes.Infrastructure.Repository;

public class PaymentMethodTypeRepository : IPaymentMethodTypeRepository
{
    private readonly AppDbContext _context;

    public PaymentMethodTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PaymentMethodType>> GetAllAsync()
    {
        var entities = await _context.PaymentMethodTypes.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<PaymentMethodType?> GetByIdAsync(PaymentMethodTypeId id)
    {
        var entity = await _context.PaymentMethodTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PaymentMethodType?> GetByNameAsync(PaymentMethodTypeName name)
    {
        var entity = await _context.PaymentMethodTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(PaymentMethodType paymentMethodType)
    {
        await _context.PaymentMethodTypes.AddAsync(MapToEntity(paymentMethodType));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PaymentMethodType paymentMethodType)
    {
        var existing = await _context.PaymentMethodTypes
            .FirstOrDefaultAsync(e => e.Id == paymentMethodType.Id.Value);

        if (existing is null)
            return;

        existing.Name = paymentMethodType.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PaymentMethodType paymentMethodType)
    {
        var entity = await _context.PaymentMethodTypes.FindAsync(paymentMethodType.Id.Value);

        if (entity is null)
            return;

        _context.PaymentMethodTypes.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(PaymentMethodTypeId id)
    {
        return await _context.PaymentMethodTypes.AnyAsync(e => e.Id == id.Value);
    }

    private static PaymentMethodType MapToDomain(PaymentMethodTypeEntity entity)
    {
        return PaymentMethodType.Create(
            PaymentMethodTypeId.Create(entity.Id),
            PaymentMethodTypeName.Create(entity.Name ?? string.Empty)
        );
    }

    private static PaymentMethodTypeEntity MapToEntity(PaymentMethodType paymentMethodType)
    {
        return new PaymentMethodTypeEntity
        {
            Id = paymentMethodType.Id.Value,
            Name = paymentMethodType.Name.Value
        };
    }
}
