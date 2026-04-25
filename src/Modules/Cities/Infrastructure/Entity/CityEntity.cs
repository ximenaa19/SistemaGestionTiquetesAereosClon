// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Cities\Infrastructure\Entity\CityEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Cities.Infrastructure.Entity;

public class CityEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int RegionId { get; set; }
}


