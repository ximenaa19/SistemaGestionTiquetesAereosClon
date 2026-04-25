// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethodTypes\Domain\Repositories\IPaymentMethodTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentMethodTypes.Domain.Repositories;

public interface IPaymentMethodTypeRepository
{
    Task<IEnumerable<PaymentMethodType>> GetAllAsync();
    Task<PaymentMethodType?> GetByIdAsync(PaymentMethodTypeId id);
    Task<PaymentMethodType?> GetByNameAsync(PaymentMethodTypeName name);
    Task AddAsync(PaymentMethodType paymentMethodType);
    Task UpdateAsync(PaymentMethodType paymentMethodType);
    Task DeleteAsync(PaymentMethodType paymentMethodType);
    Task<bool> ExistsAsync(PaymentMethodTypeId id);
}
