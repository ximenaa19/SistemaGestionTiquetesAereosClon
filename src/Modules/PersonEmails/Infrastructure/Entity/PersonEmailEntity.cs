// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonEmails\Infrastructure\Entity\PersonEmailEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.PersonEmails.Infrastructure.Entity;

public class PersonEmailEntity
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string? User { get; set; }
    public int EmailDomainId { get; set; }
    public bool IsPrimary { get; set; }
}

