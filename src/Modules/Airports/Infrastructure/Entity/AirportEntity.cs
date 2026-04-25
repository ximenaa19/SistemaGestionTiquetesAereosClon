// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Airports\Infrastructure\Entity\AirportEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Airports.Infrastructure.Entity;

public class AirportEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? IataCode { get; set; }
    public string? IcaoCode { get; set; }
    public int CityId { get; set; }
}
