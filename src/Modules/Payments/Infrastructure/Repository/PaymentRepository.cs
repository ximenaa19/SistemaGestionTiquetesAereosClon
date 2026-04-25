// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Infrastructure\Repository\PaymentRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Payments.Domain.Aggregate;
using GestionAerolineas.src.Modules.Payments.Domain.Repositories;
using GestionAerolineas.src.Modules.Payments.Domain.ValueObject;
using GestionAerolineas.src.Modules.Payments.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Payments.Infrastructure.Repository;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        var entities = await _context.Set<PaymentEntity>()
            .AsNoTracking()
            .OrderByDescending(e => e.PaidAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Payment?> GetByIdAsync(PaymentId id)
    {
        var entity = await _context.Set<PaymentEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Payment>> GetByReservationIdAsync(PaymentReservationId reservationId)
    {
        var entities = await _context.Set<PaymentEntity>()
            .AsNoTracking()
            .Where(e => e.ReservationId == reservationId.Value)
            .OrderByDescending(e => e.PaidAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Payment>> GetByStateIdAsync(PaymentStateId stateId)
    {
        var entities = await _context.Set<PaymentEntity>()
            .AsNoTracking()
            .Where(e => e.StateId == stateId.Value)
            .OrderByDescending(e => e.PaidAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Payment>> GetByMethodIdAsync(PaymentMethodId methodId)
    {
        var entities = await _context.Set<PaymentEntity>()
            .AsNoTracking()
            .Where(e => e.MethodId == methodId.Value)
            .OrderByDescending(e => e.PaidAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<Payment>> GetByPaidAtRangeAsync(DateTime fromInclusive, DateTime toInclusive)
    {
        var entities = await _context.Set<PaymentEntity>()
            .AsNoTracking()
            .Where(e => e.PaidAt >= fromInclusive && e.PaidAt <= toInclusive)
            .OrderByDescending(e => e.PaidAt)
            .ThenByDescending(e => e.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(Payment payment)
    {
        await _context.Set<PaymentEntity>().AddAsync(MapToEntity(payment));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Payment payment)
    {
        var existing = await _context.Set<PaymentEntity>()
            .FirstOrDefaultAsync(e => e.Id == payment.Id.Value);

        if (existing is null)
            return;

        existing.ReservationId = payment.ReservationId.Value;
        existing.Amount = payment.Amount.Value;
        existing.PaidAt = payment.PaidAt.Value;
        existing.StateId = payment.StateId.Value;
        existing.MethodId = payment.MethodId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Payment payment)
    {
        var entity = await _context.Set<PaymentEntity>().FindAsync(payment.Id.Value);
        if (entity is null)
            return;

        _context.Set<PaymentEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(PaymentId id)
    {
        return _context.Set<PaymentEntity>().AnyAsync(e => e.Id == id.Value);
    }

    public Task<decimal> SumPaidAmountByReservationIdAsync(int reservationId, int paidStateId, int? excludingPaymentId = null)
    {
        var query = _context.Set<PaymentEntity>()
            .AsNoTracking()
            .Where(p => p.ReservationId == reservationId && p.StateId == paidStateId);

        if (excludingPaymentId.HasValue)
            query = query.Where(p => p.Id != excludingPaymentId.Value);

        return query.SumAsync(p => p.Amount);
    }

    private static Payment MapToDomain(PaymentEntity entity)
    {
        try
        {
            return Payment.Create(
                PaymentId.Create(entity.Id),
                PaymentReservationId.Create(entity.ReservationId),
                PaymentAmount.Create(entity.Amount),
                PaymentPaidAt.Create(entity.PaidAt),
                PaymentStateId.Create(entity.StateId),
                PaymentMethodId.Create(entity.MethodId),
                PaymentCreatedAt.CreateOptional(entity.CreatedAt),
                PaymentUpdatedAt.CreateOptional(entity.UpdatedAt));
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro payments(id={entity.Id}) tiene datos invalidos. " +
                $"reserva_id={entity.ReservationId}, monto={entity.Amount}, estado_pago_id={entity.StateId}, metodo_pago_id={entity.MethodId}.",
                ex);
        }
    }

    private static PaymentEntity MapToEntity(Payment payment)
    {
        return new PaymentEntity
        {
            Id = payment.Id.Value,
            ReservationId = payment.ReservationId.Value,
            Amount = payment.Amount.Value,
            PaidAt = payment.PaidAt.Value,
            StateId = payment.StateId.Value,
            MethodId = payment.MethodId.Value,
            CreatedAt = payment.CreatedAt.Value,
            UpdatedAt = payment.UpdatedAt.Value
        };
    }
}

