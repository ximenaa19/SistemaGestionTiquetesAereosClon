// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\Infrastructure\Repository\PaymentMethodRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethods.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.ValueObject;
using GestionAerolineas.src.Modules.PaymentMethods.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.PaymentMethods.Infrastructure.Repository;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly AppDbContext _context;

    public PaymentMethodRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        var entities = await _context.PaymentMethods
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<PaymentMethod?> GetByIdAsync(PaymentMethodId id)
    {
        var entity = await _context.PaymentMethods
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PaymentMethod?> GetByCommercialNameAsync(PaymentMethodCommercialName commercialName)
    {
        var entity = await _context.PaymentMethods
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.CommercialName == commercialName.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(PaymentMethod paymentMethod)
    {
        await _context.PaymentMethods.AddAsync(MapToEntity(paymentMethod));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PaymentMethod paymentMethod)
    {
        var existing = await _context.PaymentMethods
            .FirstOrDefaultAsync(e => e.Id == paymentMethod.Id.Value);

        if (existing is null)
            return;

        existing.PaymentMethodTypeId = paymentMethod.PaymentMethodTypeId.Value;
        existing.CardTypeId = paymentMethod.CardTypeId?.Value;
        existing.CardIssuerId = paymentMethod.CardIssuerId?.Value;
        existing.CommercialName = paymentMethod.CommercialName.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PaymentMethod paymentMethod)
    {
        var entity = await _context.PaymentMethods.FindAsync(paymentMethod.Id.Value);

        if (entity is null)
            return;

        _context.PaymentMethods.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(PaymentMethodId id)
    {
        return _context.PaymentMethods.AnyAsync(e => e.Id == id.Value);
    }

    private static PaymentMethod MapToDomain(PaymentMethodEntity entity)
    {
        return PaymentMethod.Create(
            PaymentMethodId.Create(entity.Id),
            PaymentMethodTypeId.Create(entity.PaymentMethodTypeId),
            entity.CardTypeId is null ? null : CardTypeId.Create(entity.CardTypeId.Value),
            entity.CardIssuerId is null ? null : CardIssuerId.Create(entity.CardIssuerId.Value),
            PaymentMethodCommercialName.Create(entity.CommercialName ?? string.Empty)
        );
    }

    private static PaymentMethodEntity MapToEntity(PaymentMethod paymentMethod)
    {
        return new PaymentMethodEntity
        {
            Id = paymentMethod.Id.Value,
            PaymentMethodTypeId = paymentMethod.PaymentMethodTypeId.Value,
            CardTypeId = paymentMethod.CardTypeId?.Value,
            CardIssuerId = paymentMethod.CardIssuerId?.Value,
            CommercialName = paymentMethod.CommercialName.Value
        };
    }
}

