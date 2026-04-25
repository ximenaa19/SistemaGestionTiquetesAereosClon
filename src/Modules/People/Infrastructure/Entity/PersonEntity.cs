// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Infrastructure\Entity\PersonEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.People.Infrastructure.Entity;

public class PersonEntity
{
    public int Id { get; set; }
    public int DocumentTypeId { get; set; }
    public string? DocumentNumber { get; set; }
    public string? FirstNames { get; set; }
    public string? LastNames { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public int? AddressId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

