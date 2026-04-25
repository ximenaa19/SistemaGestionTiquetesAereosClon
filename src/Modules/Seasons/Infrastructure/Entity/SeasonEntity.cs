// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Infrastructure\Entity\SeasonEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Seasons.Infrastructure.Entity;

public class SeasonEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal PriceFactor { get; set; }
}
