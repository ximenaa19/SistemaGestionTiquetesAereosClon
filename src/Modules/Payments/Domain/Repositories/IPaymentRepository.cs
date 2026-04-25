// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Domain\Repositories\IPaymentRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Payments.Domain.Aggregate;
using GestionAerolineas.src.Modules.Payments.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Payments.Domain.Repositories;

public interface IPaymentRepository
{
    Task<IEnumerable<Payment>> GetAllAsync();
    Task<Payment?> GetByIdAsync(PaymentId id);
    Task<IEnumerable<Payment>> GetByReservationIdAsync(PaymentReservationId reservationId);
    Task<IEnumerable<Payment>> GetByStateIdAsync(PaymentStateId stateId);
    Task<IEnumerable<Payment>> GetByMethodIdAsync(PaymentMethodId methodId);
    Task<IEnumerable<Payment>> GetByPaidAtRangeAsync(DateTime fromInclusive, DateTime toInclusive);
    Task AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
    Task DeleteAsync(Payment payment);
    Task<bool> ExistsAsync(PaymentId id);
    Task<decimal> SumPaidAmountByReservationIdAsync(int reservationId, int paidStateId, int? excludingPaymentId = null);
}

