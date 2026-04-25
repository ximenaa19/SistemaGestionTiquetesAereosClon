// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentStates\Infrastructure\Repository\PaymentStateRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentStates.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentStates.Domain.Repositories;
using GestionAerolineas.src.Modules.PaymentStates.Domain.ValueObject;
using GestionAerolineas.src.Modules.PaymentStates.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.PaymentStates.Infrastructure.Repository;

public class PaymentStateRepository : IPaymentStateRepository
{
    private readonly AppDbContext _context;

    public PaymentStateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PaymentState>> GetAllAsync()
    {
        var entities = await _context.PaymentStates
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<PaymentState?> GetByIdAsync(PaymentStateId id)
    {
        var entity = await _context.PaymentStates
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PaymentState?> GetByNameAsync(PaymentStateName name)
    {
        var entity = await _context.PaymentStates
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(PaymentState paymentState)
    {
        await _context.PaymentStates.AddAsync(MapToEntity(paymentState));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PaymentState paymentState)
    {
        var existing = await _context.PaymentStates
            .FirstOrDefaultAsync(e => e.Id == paymentState.Id.Value);

        if (existing is null)
            return;

        existing.Name = paymentState.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PaymentState paymentState)
    {
        var entity = await _context.PaymentStates.FindAsync(paymentState.Id.Value);

        if (entity is null)
            return;

        _context.PaymentStates.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(PaymentStateId id)
    {
        return _context.PaymentStates.AnyAsync(e => e.Id == id.Value);
    }

    private static PaymentState MapToDomain(PaymentStateEntity entity)
    {
        return PaymentState.Create(
            PaymentStateId.Create(entity.Id),
            PaymentStateName.Create(entity.Name ?? string.Empty)
        );
    }

    private static PaymentStateEntity MapToEntity(PaymentState paymentState)
    {
        return new PaymentStateEntity
        {
            Id = paymentState.Id.Value,
            Name = paymentState.Name.Value
        };
    }
}
