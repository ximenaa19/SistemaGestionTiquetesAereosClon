// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\Domain\Repositories\IPaymentMethodRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethods.Domain.Aggregate;
using GestionAerolineas.src.Modules.PaymentMethods.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentMethods.Domain.Repositories;

public interface IPaymentMethodRepository
{
    Task<IEnumerable<PaymentMethod>> GetAllAsync();
    Task<PaymentMethod?> GetByIdAsync(PaymentMethodId id);
    Task<PaymentMethod?> GetByCommercialNameAsync(PaymentMethodCommercialName commercialName);
    Task AddAsync(PaymentMethod paymentMethod);
    Task UpdateAsync(PaymentMethod paymentMethod);
    Task DeleteAsync(PaymentMethod paymentMethod);
    Task<bool> ExistsAsync(PaymentMethodId id);
}

