// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Domain\Repositories\IInvoiceItemRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItems.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItems.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItems.Domain.Repositories;

public interface IInvoiceItemRepository
{
    Task<IEnumerable<InvoiceItem>> GetAllAsync();
    Task<InvoiceItem?> GetByIdAsync(InvoiceItemId id);
    Task<IEnumerable<InvoiceItem>> GetByInvoiceIdAsync(int invoiceId);
    Task<IEnumerable<InvoiceItem>> GetByItemTypeIdAsync(InvoiceItemTypeId itemTypeId);
    Task<IEnumerable<InvoiceItem>> GetByReservationPassengerIdAsync(int reservationPassengerId);
    Task AddAsync(InvoiceItem item);
    Task UpdateAsync(InvoiceItem item);
    Task DeleteAsync(InvoiceItem item);
    Task<bool> ExistsAsync(InvoiceItemId id);
    Task<bool> AnyByInvoiceIdAsync(int invoiceId);
}

