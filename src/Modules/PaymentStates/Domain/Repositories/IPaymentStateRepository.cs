// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentStates\Domain\Repositories\IPaymentStateRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentStates.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentStates.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentStates.Domain.Repositories;

public interface IPaymentStateRepository
{
    Task<IEnumerable<PaymentState>> GetAllAsync();
    Task<PaymentState?> GetByIdAsync(PaymentStateId id);
    Task<PaymentState?> GetByNameAsync(PaymentStateName name);
    Task AddAsync(PaymentState paymentState);
    Task UpdateAsync(PaymentState paymentState);
    Task DeleteAsync(PaymentState paymentState);
    Task<bool> ExistsAsync(PaymentStateId id);
}
