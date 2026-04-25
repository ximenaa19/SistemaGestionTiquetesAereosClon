// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\DocumentTypes\Domain\Repositories\IDocumentTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.DocumentTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.DocumentTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.DocumentTypes.Domain.Repositories;

public interface IDocumentTypeRepository
{
    Task<IEnumerable<DocumentType>> GetAllAsync();
    Task<DocumentType?> GetByIdAsync(DocumentTypeId id);
    Task<DocumentType?> GetByNameAsync(DocumentTypeName name);
    Task<DocumentType?> GetByCodeAsync(DocumentTypeCode code);

    Task AddAsync(DocumentType documentType);
    Task UpdateAsync(DocumentType documentType);
    Task DeleteAsync(DocumentType documentType);

    Task<bool> ExistsAsync(DocumentTypeId id);
}
