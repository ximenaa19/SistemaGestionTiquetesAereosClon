// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\DocumentTypes\Domain\Aggregate\DocumentType.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.DocumentTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.DocumentTypes.Domain.Aggregate;

public class DocumentType
{
    public DocumentTypeId Id { get; private set; }
    public DocumentTypeName Name { get; private set; }
    public DocumentTypeCode Code { get; private set; }

    private DocumentType(
        DocumentTypeId id,
        DocumentTypeName name,
        DocumentTypeCode code)
    {
        Id = id;
        Name = name;
        Code = code;
    }

    public static DocumentType Create(
        DocumentTypeId id,
        DocumentTypeName name,
        DocumentTypeCode code)
    {
        return new DocumentType(id, name, code);
    }

    public static DocumentType Create(
        DocumentTypeName name,
        DocumentTypeCode code)
    {
        return new DocumentType(DocumentTypeId.CreateNew(), name, code);
    }

    public void UpdateName(DocumentTypeName name)
    {
        Name = name;
    }

    public void UpdateCode(DocumentTypeCode code)
    {
        Code = code;
    }
}
