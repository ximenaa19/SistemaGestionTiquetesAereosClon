// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\DocumentTypes\DocumentTypeModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.DocumentTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.DocumentTypes.Application.Services;
using GestionAerolineas.src.Modules.DocumentTypes.Application.UseCases;
using GestionAerolineas.src.Modules.DocumentTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.DocumentTypes.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.DocumentTypes;

public static class DocumentTypeModule
{
    public static DocumentTypeMenu Build(AppDbContext context)
    {
        var repository = new DocumentTypeRepository(context);
        IDocumentTypeValidator validator = new DocumentTypeValidator(repository);

        var create = new CreateDocumentTypeUseCase(repository, validator);
        var getAll = new GetAllDocumentTypesUseCase(repository);
        var getById = new GetDocumentTypeByIdUseCase(repository);
        var getByCode = new GetDocumentTypeByCodeUseCase(repository);
        var update = new UpdateDocumentTypeUseCase(repository, validator);
        var delete = new DeleteDocumentTypeUseCase(repository);

        return new DocumentTypeMenu(
            create,
            getAll,
            getById,
            getByCode,
            update,
            delete
        );
    }
}
