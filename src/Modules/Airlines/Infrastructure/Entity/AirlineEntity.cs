// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airlines\Infrastructure\Entity\AirlineEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Airlines.Infrastructure.Entity;

public class AirlineEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? IataCode { get; set; }
    public int OriginCountryId { get; set; }
    public bool IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

