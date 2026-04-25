// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\Domain\Repositories\IInvoiceItemTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.InvoiceItemTypes.Domain.Repositories;

public interface IInvoiceItemTypeRepository
{
    Task<IEnumerable<InvoiceItemType>> GetAllAsync();
    Task<InvoiceItemType?> GetByIdAsync(InvoiceItemTypeId id);
    Task<InvoiceItemType?> GetByNameAsync(InvoiceItemTypeName name);
    Task AddAsync(InvoiceItemType invoiceItemType);
    Task UpdateAsync(InvoiceItemType invoiceItemType);
    Task DeleteAsync(InvoiceItemType invoiceItemType);
    Task<bool> ExistsAsync(InvoiceItemTypeId id);
}
