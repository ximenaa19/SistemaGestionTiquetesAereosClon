// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\DocumentTypes\Application\UseCases\GetDocumentTypeByCodeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.DocumentTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.DocumentTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.DocumentTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.DocumentTypes.Application.UseCases;

public class GetDocumentTypeByCodeUseCase
{
    private readonly IDocumentTypeRepository _repository;

    public GetDocumentTypeByCodeUseCase(IDocumentTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<DocumentType?> ExecuteAsync(string code)
    {
        return await _repository.GetByCodeAsync(DocumentTypeCode.Create(code));
    }
}
