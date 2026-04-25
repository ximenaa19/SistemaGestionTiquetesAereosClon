// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Domain\Repositories\IInvoiceRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Invoices.Domain.Aggregate;
using GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Invoices.Domain.Repositories;

public interface IInvoiceRepository
{
    Task<IEnumerable<Invoice>> GetAllAsync();
    Task<Invoice?> GetByIdAsync(InvoiceId id);
    Task<Invoice?> GetByNumberAsync(InvoiceNumber number);
    Task<Invoice?> GetByReservationIdAsync(InvoiceReservationId reservationId);
    Task<IEnumerable<Invoice>> GetByIssuedAtRangeAsync(DateTime fromInclusive, DateTime toInclusive);
    Task AddAsync(Invoice invoice);
    Task UpdateAsync(Invoice invoice);
    Task DeleteAsync(Invoice invoice);
    Task<bool> ExistsAsync(InvoiceId id);
    Task<bool> ExistsByReservationIdAsync(int reservationId, int? excludingInvoiceId = null);
    Task<bool> ExistsByNormalizedNumberAsync(string normalizedNumber, int? excludingInvoiceId = null);
}

