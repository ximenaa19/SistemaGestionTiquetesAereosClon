// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\DocumentTypes\Application\UseCases\GetDocumentTypeByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.DocumentTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.DocumentTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.DocumentTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.DocumentTypes.Application.UseCases;

public class GetDocumentTypeByIdUseCase
{
    private readonly IDocumentTypeRepository _repository;

    public GetDocumentTypeByIdUseCase(IDocumentTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<DocumentType?> ExecuteAsync(int id)
    {
        return await _repository.GetByIdAsync(DocumentTypeId.Create(id));
    }
}
