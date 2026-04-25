// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\DocumentTypes\Application\UseCases\GetAllDocumentTypesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.DocumentTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.DocumentTypes.Domain.Repositories;

namespace GestionAerolineas.src.Modules.DocumentTypes.Application.UseCases;

public class GetAllDocumentTypesUseCase
{
    private readonly IDocumentTypeRepository _repository;

    public GetAllDocumentTypesUseCase(IDocumentTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DocumentType>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
