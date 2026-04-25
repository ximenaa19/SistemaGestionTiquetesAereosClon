// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\DocumentTypes\Infrastructure\Repository\DocumentTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.DocumentTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.DocumentTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.DocumentTypes.Domain.ValueObject;
using GestionAerolineas.src.Modules.DocumentTypes.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.DocumentTypes.Infrastructure.Repository;

public class DocumentTypeRepository : IDocumentTypeRepository
{
    private readonly AppDbContext _context;

    public DocumentTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DocumentType>> GetAllAsync()
    {
        var entities = await _context.DocumentTypes
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<DocumentType?> GetByIdAsync(DocumentTypeId id)
    {
        var entity = await _context.DocumentTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<DocumentType?> GetByNameAsync(DocumentTypeName name)
    {
        var entity = await _context.DocumentTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<DocumentType?> GetByCodeAsync(DocumentTypeCode code)
    {
        var entity = await _context.DocumentTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Code == code.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(DocumentType documentType)
    {
        await _context.DocumentTypes.AddAsync(MapToEntity(documentType));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DocumentType documentType)
    {
        var existing = await _context.DocumentTypes
            .FirstOrDefaultAsync(e => e.Id == documentType.Id.Value);

        if (existing is null)
        {
            return;
        }

        existing.Name = documentType.Name.Value;
        existing.Code = documentType.Code.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(DocumentType documentType)
    {
        var entity = await _context.DocumentTypes.FindAsync(documentType.Id.Value);

        if (entity is null)
        {
            return;
        }

        _context.DocumentTypes.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(DocumentTypeId id)
    {
        return await _context.DocumentTypes.AnyAsync(e => e.Id == id.Value);
    }

    private static DocumentType MapToDomain(DocumentTypeEntity entity)
    {
        return DocumentType.Create(
            DocumentTypeId.Create(entity.Id),
            DocumentTypeName.Create(entity.Name ?? string.Empty),
            DocumentTypeCode.Create(entity.Code ?? string.Empty)
        );
    }

    private static DocumentTypeEntity MapToEntity(DocumentType documentType)
    {
        return new DocumentTypeEntity
        {
            Id = documentType.Id.Value,
            Name = documentType.Name.Value,
            Code = documentType.Code.Value
        };
    }
}
